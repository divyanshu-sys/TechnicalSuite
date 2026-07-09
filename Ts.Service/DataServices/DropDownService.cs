using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Ts.Application.AppConstants;
using Ts.Domain.Interfaces;
using Ts.Dto;
using Ts.Dto.CurrencyTypeDtos;
using Ts.Dto.DeliveryPolicyDtos;
using Ts.Dto.ExchangePolicyDtos;
using Ts.Dto.OrderStatusDtos;
using Ts.Dto.PaymentGatewayTypeDtos;
using Ts.Dto.PaymentModeDtos;
using Ts.Dto.PaymentStatusDtos;
using Ts.Dto.ReturnPolicyDtos;
using Ts.Service.DataInterfaces;

namespace Ts.Service.DataServices
{
    public class DropDownService : IDropDownService
    {
        private const string ExchangePolicyCacheKey = "ExchangePolicy";
        private const string DeliveryPolicyCacheKey = "DeliveryPolicy";
        private const string ReturnPolicyCacheKey = "ReturnPolicy";
        private const string PaymentModeCacheKey = "PaymentMode";
        private const string PaymentGatewayTypeCacheKey = "PaymentGatewayType";
        private const string OrderStatusCacheKey = "OrderStatus";
        private const string PaymentStatusCacheKey = "PaymentStatus";
        private const string CurrencyTypeCacheKey = "CurrencyType";
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;

        public DropDownService(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IEnumerable<ExchangePolicyDto>> GetExchangePoliciesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(ExchangePolicyCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<ExchangePolicyDto>>(cachedEntity);

            var entities = await unitOfWork.ExchangePolicyRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<ExchangePolicyDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(ExchangePolicyCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<DropdownItemDto>> GetExchangePolicyDropDownAsync()
        {
            var dtos = await GetExchangePoliciesAsync().ConfigureAwait(false);

            var modelDto = dtos
                        .Select(x => new DropdownItemDto
                        {
                            Key = x.Id,
                            Value = x.Name
                        });

            return modelDto;
        }

        public async Task<IEnumerable<DeliveryPolicyDto>> GetDeliveryPoliciesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(DeliveryPolicyCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<DeliveryPolicyDto>>(cachedEntity);
            var entities = await unitOfWork.DeliveryPolicyRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<DeliveryPolicyDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(DeliveryPolicyCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<DropdownItemDto>> GetDeliveryPolicyDropDownAsync()
        {
            var dtos = await GetDeliveryPoliciesAsync().ConfigureAwait(false);

            var modelDto = dtos
                        .Select(x => new DropdownItemDto
                        {
                            Key = x.Id,
                            Value = x.Name
                        });

            return modelDto;
        }

        public async Task<IEnumerable<ReturnPolicyDto>> GetReturnPoliciesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(ReturnPolicyCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<ReturnPolicyDto>>(cachedEntity);
            var entities = await unitOfWork.ReturnPolicyRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<ReturnPolicyDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(ReturnPolicyCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<DropdownItemDto>> GetReturnPolicyDropDownAsync()
        {
            var dtos = await GetReturnPoliciesAsync().ConfigureAwait(false);

            var modelDto = dtos
                        .Select(x => new DropdownItemDto
                        {
                            Key = x.Id,
                            Value = x.Name
                        });

            return modelDto;
        }

        public async Task<IEnumerable<PaymentModeDto>> GetPaymentModesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(PaymentModeCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<PaymentModeDto>>(cachedEntity);
            var entities = await unitOfWork.PaymentModeRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<PaymentModeDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(PaymentModeCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<OrderStatusDto>> GetOrderStatusesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(OrderStatusCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<OrderStatusDto>>(cachedEntity);
            var entities = await unitOfWork.OrderStatusRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<OrderStatusDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(OrderStatusCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<PaymentGatewayTypeDto>> GetPaymentGatewayTypesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(PaymentGatewayTypeCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<PaymentGatewayTypeDto>>(cachedEntity);
            var entities = await unitOfWork.PaymentGatewayTypeRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<PaymentGatewayTypeDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(PaymentGatewayTypeCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<PaymentStatusDto>> GetPaymentStatusesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(PaymentStatusCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<PaymentStatusDto>>(cachedEntity);
            var entities = await unitOfWork.PaymentStatusRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<PaymentStatusDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(PaymentStatusCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public async Task<IEnumerable<CurrencyTypeDto>> GetCurrencyTypesAsync()
        {
            var cachedEntity = await cache.GetStringAsync(CurrencyTypeCacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<IEnumerable<CurrencyTypeDto>>(cachedEntity);
            var entities = await unitOfWork.CurrencyTypeRepo.GetAllAsync().ConfigureAwait(false);
            var modelDto = mapper.Map<List<CurrencyTypeDto>>(entities);

            if (modelDto.Count > 0)
                await cache.SetStringAsync(CurrencyTypeCacheKey, JsonConvert.SerializeObject(modelDto)).ConfigureAwait(false);
            return modelDto;
        }

        public IEnumerable<DropdownItemDto> GetHrefLangs()
        {
            var modelDto = RelativeHrefLangConstant.GetHrefLangs()
                        .Select(x => new DropdownItemDto
                        {
                            Key = x.Key,
                            Value = x.Value
                        });

            return modelDto;
        }
    }
}
