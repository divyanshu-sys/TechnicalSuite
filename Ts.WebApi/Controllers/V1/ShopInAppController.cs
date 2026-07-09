using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Dto;
using Ts.ShopIn.Dto.HomeDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ShopInAppController : ControllerBase
    {
        private readonly IEmailShopInMessageService emailShopInMessageService;
        private readonly ILogger<ShopInAppController> logger;
        private readonly IHomeService homeService;

        public ShopInAppController(IEmailShopInMessageService emailShopInMessageService,
            ILogger<ShopInAppController> logger, IHomeService homeService)
        {
            this.emailShopInMessageService = emailShopInMessageService;
            this.logger = logger;
            this.homeService = homeService;
        }

        [Authorize(Roles = RoleConstant.ShopInUser)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult ShopIn(ContactFormDto modelDto)
        {
            _ = emailShopInMessageService.NotifyContactFormPublicAsync(modelDto, logger).ConfigureAwait(false);
            return Accepted();
        }

        [Authorize(Roles = $"{RoleConstant.ShopInUser}")]
        [HttpGet("home-list-for-view")]
        [ProducesResponseType(typeof(DisplayHomeItemsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayItems()
        {
            return Ok(await homeService.DisplayItemsAsync().ConfigureAwait(false));
        }
    }
}
