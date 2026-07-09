using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ts.Common.AppInterfaces;
using Ts.Common.Constant.AppConstants;
using Ts.Common.Constant.SiteConstants;
using Ts.Domain.HelperModels;
using Ts.Dto;
using Ts.Dto.OrderStatusDtos;
using Ts.Dto.PaymentStatusDtos;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.DataTableModels.OrderDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Dto.DataTableDtos.OrderDataTableDtos;
using Ts.ShopIn.Dto.HomeDtos;
using Ts.ShopIn.Dto.OrderDetailDtos;
using Ts.ShopIn.Dto.OrderDtos;
using Ts.ShopIn.Dto.PaymentDtos;
using Ts.ShopIn.Dto.ProductDetailDocumentDtos;
using Ts.ShopIn.Service.DataInterfaces;
using Ts.ShopIn.Service.HelperModel;
using Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;

namespace Ts.ShopIn.Service.DataServices
{
    public class OrderService : IOrderService
    {
        private readonly IRazorpayService razorpayService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IShopCategoryService shopCategoryService;
        private readonly IConfiguration configuration;
        private readonly IRandomService randomService;
        private readonly IDropDownService dropDownService;
        private readonly ILogger<OrderService> logger;
        private readonly IEmailShopInMessageService emailShopInMessageService;
        private readonly IMapper mapper;
        private readonly IFileClient fileClient;
        private readonly string apiKey;
        private readonly string keySecret;
        private readonly string webhookSecret;

        public OrderService(IRazorpayService razorpayService, IUnitOfWork unitOfWork,
            IShopCategoryService shopCategoryService, IConfiguration configuration,
            IRandomService randomService, IDropDownService dropDownService, ILogger<OrderService> logger,
            IEmailShopInMessageService emailShopInMessageService, IMapper mapper,
            IFileClient fileClient)
        {
            this.razorpayService = razorpayService;
            this.unitOfWork = unitOfWork;
            this.shopCategoryService = shopCategoryService;
            this.configuration = configuration;
            this.randomService = randomService;
            this.dropDownService = dropDownService;
            this.logger = logger;
            this.emailShopInMessageService = emailShopInMessageService;
            this.mapper = mapper;
            this.fileClient = fileClient;
            apiKey = configuration.GetValue<string>("Razorpay:ApiKey");
            keySecret = configuration.GetValue<string>("Razorpay:KeySecret");
            webhookSecret = configuration.GetValue<string>("Razorpay:WebhookSecret");
        }

        public async Task<ResponseMessageDto<GetRazorpayOrderDto>> CreateRazorpayOrderAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetRazorpayOrderDto>();

            var user = await unitOfWork.ClientUserRepo.GetForCartOrderAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                responseResult.ErrorMessage.Add("User not found.");
                return responseResult;
            }

            var userCartData = await CartCommonService.GetCartByUserIdAsync(unitOfWork, shopCategoryService, configuration, userId).ConfigureAwait(false);
            if (userCartData.TotalAvailableItemCount == 0)
            {
                responseResult.ErrorMessage.Add("Please add available item to you cart.");
                return responseResult;
            }

