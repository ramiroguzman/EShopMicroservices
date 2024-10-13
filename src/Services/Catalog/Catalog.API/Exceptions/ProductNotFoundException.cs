using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;


internal class ProductNotFoundException(Guid Id) : NotFoundException("Product", Id)
{
}