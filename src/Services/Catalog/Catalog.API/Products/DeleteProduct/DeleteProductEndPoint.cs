namespace Catalog.API.Products.DeleteProduct;

   // public record DeleteProductRequest(Guid Id):ICommand;
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndPoint: ICarterModule
    {
         public void AddRoutes(IEndpointRouteBuilder  app)
         {
               app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
               {
                  var result = await sender.Send(new DeleteProductCommand(id));
                  var response = result.Adapt<DeleteProductResponse>();
                  return Results.Ok(response);
               })
               .WithName("DeleteProduct")
               .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .ProducesProblem(StatusCodes.Status404NotFound)
               .WithSummary("Delete a product")
               .WithDescription("Delete a product in the catalog by its id");
         }
        
    }
