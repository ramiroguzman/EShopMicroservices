namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndPoint : ICarterModule 
{
      public void AddRoutes(IEndpointRouteBuilder app)
      {
         app.MapGet("/products/{id}", async (ISender sender, Guid id) =>
         {
            try{
               var result = await sender.Send(new GetProductByIdQuery(id));
               var response = result.Adapt<GetProductByIdResponse>();
               return Results.Ok(response);
            }
            catch(ProductNotFoundException)
            {
               return Results.NotFound();
            }
             
         }).WithName("GetProductById")
         .Produces<Product>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithSummary("Get product by id")
         .WithDescription("Get a product in the catalog by its id");
      }
}