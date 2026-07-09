using Ts.Dto;
using Ts.ShopIn.Client.ViewModels;
namespace Ts.ShopIn.Client.HttpClientServices.ClientInterfaces
{
    public interface IContactClient
    {
        Task<ResponseMessageDto<bool>> ContactAsync(ContactFormVm model);
    }
}
