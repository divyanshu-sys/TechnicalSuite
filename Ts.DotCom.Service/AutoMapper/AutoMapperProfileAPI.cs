using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.DotCom.Domain.DataTableModels;
using Ts.DotCom.Domain.DataTableModels.PostDataTables;
using Ts.DotCom.Domain.DataTableModels.StoryDataTables;
using Ts.DotCom.Domain.Models;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotCom.Dto.PostDtos;
using Ts.DotCom.Dto.PostImageDtos;
using Ts.DotCom.Dto.PostRelativeDtos;
using Ts.DotCom.Dto.PostViewDtos;
using Ts.DotCom.Dto.StoryDtos;
using Ts.DotCom.Dto.StoryImageDtos;
using Ts.DotCom.Dto.StoryRelativeDtos;
using Ts.DotCom.Dto.StoryViewDtos;
using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Service.AutoMapper
{
    public class AutoMapperProfileApi : Profile
    {
        public AutoMapperProfileApi(IConfiguration config)
        {
            #region DataTable
            CreateMap(typeof(SortOrderDto<>), typeof(SortOrder<>));
            CreateMap<SearchDto, Search>();

            CreateMap<PostOrderDto, PostOrder>();
            CreateMap<PostDataTableRequestDto, PostDataTableRequest>();
            CreateMap<PostDataTableForViewRequestDto, PostDataTableForViewRequest>();

            CreateMap<StoryOrderDto, StoryOrder>();
            CreateMap<StoryDataTableRequestDto, StoryDataTableRequest>();
            CreateMap<StoryDataTableForViewRequestDto, StoryDataTableForViewRequest>();
            #endregion

            #region Post
            CreateMap<Post, PostDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:PostImageUrl")}/{src.MainImage}"));
            CreateMap<Post, GetPostForViewDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:PostImageUrl")}/{src.MainImage}"));
            CreateMap<Post, GetPostDataTableDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            CreateMap<UpdatePostDescriptionDto, Post>();
            CreateMap<UpdatePostWorkerDto, Post>();
            CreateMap<PublishPostDto, Post>();
            CreateMap<UpdatePostMainImageDto, Post>();
            CreateMap<Post, GetUpdatePostDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:PostImageUrl")}/{src.MainImage}"));
            #endregion

            #region PostView
            CreateMap<PostView, PostViewDto>();
            #endregion

            #region PostRelative
            CreateMap<PostRelative, PostRelativeDto>();
            #endregion

            #region PostImage
            CreateMap<PostImage, PostImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiDotCom:PostImageUrl")));
            #endregion

            #region Story
            CreateMap<Story, StoryDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}"));
            CreateMap<Story, GetStoryForViewDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}"));
            CreateMap<Story, GetStoryDataTableDto>();
            CreateMap<CreateStoryDto, Story>();
            CreateMap<UpdateStoryDto, Story>();
            CreateMap<UpdateStoryDescriptionDto, Story>();
            CreateMap<UpdateStoryWorkerDto, Story>();
            CreateMap<PublishStoryDto, Story>();
            CreateMap<UpdateStoryMainImageDto, Story>();
            CreateMap<Story, GetUpdateStoryDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotCom:StoryImageUrl")}"));
            #endregion

            #region StoryView
            CreateMap<StoryView, StoryViewDto>();
            #endregion

            #region StoryRelative
            CreateMap<StoryRelative, StoryRelativeDto>();
            #endregion

            #region StoryImage
            CreateMap<StoryImage, StoryImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiDotCom:StoryImageUrl")));
            #endregion
        }
    }
}
