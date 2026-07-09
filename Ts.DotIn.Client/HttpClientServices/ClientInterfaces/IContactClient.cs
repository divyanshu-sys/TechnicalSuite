using Ts.DotIn.Client.ViewModels;
using Ts.Dto;
namespace Ts.DotIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IContactClient
    {
        Task<ResponseMessageDto<bool>> ContactAsync(ContactFormVm model);
    }
}
