namespace RedisDemo.Installers
{
  public static class InstallerExtenstions
  {
    public static void InstallerServiceAssembly(this IServiceCollection services, IConfiguration configuration)
    {
      var installs = typeof(Program).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface
      && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
      installs.ForEach(installs => installs.InstallerServices(services,configuration));
    }
  }
}
