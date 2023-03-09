namespace Data;

public interface IRepository<T> where T:class
{
    Task<List<T>> Get();

    Task<T> Get(int id);

    Task<T> Create(T entity);

    Task<T> Update(T entity);

    Task<T> Delete(int id);

    Task Save();
}