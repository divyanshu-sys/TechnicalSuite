using AutoMapper;
using Ts.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.ViewModels;
using Ts.DotCom.Client.ViewModels.DataTableVms;
using Ts.DotCom.Client.ViewModels.PostRelativeVms;
using Ts.DotCom.Client.ViewModels.PostVms;
using Ts.DotCom.Client.ViewModels.StoryRelativeVms;
using Ts.DotCom.Client.ViewModels.StoryVms;
using Ts.DotCom.Dto.DataTableDtos.PostDataTableDtos;
using Ts.DotCom.Dto.DataTableDtos.StoryDataTableDtos;
using Ts.DotCom.Dto.PostDtos;
using Ts.DotCom.Dto.PostRelativeDtos;
using Ts.DotCom.Dto.StoryDtos;
using Ts.DotCom.Dto.StoryRelativeDtos;
using Ts.Dto;
using Ts.Dto.DataTableDtos;
namespace Ts.DotCom.Client.AutoMapper
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
