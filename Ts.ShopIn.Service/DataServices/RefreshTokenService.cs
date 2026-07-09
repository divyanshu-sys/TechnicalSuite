using AutoMapper;
using Ts.Dto.RefreshTokenDtos;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.ShopIn.Service.DataServices
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RefreshTokenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> DeleteByUserIdAsync(string userId)
        {
            var isDeleted = await RefreshTokenCommonService.DeleteByUserIdAsync(userId, unitOfWork).ConfigureAwait(false);

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
    }
}
