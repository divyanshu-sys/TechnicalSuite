using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Dto;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class PublicAppController : ControllerBase
    {
        private readonly IEmailMessageService emailMessageService;
        private readonly IEmailShopInMessageService emailShopInMessageService;
        private readonly ILogger<PublicAppController> logger;

        public PublicAppController(IEmailMessageService emailMessageService,
            IEmailShopInMessageService emailShopInMessageService,
            ILogger<PublicAppController> logger)
        {
            this.emailMessageService = emailMessageService;
            this.emailShopInMessageService = emailShopInMessageService;
            this.logger = logger;
        }

        [Authorize(Roles = $"{RoleConstant.DotInSiteUser}, {RoleConstant.DotComSiteUser}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Post(ContactFormDto modelDto)
        {
            _ = emailMessageService.NotifyContactFormPublicAsync(modelDto, logger).ConfigureAwait(false);
            return Accepted();
        }
    }
}
