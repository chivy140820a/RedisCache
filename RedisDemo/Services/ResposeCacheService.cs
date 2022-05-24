using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace RedisDemo.Services
{
  public class ResposeCacheService : IResposeCacheService
  {
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public ResposeCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
    {
      _connectionMultiplexer = connectionMultiplexer;
      _distributedCache = distributedCache;
    }
    public async Task<string> GetCacheResponse(string cacheKey)
    {
      var get = await _distributedCache.GetStringAsync(cacheKey);
      if (!string.IsNullOrEmpty(get))
      {
        return get;
      }
      return "";
    }

    public async Task RemoveCache(string partern)
    {
      if (string.IsNullOrEmpty(partern))
        throw new ArgumentException("Error");
      await foreach(var key in GetKey(partern + "*"))
      {
        await _distributedCache.RemoveAsync(key);
      }

    }
    private async IAsyncEnumerable<string> GetKey(string partern)
    {
      if (string.IsNullOrEmpty(partern))
        throw new ArgumentException("Error");
      foreach(var enpoin in _connectionMultiplexer.GetEndPoints())
      {
        var server = _connectionMultiplexer.GetServer(enpoin);
        foreach(var key in server.Keys(pattern:partern))
        {
          yield return key.ToString();
        }
      }
    }

    public async Task SetCacheResponse(string cacheKey, string response, TimeSpan timespan)
    {
      if (response == null)
      {
        return;
      }
      var responsecontent = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });
      await _distributedCache.SetStringAsync(cacheKey, responsecontent, new DistributedCacheEntryOptions()
      {
        AbsoluteExpirationRelativeToNow = timespan
      });
    }
  }
}
