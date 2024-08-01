using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace CarWebMVC.Services;

public class LuceneService<TEntity> : ILuceneService<TEntity> where TEntity : class
{
    private readonly IndexWriter _writer;
    private readonly Lucene.Net.Analysis.Analyzer _analyzer;
    private readonly MultiFieldQueryParser _multiFieldQueryParser;

    public LuceneService(ILuceneWriter writer)
    {
        _writer = writer.Writer;
        _analyzer = writer.Analyzer;

        var fields = GetEntityFields();
        _multiFieldQueryParser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, fields, _analyzer);
    }

    public void Add(TEntity entity)
    {
        var doc = MapEntityToDocument(entity);
        _writer.AddDocument(doc);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Add(entity);
        }
    }

    public void Update(TEntity entity)
    {
        Delete(entity);
        Add(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        Term[] terms =
        [
            new Term("Id", entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString() ?? ""),
            new Term("EntityType", typeof(TEntity).Name)
        ];
        _writer.DeleteDocuments(terms);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }

    public void Clear()
    {
        var term = new Term("EntityType", typeof(TEntity).Name);
        _writer.DeleteDocuments(term);
    }

    public void Commit()
    {
        _writer.Commit();
    }

    public IEnumerable<TResult> Search<TResult>(string query, int maxHits = 10)
    {
        var resultList = new List<TResult>();
        var searcher = new IndexSearcher(_writer.GetReader(applyAllDeletes: true));
        var parsedQuery = _multiFieldQueryParser.Parse(query);
        var booleanQuery = new BooleanQuery
        {
            { parsedQuery, Occur.MUST },
            { new TermQuery(new Term("EntityType", typeof(TEntity).Name)), Occur.MUST }
        };
        var hits = searcher.Search(booleanQuery, maxHits);
        foreach (var hit in hits.ScoreDocs)
        {
            Document doc = searcher.Doc(hit.Doc);
            var result = Activator.CreateInstance<TResult>();
            foreach (var p in typeof(TResult).GetProperties())
            {
                if (p.PropertyType == typeof(string))
                {
                    p.SetValue(result, doc.Get(p.Name));
                }
                else
                {
                    var convertedValue = Convert.ChangeType(doc.Get(p.Name), p.PropertyType);
                    p.SetValue(result, convertedValue);
                }
            }
            resultList.Add(result);
        }
        return resultList;
    }

    private string[] GetEntityFields()
    {
        var fields = new List<string>();
        Type entityType = typeof(TEntity);
        foreach (var p in entityType.GetProperties())
        {
            if (p.PropertyType == typeof(string))
            {
                fields.Add(p.Name);
            }
        }
        return fields.ToArray();
    }

    private Document MapEntityToDocument(TEntity entity)
    {
        var doc = new Document();
        Type entityType = typeof(TEntity);
        doc.Add(new StringField("EntityType", typeof(TEntity).Name, Field.Store.YES));
        foreach (var p in entityType.GetProperties())
        {
            if (p.PropertyType == typeof(string))
            {
                doc.Add(new TextField(p.Name, p.GetValue(entity)?.ToString() ?? "", Field.Store.YES));
            }
            else
            {
                doc.Add(new StringField(p.Name, p.GetValue(entity)?.ToString() ?? "", Field.Store.YES));
            }
        }
        return doc;
    }
}