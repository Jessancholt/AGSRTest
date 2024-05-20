namespace Test.Core.Services.Interfaces
{
    public interface ICacheService<TKey, TValue>
    {
        Task<TValue> Get(TKey key, Func<Task<TValue>> action);

        Task<List<TValue>> Get(TKey key, Func<Task<List<TValue>>> action);
    }
}
