using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisDemo.CommonCaches;
using RedisDemo.Entities;
using RedisDemo.Services;
using System.Text;

namespace RedisDemo.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IResposeCacheService _resposeCacheService;
    private readonly IDistributedCache _distributedCache;
    private readonly ProductService _productService;
    public ProductController(ProductService productService, IDistributedCache distributedCache
      , IResposeCacheService resposeCacheService)
    {
      _resposeCacheService = resposeCacheService;
      _distributedCache = distributedCache;
      _productService = productService;
    }
    [HttpGet("GetAllProduct")]
    [Cache(1000)]
    public async Task<IActionResult> GetAll()
    {
      try
      {
        var content = HttpContext.Session.GetString("Keycontent");
        if (HttpContext.Session.GetString("Keycontent") != null)
        {
          var contentRes = await _distributedCache.GetStringAsync(HttpContext.Session.GetString("Keycontent"));
          var res = JsonConvert.DeserializeObject(contentRes);
          var listproduct = JsonConvert.DeserializeObject<List<Product>>(res.ToString());
          //foreach(var sub in res)
          //{
          //  Console.WriteLine(sub.Id, sub.Name);
          //}
          return Ok(listproduct);
        }
        return Ok(_productService.GetAllProduct());
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        return Ok(ex.Message);
      }
     
    }
    [HttpGet("Remove")]
    public async Task<IActionResult> RemoveCache()
    {
      try
      {
        await _resposeCacheService.RemoveCache("/api/Product/GetAllProduct");
        return Ok();
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return Ok();
      
    }
    private static string GenCachKey(HttpRequest request)
    {
      var keybuilder = new StringBuilder();
      keybuilder.Append($"/api/Product");
      foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
      {
        keybuilder.Append($"|{key}-{value}");
      }
      return keybuilder.ToString();
    }

  }
}
