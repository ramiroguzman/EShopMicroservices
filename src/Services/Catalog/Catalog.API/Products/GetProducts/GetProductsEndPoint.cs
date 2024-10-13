namespace Catalog.API.Products.GetProducts;

public class GetProductsEndPoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = result.Adapt<GetProductsResult>();
            return Results.Ok(response);
        }).WithName("GetProducts")
        .Produces<IEnumerable<Product>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get all products")
        .WithDescription("Get all products in the catalog");
    }
}
