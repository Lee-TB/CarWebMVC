namespace CarWebMVC.Services;

public interface ILuceneService<TEntity>
{
    public void Add(TEntity entity);
    public void AddRange(IEnumerable<TEntity> entities);
    public void Update(TEntity entity);
    public void UpdateRange(IEnumerable<TEntity> entities);
    public void Delete(TEntity entity);
    public void DeleteRange(IEnumerable<TEntity> entities);
    public void Commit();
    public void Clear();
    public IEnumerable<TSearchResult> Search<TSearchResult>(string query, int maxHits = 10);
}