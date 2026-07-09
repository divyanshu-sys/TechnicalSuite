using AutoMapper;
using Microsoft.Extensions.Configuration;
using Ts.DotIn.Domain.DataTableModels;
using Ts.DotIn.Domain.DataTableModels.PostDataTables;
using Ts.DotIn.Domain.DataTableModels.StoryDataTables;
using Ts.DotIn.Domain.Models;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotIn.Dto.PostDtos;
using Ts.DotIn.Dto.PostImageDtos;
using Ts.DotIn.Dto.PostRelativeDtos;
using Ts.DotIn.Dto.PostViewDtos;
using Ts.DotIn.Dto.StoryDtos;
using Ts.DotIn.Dto.StoryImageDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
using Ts.DotIn.Dto.StoryViewDtos;
using Ts.Dto.DataTableDtos;
namespace Ts.DotIn.Service.AutoMapper
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
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:PostImageUrl")}/{src.MainImage}"));
            CreateMap<Post, GetPostForViewDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:PostImageUrl")}/{src.MainImage}"));
            CreateMap<Post, GetPostDataTableDto>();
            CreateMap<CreatePostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            CreateMap<UpdatePostDescriptionDto, Post>();
            CreateMap<UpdatePostWorkerDto, Post>();
            CreateMap<PublishPostDto, Post>();
            CreateMap<UpdatePostMainImageDto, Post>();
            CreateMap<Post, GetUpdatePostDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:PostImageUrl")}/{src.MainImage}"));
            #endregion

            #region PostView
            CreateMap<PostView, PostViewDto>();
            #endregion

            #region PostRelative
            CreateMap<PostRelative, PostRelativeDto>();
            #endregion

            #region PostImage
            CreateMap<PostImage, PostImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiDotIn:PostImageUrl")));
            #endregion

            #region Story
            CreateMap<Story, StoryDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}"));
            CreateMap<Story, GetStoryForViewDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}"));
            CreateMap<Story, GetStoryDataTableDto>();
            CreateMap<CreateStoryDto, Story>();
            CreateMap<UpdateStoryDto, Story>();
            CreateMap<UpdateStoryDescriptionDto, Story>();
            CreateMap<UpdateStoryWorkerDto, Story>();
            CreateMap<PublishStoryDto, Story>();
            CreateMap<UpdateStoryMainImageDto, Story>();
            CreateMap<Story, GetUpdateStoryDto>()
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}/{src.MainImage}"))
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => src.MainImage == null ? null : $"{config.GetValue<string>("SrcApiDotIn:StoryImageUrl")}"));
            #endregion

            #region StoryView
            CreateMap<StoryView, StoryViewDto>();
            #endregion

            #region StoryRelative
            CreateMap<StoryRelative, StoryRelativeDto>();
            #endregion

            #region StoryImage
            CreateMap<StoryImage, StoryImageDto>()
            .ForMember(dest => dest.ImageBaseUrl, opt => opt.MapFrom(src => config.GetValue<string>("SrcApiDotIn:StoryImageUrl")));
            #endregion
        }
    }
}
