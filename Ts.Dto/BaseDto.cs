namespace Ts.Dto
{
    public abstract class BaseDto
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
