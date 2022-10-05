namespace IBot.Core.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task<T?> GetAsync(Guid id);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task<List<T>> Find(int skip, int take);
}