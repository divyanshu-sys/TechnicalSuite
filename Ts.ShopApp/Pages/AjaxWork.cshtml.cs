using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ts.ShopIn.Client.HttpClientServices.ClientInterfaces;
namespace Ts.ShopApp.Pages
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class AjaxWorkModel : PageModel
    {
        private readonly IBlogForViewClient blogForViewClient;
        private readonly IProductDetailForViewClient productDetailForViewClient;
        private readonly IProductForViewClient productForViewClient;

        public AjaxWorkModel(IBlogForViewClient blogForViewClient,
            IProductDetailForViewClient productDetailForViewClient,
            IProductForViewClient productForViewClient)
        {
            this.blogForViewClient = blogForViewClient;
            this.productDetailForViewClient = productDetailForViewClient;
            this.productForViewClient = productForViewClient;
        }

        [FromQuery]
        public int Start { get; set; }

        [FromQuery]
        public int Length { get; set; }

        [FromQuery]
        public string SubCategoryName { get; set; }

        [FromQuery]
        public string ShopCategoryName { get; set; }

        [FromQuery]
        public string Search { get; set; }

        [FromQuery]
        public int BlogId { get; set; }

        [FromQuery]
        public int ProductDetailId { get; set; }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = await blogForViewClient.GetForListViewAsync(new()
            {
                Start = Start,
                Length = Length,
                SubCategoryName = SubCategoryName,
                Search = Search
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_BlogGalleryCasePartial", response.Data);
        }

        public async Task<IActionResult> OnPostUpdateBlogForViewCount()
        {
            var response = await blogForViewClient.UpdateBlogForViewCountAsync(BlogId, Request.Headers.ContainsKey("sec-ch-ua")).ConfigureAwait(false);
            if (response?.Data == true)
                return new NoContentResult();
            if (response == null)
                return BadRequest("Not a browser.");
            return BadRequest(response.ErrorMessage.ToHtmlBreakString());
        }

        public async Task<IActionResult> OnPostProductDetail()
        {
            var response = await productDetailForViewClient.GetForListViewAsync(new()
            {
                Start = Start,
                Length = Length,
                ShopCategoryName = ShopCategoryName,
                Search = Search
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_ProductDetailGalleryCasePartial", response.Data);
        }

        public async Task<IActionResult> OnPostUpdateProductDetailForViewCount()
        {
            var response = await productDetailForViewClient.UpdateProductDetailForViewCountAsync(ProductDetailId, Request.Headers.ContainsKey("sec-ch-ua")).ConfigureAwait(false);
            if (response?.Data == true)
                return new NoContentResult();
            if (response == null)
                return BadRequest("Not a browser.");
            return BadRequest(response.ErrorMessage.ToHtmlBreakString());
        }

        public async Task<IActionResult> OnPostProduct()
        {
            var response = await productForViewClient.GetForListViewAsync(new()
            {
                Start = Start,
                Length = Length,
                Search = Search
            }).ConfigureAwait(false);

            if (response.Data.Count() == 0)
                return new NoContentResult();
            return Partial("_ProductGalleryCasePartial", response.Data);
        }
    }
}
