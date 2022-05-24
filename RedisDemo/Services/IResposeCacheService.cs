namespace RedisDemo.Services
{
  public interface IResposeCacheService
  {
    Task SetCacheResponse(string cacheKey, string response, TimeSpan timespan);
    Task<string> GetCacheResponse(string cacheKey);
    Task RemoveCache(string partern);
  }
}
