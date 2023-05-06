namespace Blog.Database.DatabaseRepository;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetById(int id);
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task Delete(TEntity entity);
}