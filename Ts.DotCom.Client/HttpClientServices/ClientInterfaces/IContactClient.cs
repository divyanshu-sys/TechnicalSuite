using Ts.DotCom.Client.ViewModels;
using Ts.Dto;
namespace Ts.DotCom.Client.HttpClientServices.ClientInterfaces
{
    public interface IContactClient
    {
        Task<ResponseMessageDto<bool>> ContactAsync(ContactFormVm model);
    }
}
