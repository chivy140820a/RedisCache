using RedisDemo.CommonIOptions;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using RedisDemo.Services;

namespace RedisDemo.Installers
{
  public class CachRedisInstaller : IInstaller
  {
    public void InstallerServices(IServiceCollection services, IConfiguration configuration)
    {
      var redisconfiguration = new RedisConfiguration();
      configuration.GetSection("RedisConfiguration").Bind(redisconfiguration);
      services.AddSingleton(redisconfiguration);
      if (!redisconfiguration.Enabled)
      {
        return;
      }
      services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisconfiguration.ConnectionString));
      services.AddStackExchangeRedisCache(option => option.Configuration = redisconfiguration.ConnectionString);
      services.AddSingleton<IResposeCacheService, ResposeCacheService>();
    }
  }
}
