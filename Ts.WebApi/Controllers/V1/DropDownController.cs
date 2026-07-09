using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppConstants;
using Ts.Dto;
using Ts.Service.DataInterfaces;
namespace Ts.WebApi.Controllers.V1
{
    [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class DropDownController : ControllerBase
    {
        private readonly IDropDownService dropDownService;

        public DropDownController(IDropDownService dropDownService)
        {
            this.dropDownService = dropDownService;
        }

        [HttpGet("hreflang")]
        [ProducesResponseType(typeof(IEnumerable<DropdownItemDto>), StatusCodes.Status200OK)]
        public IActionResult GetHrefLang()
        {
            return Ok(dropDownService.GetHrefLangs());
        }

        [HttpGet("exchangepolicy")]
        [ProducesResponseType(typeof(IEnumerable<DropdownItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExchangePolicyDropDown()
        {
            return Ok(await dropDownService.GetExchangePolicyDropDownAsync().ConfigureAwait(false));
        }

        [HttpGet("deliverypolicy")]
        [ProducesResponseType(typeof(IEnumerable<DropdownItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeliveryPolicyDropDown()
        {
            return Ok(await dropDownService.GetDeliveryPolicyDropDownAsync().ConfigureAwait(false));
        }

        [HttpGet("returnpolicy")]
        [ProducesResponseType(typeof(IEnumerable<DropdownItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnPolicyDropDown()
        {
            return Ok(await dropDownService.GetReturnPolicyDropDownAsync().ConfigureAwait(false));
        }
    }
}
