namespace RedisDemo.Installers
{
  public class SystemInstallers : IInstaller
  {
    public void InstallerServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddControllers();
      services.AddMvc();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }
  }
}
