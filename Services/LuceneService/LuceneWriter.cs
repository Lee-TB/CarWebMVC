using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace CarWebMVC.Services;

public class LuceneWriter : ILuceneWriter
{
    private readonly IndexWriter _writer;
    private readonly Lucene.Net.Analysis.Analyzer _analyzer;

    public IndexWriter Writer => _writer;

    public Lucene.Net.Analysis.Analyzer Analyzer => _analyzer;

    public LuceneWriter()
    {
        var directory = FSDirectory.Open("LuceneIndex");
        _analyzer = new VietnameseAnalyzer(LuceneVersion.LUCENE_48);
        var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, _analyzer);
        _writer = new IndexWriter(directory, indexConfig);
    }
}