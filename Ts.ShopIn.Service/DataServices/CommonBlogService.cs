using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.Service.DataInterfaces;
using Ts.ShopIn.Domain.DataTableModels.BlogDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Dto.BlogDtos;
using Ts.ShopIn.Dto.DataTableDtos.BlogDataTableDtos;

namespace Ts.ShopIn.Service.DataServices
{
    public static class CommonBlogService
    {
        public static async Task<List<GetBlogForListViewDto>> GetForListViewAsync(IUnitOfWork unitOfWork, IMapper mapper, ISubCategoryService subCategoryService, IConfiguration config, BlogDataTableForViewRequestDto modelDto)
        {
            var dtos = new List<GetBlogForListViewDto>();

            var model = mapper.Map<BlogDataTableForViewRequest>(modelDto);

            var subCategories = await subCategoryService.GetAllAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(modelDto.SubCategoryName))
            {
                model.SubCategoryId = subCategories.FirstOrDefault(x => x.Name.Equals(modelDto.SubCategoryName, StringComparison.OrdinalIgnoreCase))?.Id;
                if (model.SubCategoryId == null)
                    return dtos;
            }

            var entities = await unitOfWork.BlogRepo.GetForListViewAsync(model).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                var dto = new GetBlogForListViewDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    SubCategoryId = entity.SubCategoryId,
                    SubCategoryName = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId).Name,
                    BlogLink = entity.BlogLink,
                    MainImageUrl = entity.MainImage == null ? null : $"{config.GetValue<string>("SrcApiShopIn:BlogImageUrl")}/{entity.MainImage}",
                    PublishedOn = entity.PublishedOn
                };
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
