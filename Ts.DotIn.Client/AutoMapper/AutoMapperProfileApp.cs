using AutoMapper;
using Ts.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.ViewModels;
using Ts.DotIn.Client.ViewModels.DataTableVms;
using Ts.DotIn.Client.ViewModels.PostRelativeVms;
using Ts.DotIn.Client.ViewModels.PostVms;
using Ts.DotIn.Client.ViewModels.StoryRelativeVms;
using Ts.DotIn.Client.ViewModels.StoryVms;
using Ts.DotIn.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotIn.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotIn.Dto.PostDtos;
using Ts.DotIn.Dto.PostRelativeDtos;
using Ts.DotIn.Dto.StoryDtos;
using Ts.DotIn.Dto.StoryRelativeDtos;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
namespace Ts.DotIn.Client.AutoMapper
{
    public class AutoMapperProfileApp : Profile
    {
        public AutoMapperProfileApp()
        {
            #region DataTable
            CreateMap(typeof(SearchVm), typeof(SearchDto));

            CreateMap<PostDataTableRequestVm, PostDataTableRequestDto>();
            CreateMap<StoryDataTableRequestVm, StoryDataTableRequestDto>();
            #endregion

            #region Post
            CreateMap<UpdatePostVm, UpdatePostDto>();
            CreateMap<CreatePostVm, CreatePostDto>();
            CreateMap<PostVm, UpdatePostVm>();
            CreateMap<UpdatePostDescriptionVm, UpdatePostDescriptionDto>();
            CreateMap<PostVm, UpdatePostDescriptionVm>();
            CreateMap<PostVm, UpdatePostMainImageVm>();
            CreateMap<PostVm, UpdatePostWorkerVm>();
            CreateMap<UpdatePostWorkerVm, UpdatePostWorkerDto>();
            CreateMap<UpdatePostMainImageVm, UpdatePostMainImageDto>();
            CreateMap<UpdatePostRelativeVm, UpdatePostRelativeDto>();
            #endregion

            #region Story
            CreateMap<UpdateStoryVm, UpdateStoryDto>();
            CreateMap<CreateStoryVm, CreateStoryDto>();
            CreateMap<StoryVm, UpdateStoryVm>();
            CreateMap<UpdateStoryDescriptionVm, UpdateStoryDescriptionDto>();
            CreateMap<StoryDescriptionHelperVm, UpdateStoryDescriptionVm>();
            CreateMap<StoryVm, UpdateStoryMainImageVm>();
            CreateMap<StoryVm, UpdateStoryWorkerVm>();
            CreateMap<UpdateStoryWorkerVm, UpdateStoryWorkerDto>();
            CreateMap<UpdateStoryMainImageVm, UpdateStoryMainImageDto>();
            CreateMap<UpdateStoryRelativeVm, UpdateStoryRelativeDto>();
            #endregion

            CreateMap<ContactFormVm, ContactFormDto>();
        }
    }
}
