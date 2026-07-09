using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Ts.Application.AppConstants;
using Ts.Common.HelperExtensions;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.HomeDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.PaymentDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Service.DataInterfaces;

namespace Ts.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class OrderDotInController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ILogger<OrderDotInController> logger;

        public OrderDotInController(IOrderService orderService, ILogger<OrderDotInController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
        [HttpPost("create-razorpay-order")]
        [ProducesResponseType(typeof(GetRazorpayOrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateRazorpayOrder()
        {
            var responseResult = await orderService.CreateRazorpayOrderAsync(User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
        [HttpPost("verify-razorpay-order-payment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> VerifyRazorpayOrderPayment(VerifyRazorpayPaymentDto modelDto)
        {
            var responseResult = await orderService.VerifyRazorpayOrderPaymentAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data)
                return NoContent();
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
        [HttpGet("paymentstatus/{paymentId}")]
        [ProducesResponseType(typeof(GetPaymentStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPaymentStatus(string paymentId)
        {
            var responseResult = await orderService.GetRazorpayPaymentStatusAsync(paymentId, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
        [HttpPost("getbyuser")]
        [ProducesResponseType(typeof(IEnumerable<GetOrderForViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(OrderDataTableForViewRequestDto modelDto)
        {
            return Ok(await orderService.GetForListViewAsync(modelDto, User.Claims.GetUserId()).ConfigureAwait(false));
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.PublicScheme)]
        [HttpGet("document/{orderDetailId}")]
        [ProducesResponseType(typeof(DownloadDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetProductDocumentDetail(int orderDetailId)
        {
            var responseResult = await orderService.GetProductDocumentDetailAsync(orderDetailId, User.Claims.GetUserId()).ConfigureAwait(false);
            if (responseResult.Data != null)
                return Ok(responseResult.Data);
            return UnprocessableEntity(responseResult.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleWebhook()
        {
            string receivedSignature = Request.Headers["X-Razorpay-Signature"];

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string requestBody = await reader.ReadToEndAsync();

            var responseResult = await orderService.ProcessRazorpayWebhookAsync(requestBody, receivedSignature).ConfigureAwait(false);
            if (responseResult.Data)
                return Ok();
            logger.LogWarning("Failed to process Razorpay webhook: {ErrorMessage}", responseResult.ErrorMessage);
            logger.LogWarning("Received Razorpay webhook payload: {Payload}", requestBody);
            return BadRequest(responseResult.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("total-revenue")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalRevenue(DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            var response = await orderService.GetTotalRevenue(startDate, endDate).ConfigureAwait(false);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = AuthSchemeConstant.AdminScheme)]
        [Authorize(Roles = $"{RoleConstant.Administrator}, {RoleConstant.EmployeeShopIn}")]
        [Authorize(Policy = ProductPolicy.CanView)]
        [HttpGet("total-sales-data")]
        [ProducesResponseType(typeof(ChartResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalSalesData(DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            var response = await orderService.GetTotalSalesDataAsync(startDate, endDate).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