            var paymentModes = await dropDownService.GetPaymentModesAsync().ConfigureAwait(false);
            var paymentMode = paymentModes.FirstOrDefault(x => x.Name == PaymentModeConstant.Online);

            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);

            var currencyTypes = await dropDownService.GetCurrencyTypesAsync().ConfigureAwait(false);
            var currencyType = currencyTypes.FirstOrDefault(x => x.Letter == userCartData.CurrencyLetter);

            var paymentGatewayTypes = await dropDownService.GetPaymentGatewayTypesAsync().ConfigureAwait(false);
            var paymentGatewayType = paymentGatewayTypes.FirstOrDefault(x => x.Name == PaymentGatewayTypeConstant.Razorpay);

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var siteSetting = await unitOfWork.NextOrderSettingRepo.GetWithLockFirstOrDefaultAsync().ConfigureAwait(false);
                if (siteSetting == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Site setting not found. Please contact admin.");
                    return responseResult;
                }

                var orderNumber = randomService.GenerateOrderNumber(user.FirstName, 3, siteSetting.NextOrderNumber.ToString());

                var orderStatusHelperModels = new List<OrderDetailStatusHelperModel>();
                OrderStatusDto orderStatusDto;
                RazorPayOrderHelperModel razorpayOrder = null;
                var currentUtcNow = DateTime.UtcNow;

                if (userCartData.TotalAvailableItemAmount > 0)
                {
                    razorpayOrder = await razorpayService.CreateOrderAsync(userCartData.TotalAvailableItemAmount, userCartData.CurrencyLetter, orderNumber, apiKey, keySecret).ConfigureAwait(false);

                    if (razorpayOrder == null)
                    {
                        await unitOfWork.RollbackAsync().ConfigureAwait(false);
                        responseResult.ErrorMessage.Add("Failed to create order. Please try again.");
                        return responseResult;
                    }

                    orderStatusDto = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderInitiated);
                    orderStatusHelperModels.Add(new()
                    {
                        OrderStatusId = orderStatusDto.Id,
                        AddedOn = currentUtcNow
                    });
                }
                else
                {
                    orderStatusDto = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderCompleted);
                    orderStatusHelperModels.Add(new()
                    {
                        OrderStatusId = orderStatusDto.Id,
                        AddedOn = currentUtcNow
                    });
                }

                var productDescription = $"OrderNumber: {orderNumber}";
                var clientName = $"{user.FirstName} {user.LastName}".Trim();

                var order = new Order
                {
                    OrderNumber = orderNumber,
                    ClientName = clientName,
                    PaymentModeId = paymentMode.Id,
                    TotalItemCount = userCartData.TotalAvailableItemCount,
                    CurrencyTypeId = currencyType.Id,
                    TotalAmount = userCartData.TotalAvailableItemAmount,
                    UserId = userId,
                    OrderedOn = currentUtcNow,
                    OrderDetails = userCartData.ProductDetails.Where(x => x.ProductDetail.IsAvailable).Select(x => new OrderDetail
                    {
                        OrderStatusId = orderStatusDto.Id,
                        ProductDetailId = x.ProductDetailId,
                        Title = x.ProductDetail.Title,
                        ItemCount = x.ItemCount,
                        CurrencyTypeId = currencyType.Id,
                        ItemPrice = x.ProductDetail.Price,
                        TotalPrice = x.TotalPrice,
                        ShopCategoryId = x.ProductDetail.ShopCategoryId,
                        ExchangePolicyId = x.ProductDetail.ExchangePolicyId,
                        DeliveryPolicyId = x.ProductDetail.DeliveryPolicyId,
                        ReturnPolicyId = x.ProductDetail.ReturnPolicyId,
                        CreatedOn = currentUtcNow,
                        UpdatedOn = orderStatusDto.Name == OrderStatusConstant.OrderCompleted ? currentUtcNow : null,
                        OrderDetailStatusHistories = new()
                        {
                            OrderStatuses = JsonConvert.SerializeObject(orderStatusHelperModels)
                        }
                    }).ToList()
                };

                siteSetting.NextOrderNumber += 1;

                if (userCartData.TotalAvailableItemAmount > 0)
                {
                    order.Payment = new()
                    {
                        Amount = userCartData.TotalAvailableItemAmount,
                        CurrencyTypeId = currencyType.Id,
                        Razorpay_Order_Id = razorpayOrder.Id,
                        PaymentGatewayTypeId = paymentGatewayType.Id,
                        CreatedOn = currentUtcNow
                    };
                }
                else
                {
                    await unitOfWork.CartRepo.DeleteByUserIdAsync(userId).ConfigureAwait(false);
                }

                unitOfWork.OrderRepo.Create(order);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                await unitOfWork.CommitAsync().ConfigureAwait(false);

                if (userCartData.TotalAvailableItemAmount > 0)
                {
                    responseResult.Data = new()
                    {
                        RazorpayApiKey = apiKey,
                        IsPaymentRequired = true,
                        Order_Id = razorpayOrder.Id,
                        ClientName = clientName,
                        ClientEmail = user.Email,
                        ClientFullPhoneCodeNumber = user.PhoneNumberConfirmed ? $"{user.PhoneCode}{user.PhoneNumber}" : null,
                        OrderNumber = orderNumber,
                        Description = productDescription
                    };
                }
                else
                {
                    responseResult.Data = new();
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Error creating order.");
                responseResult.ErrorMessage.Add("Database operation failed. please try again.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> VerifyRazorpayOrderPaymentAsync(VerifyRazorpayPaymentDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<bool>();

            var razorpayPayment = await razorpayService.VerifyOrderPaymentAsync(modelDto.Razorpay_Payment_Id, modelDto.Razorpay_Order_Id, modelDto.Razorpay_Signature, apiKey, keySecret).ConfigureAwait(false);
            if (razorpayPayment == null)
            {
                responseResult.ErrorMessage.Add("Payment verification failed. Please check your order page after sometime.");
                return responseResult;
            }

            var paymentGatewayTypes = await dropDownService.GetPaymentGatewayTypesAsync().ConfigureAwait(false);
            var paymentStatuses = await dropDownService.GetPaymentStatusesAsync().ConfigureAwait(false);
            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entity = await unitOfWork.PaymentRepo.GetRazorpayPaymentWithLockByAsync(null, razorpayPayment.OrderId, userId).ConfigureAwait(false);
                if (entity == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Payment not found.");
                    return responseResult;
                }

                var paymentGatewayType = paymentGatewayTypes.FirstOrDefault(x => x.Id == entity.PaymentGatewayTypeId);

                var razorpayPaymentStatus = paymentStatuses.Where(x => x.PaymentGatewayTypeId == paymentGatewayType.Id);
                var oldPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Id == entity.PaymentStatusId);
                var newPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Name.Equals(razorpayPayment.Status, StringComparison.OrdinalIgnoreCase));

                var orderStatus = orderStatuses.FirstOrDefault(x => x.Name == RazorpayPaymentStatusConstant.GetOrderStatusConstantBy(newPaymentStatus.Name));

                var isValidTransition = RazorpayPaymentStatusConstant.IsValidTransition(oldPaymentStatus?.Name, newPaymentStatus.Name);

                if (newPaymentStatus.Name != RazorpayPaymentStatusConstant.Failed)
                    await unitOfWork.CartRepo.DeleteByUserIdAsync(userId).ConfigureAwait(false);

                if (entity.Razorpay_Created_At < razorpayPayment.CreatedAt || isValidTransition)
                {
                    SetPaymentCommon(razorpayPayment, orderStatus, entity, newPaymentStatus);
                    await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                    if (newPaymentStatus.Name != RazorpayPaymentStatusConstant.Authorized)
                        _ = emailShopInMessageService.NotifyOrderStatusAsync(RazorpayPaymentStatusConstant.GetPaymentStatusConstantStatusMessageBy(newPaymentStatus.Name), entity.Order.OrderNumber, entity.Order.OrderedBy.Email, entity.Order.ClientName, logger).ConfigureAwait(false);
                }

                await unitOfWork.CommitAsync().ConfigureAwait(false);
                responseResult.Data = true;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Error verifying payment.");
                responseResult.ErrorMessage.Add("Payment is successfull, database operation failed. Please check your order after sometime.");
            }

            return responseResult;
        }

        public async Task<ResponseMessageDto<GetPaymentStatusDto>> GetRazorpayPaymentStatusAsync(string razorpayPaymentId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<GetPaymentStatusDto>();

            var razorpayPayment = await razorpayService.GetPaymentByPaymentIdAsync(razorpayPaymentId, apiKey, keySecret).ConfigureAwait(false);
            if (razorpayPayment == null)
            {
                responseResult.ErrorMessage.Add("Payment verification failed. Please contact admin.");
                return responseResult;
            }

            var paymentGatewayTypes = await dropDownService.GetPaymentGatewayTypesAsync().ConfigureAwait(false);
            var paymentStatuses = await dropDownService.GetPaymentStatusesAsync().ConfigureAwait(false);
            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entity = await unitOfWork.PaymentRepo.GetRazorpayPaymentWithLockByAsync(razorpayPaymentId, razorpayPayment.OrderId, userId).ConfigureAwait(false);
                if (entity == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Payment not found.");
                    return responseResult;
                }

                var paymentGatewayType = paymentGatewayTypes.FirstOrDefault(x => x.Id == entity.PaymentGatewayTypeId);

                if (paymentGatewayType.Name == PaymentGatewayTypeConstant.Razorpay)
                {
                    var razorpayPaymentStatus = paymentStatuses.Where(x => x.PaymentGatewayTypeId == paymentGatewayType.Id);

                    var oldPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Id == entity.PaymentStatusId);
                    var newPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Name == razorpayPayment.Status);

                    var orderStatus = orderStatuses.FirstOrDefault(x => x.Name == RazorpayPaymentStatusConstant.GetOrderStatusConstantBy(newPaymentStatus.Name));

                    var paymentStatusMessage = RazorpayPaymentStatusConstant.GetPaymentStatusConstantStatusMessageBy(newPaymentStatus.Name);

                    var isValidTransition = RazorpayPaymentStatusConstant.IsValidTransition(oldPaymentStatus?.Name, newPaymentStatus.Name);

                    if (entity.Razorpay_Created_At < razorpayPayment.CreatedAt || isValidTransition)
                    {
                        SetPaymentCommon(razorpayPayment, orderStatus, entity, newPaymentStatus);

                        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                        await unitOfWork.CommitAsync().ConfigureAwait(false);

                        if (newPaymentStatus.Name != RazorpayPaymentStatusConstant.Authorized)
                            _ = emailShopInMessageService.NotifyOrderStatusAsync(paymentStatusMessage, entity.Order.OrderNumber, entity.Order.OrderedBy.Email, entity.Order.ClientName, logger).ConfigureAwait(false);
                    }
                    else
                    {
                        await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    }

                    responseResult.Data = new()
                    {
                        PaymentStatus = paymentStatusMessage,
                        OrderNumber = entity.Order.OrderNumber
                    };
                }
                else
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("No Payment Gateway Type found for the payment.");
                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Error fetching payment status.");
                responseResult.ErrorMessage.Add("Database operation failed. Please check your order after sometime.");
            }

            return responseResult;
        }

        public async Task<IEnumerable<GetOrderForViewDto>> GetForListViewAsync(OrderDataTableForViewRequestDto modelDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var result = new List<GetOrderForViewDto>();

            var currencyTypes = await dropDownService.GetCurrencyTypesAsync().ConfigureAwait(false);
            var orderstatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);
            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var shopCategories = await shopCategoryService.GetAllAsync().ConfigureAwait(false);

            var model = mapper.Map<OrderDataTableForViewRequest>(modelDto);
            var entities = await unitOfWork.OrderRepo.GetForListViewAsync(model, userId, orderstatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderInitiated).Id).ConfigureAwait(false);

            foreach (var order in entities)
            {
                var orderForViewDto = mapper.Map<GetOrderForViewDto>(order);

                var currencyType = currencyTypes.FirstOrDefault(x => x.Id == order.CurrencyTypeId);
                orderForViewDto.CurrencyType = currencyType;

                orderForViewDto.TotalAmount = CommonProductDetailService.RoundingToDecimal(order.TotalAmount, currencyType.Letter);

                orderForViewDto.OrderDetailsForView = [];
                foreach (var orderDetail in order.OrderDetails)
                {
                    var orderDetailForViewDto = mapper.Map<GetOrderDetailForViewDto>(orderDetail);

                    var orderStatus = orderstatuses.FirstOrDefault(x => x.Id == orderDetail.OrderStatusId);
                    orderDetailForViewDto.OrderStatus = orderStatus;

                    var currencyTypeOrderDetail = currencyTypes.FirstOrDefault(x => x.Id == order.CurrencyTypeId);
                    orderDetailForViewDto.CurrencyType = currencyTypeOrderDetail;

                    orderDetailForViewDto.ItemPrice = CommonProductDetailService.RoundingToDecimal(orderDetail.ItemPrice, currencyTypeOrderDetail.Letter);
                    orderDetailForViewDto.TotalPrice = CommonProductDetailService.RoundingToDecimal(orderDetail.TotalPrice, currencyTypeOrderDetail.Letter);

                    var deliveryPolicy = deliveryPolicies.FirstOrDefault(x => x.Id == orderDetail.DeliveryPolicyId);
                    orderDetailForViewDto.DeliveryPolicy = deliveryPolicy;

                    var downloadableIds = CommonProductDetailService.GetDownloadableIdsForDeliveryPolicy(deliveryPolicies);
                    if ((orderStatus.Name == OrderStatusConstant.OrderCompleted || orderStatus.Name == OrderStatusConstant.OrderPaymentRefundFailed) && downloadableIds.Contains(orderDetail.DeliveryPolicyId))
                    {
                        orderDetailForViewDto.IsDownloadable = true;
                        var days = (DateTime.UtcNow - orderDetail.UpdatedOn).Value.Days;
                        if (days < deliveryPolicy.DeliveryInDays)
                        {
                            orderDetailForViewDto.IsDownloadableRemainingDays = deliveryPolicy.DeliveryInDays - days;
                        }
                    }

                    if (orderStatus.Name == OrderStatusConstant.OrderPaymentRefunded)
                    {
                        orderForViewDto.RefundedAmount += orderDetail.TotalPrice;
                    }

                    orderDetailForViewDto.ProductDetailForView = CommonProductDetailService.SetProductDetailForListViewDto(configuration, shopCategories, orderDetail.ProductDetail);
                    orderForViewDto.OrderDetailsForView.Add(orderDetailForViewDto);
                }

                if (orderForViewDto.RefundedAmount > 0)
                {
                    orderForViewDto.RefundedAmount = CommonProductDetailService.RoundingToDecimal(orderForViewDto.RefundedAmount, currencyType.Letter);
                }

                result.Add(orderForViewDto);
            }

            return result;
        }

        public async Task<ResponseMessageDto<DownloadDocumentDto>> GetProductDocumentDetailAsync(int orderDetailId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException(ExceptionMessageConstant.ValueNotSpecified, nameof(userId));

            var responseResult = new ResponseMessageDto<DownloadDocumentDto>();

            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);
            var orderStatusCompleted = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderCompleted);
            var orderStatusRefundFailed = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderPaymentRefundFailed);

            var entity = await unitOfWork.OrderDetailRepo.GetByAsync(orderDetailId, userId, orderStatusCompleted.Id, orderStatusRefundFailed.Id).ConfigureAwait(false);
            if (entity == null)
            {
                responseResult.ErrorMessage.Add("OrderDetail was not found.");
                return responseResult;
            }

            if (entity.ProductDetail == null)
            {
                responseResult.ErrorMessage.Add("ProductDetail was not found.");
                return responseResult;
            }

            if (entity.ProductDetail.ProductDetailDocument == null)
            {
                responseResult.ErrorMessage.Add("Document was not found.");
                return responseResult;
            }

            var deliveryPolicies = await dropDownService.GetDeliveryPoliciesAsync().ConfigureAwait(false);
            var deliveryPolicy = deliveryPolicies.FirstOrDefault(x => x.Id == entity.DeliveryPolicyId);

            var days = (DateTime.UtcNow - entity.UpdatedOn).Value.Days;
            if (days >= deliveryPolicy.DeliveryInDays)
            {
                responseResult.ErrorMessage.Add($"0 Days left out of {deliveryPolicy.DeliveryInDays}");
                return responseResult;
            }

            var dateUtc = DateTime.UtcNow;
            var downloadDocumentDto = new DownloadDocumentDto
            {
                Token = fileClient.GetEncryptedFileToken(dateUtc, RoleConstant.ShopInUser),
                FileUrl = fileClient.DownloadDocumentUrl,
                EncryptedFileName = fileClient.GetEncryptedFileName(entity.ProductDetail.ProductDetailDocument.Document, dateUtc),
                AuthType = "Basic"
            };
            responseResult.Data = downloadDocumentDto;

            return responseResult;
        }

        public async Task<ResponseMessageDto<bool>> ProcessRazorpayWebhookAsync(string requestBody, string receivedSignature)
        {
            var responseResult = new ResponseMessageDto<bool>();

            var razorpayPayment = await razorpayService.VerifyWebhookAsync(requestBody, receivedSignature, webhookSecret).ConfigureAwait(false);
            if (razorpayPayment == null)
            {
                responseResult.ErrorMessage.Add("Webhook payment verification failed.");
                return responseResult;
            }

            var paymentGatewayTypes = await dropDownService.GetPaymentGatewayTypesAsync().ConfigureAwait(false);
            var paymentStatuses = await dropDownService.GetPaymentStatusesAsync().ConfigureAwait(false);
            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);

            await unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
            try
            {
                var entity = await unitOfWork.PaymentRepo.GetRazorpayPaymentWithLockByAsync(null, razorpayPayment.OrderId, null).ConfigureAwait(false);
                if (entity == null)
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                    responseResult.ErrorMessage.Add("Payment not found.");
                    return responseResult;
                }

                var paymentGatewayType = paymentGatewayTypes.FirstOrDefault(x => x.Id == entity.PaymentGatewayTypeId);

                var razorpayPaymentStatus = paymentStatuses.Where(x => x.PaymentGatewayTypeId == paymentGatewayType.Id);
                var oldPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Id == entity.PaymentStatusId);
                var newPaymentStatus = razorpayPaymentStatus.FirstOrDefault(x => x.Name.Equals(razorpayPayment.Status, StringComparison.OrdinalIgnoreCase));

                var orderStatus = orderStatuses.FirstOrDefault(x => x.Name == RazorpayPaymentStatusConstant.GetOrderStatusConstantBy(newPaymentStatus.Name));

                var isValidTransition = RazorpayPaymentStatusConstant.IsValidTransition(oldPaymentStatus?.Name, newPaymentStatus.Name);

                if (entity.Razorpay_Created_At < razorpayPayment.CreatedAt || isValidTransition)
                {
                    SetPaymentCommon(razorpayPayment, orderStatus, entity, newPaymentStatus);

                    await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                    await unitOfWork.CommitAsync().ConfigureAwait(false);

                    if (newPaymentStatus.Name != RazorpayPaymentStatusConstant.Authorized)
                        _ = emailShopInMessageService.NotifyOrderStatusAsync(RazorpayPaymentStatusConstant.GetPaymentStatusConstantStatusMessageBy(newPaymentStatus.Name), entity.Order.OrderNumber, entity.Order.OrderedBy.Email, entity.Order.ClientName, logger).ConfigureAwait(false);
                }
                else
                {
                    await unitOfWork.RollbackAsync().ConfigureAwait(false);
                }

                responseResult.Data = true;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync().ConfigureAwait(false);
                logger.LogError(ex, "Error processing razorpay webhook.");
                responseResult.ErrorMessage.Add("Payment verification is successfull, database operation failed.");
            }

            return responseResult;
        }

        private static void SetPaymentCommon(RazorpayPaymentHelperModel razorpayPayment, OrderStatusDto orderStatus, Payment entity, PaymentStatusDto newPaymentStatus)
        {
            var currentUtcNow = DateTime.UtcNow;

            entity.Razorpay_Method = razorpayPayment.Method;
            entity.Razorpay_Payment_Id = razorpayPayment.Id;
            entity.Razorpay_Amount_Refunded = CommonProductDetailService.ConvertToDecimal(razorpayPayment.AmountRefunded, razorpayPayment.Currency);
            entity.Razorpay_Refund_Status = razorpayPayment.RefundStatus;
            entity.Razorpay_Created_At = razorpayPayment.CreatedAt;
            entity.PaymentStatusId = newPaymentStatus.Id;
            entity.UpdatedOn = currentUtcNow;

            var paymentStatusHelperModel = JsonConvert.DeserializeObject<List<PaymentStatusHelperModel>>(entity.PaymentStatusHistory?.PaymentStatuses ?? "[]");
            paymentStatusHelperModel.Add(new()
            {
                PaymentStatusId = newPaymentStatus.Id,
                AddedOn = currentUtcNow
            });

            if (entity.PaymentStatusHistory == null)
            {
                entity.PaymentStatusHistory = new()
                {
                    PaymentStatuses = JsonConvert.SerializeObject(paymentStatusHelperModel)
                };
            }
            else
            {
                entity.PaymentStatusHistory.PaymentStatuses = JsonConvert.SerializeObject(paymentStatusHelperModel);
            }

            entity.Order.OrderDetails.ForEach(x =>
            {
                if (x.OrderStatusId != orderStatus.Id)
                {
                    x.OrderStatusId = orderStatus.Id;
                    x.UpdatedOn = currentUtcNow;

                    var orderStatusHelperModel = JsonConvert.DeserializeObject<List<OrderDetailStatusHelperModel>>(x.OrderDetailStatusHistories?.OrderStatuses ?? "[]");
                    orderStatusHelperModel.Add(new()
                    {
                        OrderStatusId = orderStatus.Id,
                        AddedOn = currentUtcNow
                    });

                    if (x.OrderDetailStatusHistories == null)
                    {
                        x.OrderDetailStatusHistories = new()
                        {
                            OrderStatuses = JsonConvert.SerializeObject(orderStatusHelperModel)
                        };
                    }
                    else
                    {
                        x.OrderDetailStatusHistories.OrderStatuses = JsonConvert.SerializeObject(orderStatusHelperModel);
                    }
                }
            });
        }

        public async Task<decimal> GetTotalRevenue(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);
            var orderCompleted = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderCompleted);
            var orderRefundFailed = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderPaymentRefundFailed);

            return await unitOfWork.OrderDetailRepo.GetTotalPriceAsync(startDate, endDate, orderCompleted.Id, orderRefundFailed.Id).ConfigureAwait(false);
        }

        public async Task<ChartResponseDto> GetTotalSalesDataAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            var orderStatuses = await dropDownService.GetOrderStatusesAsync().ConfigureAwait(false);
            var orderCompleted = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderCompleted);
            var orderRefundFailed = orderStatuses.FirstOrDefault(x => x.Name == OrderStatusConstant.OrderPaymentRefundFailed);

            var data = await unitOfWork.OrderDetailRepo.GetTotalPriceByMonthsAsync(startDate, endDate, orderCompleted.Id, orderRefundFailed.Id).ConfigureAwait(false);

            var chartResponseDto = new ChartResponseDto
            {
                Labels = [.. data.Select(x => x.Month)],
                Datasets = [.. data.Select(x => x.TotalPrice)]
            };

            return chartResponseDto;
        }
    }
}
