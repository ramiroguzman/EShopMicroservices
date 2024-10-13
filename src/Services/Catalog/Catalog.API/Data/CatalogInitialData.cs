using System.Runtime.CompilerServices;
using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
   public async Task Populate(IDocumentStore store, CancellationToken cancellationToken)
   {
      using var session = store.LightweightSession();
      if (await session.Query<Product>().AnyAsync())
      {
         return;
      }
      session.Store<Product>(GetPreconfiguredProducts());
      await session.SaveChangesAsync(cancellationToken);
   }
   private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>{
      new Product()
      {
         Id = new Guid("6f4f05cb-b1fb-479f-a384-37e3b44d030f"),
         Name = "IPhone X",
         Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
         Price = 100,
         ImageFile = "product-1.png"
      },
   };

}

