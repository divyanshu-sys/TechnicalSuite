using Ts.Common.HelperExtensions;
namespace Ts.Client.ViewModels
{
    public abstract class BaseVm
    {
        public string CreatedById { get; set; }

        public string CreatedByFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string CreatedByUserName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedOnIst => CreatedOn?.ToDateTimeIstString();

        public string UpdatedById { get; set; }

        public string UpdatedByFirstName { get; set; }

        public string UpdatedByLastName { get; set; }

        public string UpdatedByUserName { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedOnIst => UpdatedOn?.ToDateTimeIstString();
    }
}
