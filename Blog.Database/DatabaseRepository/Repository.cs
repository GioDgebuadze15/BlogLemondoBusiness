using Blog.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blog.Database.DatabaseRepository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = ctx.Set<TEntity>();
    }


    public IQueryable<TEntity> GetAll()
        => _dbSet.AsQueryable();

    public IQueryable<TEntity> GetById(int id)
        => _dbSet.Find(id) is null
            ? throw new InvalidOperationException($"Entity with id {id} was not found.")
            : _dbSet.Where(x => x.Equals(_dbSet.Find(id))).AsQueryable();


    public async Task<TEntity> Add(TEntity entity)
    {
        _dbSet.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _ctx.SaveChangesAsync();
    }
}