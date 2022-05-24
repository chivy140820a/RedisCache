using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RedisDemo.CommonIOptions;
using RedisDemo.Entities;
using RedisDemo.Services;
using System.Text;

namespace RedisDemo.CommonCaches
{
  public class CacheAttribute : Attribute, IAsyncActionFilter
  {
    private readonly int _timetoliveSecond;
    public CacheAttribute(int timetoliveSecond= 1000)
    {
      _timetoliveSecond = timetoliveSecond;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var catcheconfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
      if (!catcheconfiguration.Enabled)
      {
        await next();
        return;
      }
      var cachekey = GenCachKey(context.HttpContext.Request);
      var catecheService = context.HttpContext.RequestServices.GetRequiredService<IResposeCacheService>();
      var catcheResponse =await catecheService.GetCacheResponse(cachekey);
      if (!string.IsNullOrEmpty(catcheResponse))
      {
        var contentRes = new ContentResult()
        {
          Content = catcheResponse,
          ContentType = "application/json",
          StatusCode = 200
        };
        context.HttpContext.Session.SetString("Keycontent",cachekey);
        await next();
        //context.Result = contentRes;
      }
      else
      {
        var ex = await next();
        if (ex.Result is OkObjectResult okResult)
        {
          var content = (List<Product>)okResult.Value;
          await catecheService.SetCacheResponse(cachekey,JsonConvert.SerializeObject(content), TimeSpan.FromSeconds(_timetoliveSecond));
        }  
      }
    }
    private static string GenCachKey(HttpRequest request)
    {
      var keybuilder = new StringBuilder();
      keybuilder.Append($"{request.Path}");
      foreach(var(key,value) in request.Query.OrderBy(x => x.Key))
      {
        keybuilder.Append($"|{key}-{value}");
      }
      return keybuilder.ToString();
    }
  }
}
