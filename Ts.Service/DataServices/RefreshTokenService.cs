using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Dto.RefreshTokenDtos;
using Ts.Service.DataInterfaces;
namespace Ts.Service.DataServices
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;

        private const string EntityCacheKey = RefreshTokenCommonService.EntityCacheKey;
        private const string UserIdCacheKey = RefreshTokenCommonService.UserIdCacheKey;

        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _absoluteExpiration = TimeSpan.FromHours(1);

        public RefreshTokenService(IUnitOfWork unitOfWork, IMapper mapper,
            IDistributedCache cache)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<bool> DeleteByUserIdAsync(string userId)
        {
            var isDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(userId, unitOfWork, cache).ConfigureAwait(false);

            if (isDeleted) return true;

            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                return true;
            return false;
        }

        public async Task<RefreshTokenDto> CreateByUserIdAsync(string userId)
        {
            RefreshToken entity = RefreshTokenCommonService.CreateByUserId(userId, unitOfWork);
            var rowsChanged = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (rowsChanged > 0)
                return mapper.Map<RefreshTokenDto>(entity);

            return null;
        }

        public async Task<bool> ValidateReloginAsync(string userId, string refreshReloginId)
        {
            var modelDto = await GetCacheRefreshTokenDtoAsync(userId).ConfigureAwait(false);

            if (modelDto == null || modelDto.RefreshReloginId != refreshReloginId) return false;
            return true;
        }

        private async Task<RefreshTokenDto> GetCacheRefreshTokenDtoAsync(string userId)
        {
            var cacheKey = $"{EntityCacheKey}_{UserIdCacheKey}_{userId}";
            var cachedEntity = await cache.GetStringAsync(cacheKey).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cachedEntity))
                return JsonConvert.DeserializeObject<RefreshTokenDto>(cachedEntity);

            var entity = await unitOfWork.RefreshTokenRepo.GetByUserIdAsync(userId).ConfigureAwait(false);
            if (entity == null) return null;

            var modelDto = mapper.Map<RefreshTokenDto>(entity);

            if (modelDto != null)
            {
                var cacheEntryOptions = new DistributedCacheEntryOptions()
               .SetSlidingExpiration(_cacheExpiration)
               .SetAbsoluteExpiration(_absoluteExpiration);
                await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(modelDto), cacheEntryOptions).ConfigureAwait(false);
            }

            return modelDto;
        }
    }
}
