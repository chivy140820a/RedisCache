namespace RedisDemo.Installers
{
  public interface IInstaller
  {
    void InstallerServices(IServiceCollection services, IConfiguration configuration);
  }
}
