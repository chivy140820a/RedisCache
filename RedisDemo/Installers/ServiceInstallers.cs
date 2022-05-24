using RedisDemo.Services;

namespace RedisDemo.Installers
{
  public class ServiceInstallers : IInstaller
  {
    public void InstallerServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddTransient<IResposeCacheService, ResposeCacheService>();
      services.AddSingleton<ProductService>();
    }
  }
}
