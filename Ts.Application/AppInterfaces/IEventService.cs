using Ts.Application.Helpers.HelperClasses;
namespace Ts.Application.AppInterfaces
{
    public interface IEventService
    {
        void SendNotificationsAsync(NotifierEventArgs e);
    }
}
