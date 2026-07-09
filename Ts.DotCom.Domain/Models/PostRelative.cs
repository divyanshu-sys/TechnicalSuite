namespace Ts.DotCom.Domain.Models
{
    public class PostRelative : BaseEntity
    {
        public new int Id { get; set; }
        public virtual Post Post { get; set; }
        public string RelativeUrl { get; set; }

    }
}
