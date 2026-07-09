using Ts.Dto.ApplicationUserDtos;
namespace Ts.ShopIn.Dto.ProductDtos
{
    public class GetProductDataTableDto : ProductDto
    {
        public ApplicationUserDto PublishedBy { get; set; }
        public ApplicationUserDto ProductWorker { get; set; }
    }
}
