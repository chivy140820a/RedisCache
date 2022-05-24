using RedisDemo.Entities;

namespace RedisDemo.Services
{
  public class ProductService
  {
    public List<Product> GetAllProduct()
    {
      var list = new List<Product>();
      for(int i = 0; i < 10; i++)
      {
        list.Add(new Product()
        {
          Id = i,
          Name = $"Product{i}"
        });
        
      }
      return list;
    }
  }
}
