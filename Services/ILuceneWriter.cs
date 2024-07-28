namespace CarWebMVC.Services;

public interface ILuceneWriter
{
    public Lucene.Net.Index.IndexWriter Writer { get; }
    public Lucene.Net.Analysis.Analyzer Analyzer { get; }
}