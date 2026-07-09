namespace Ts.Dto
{
    public class ResponseMessageDto<T> : ResponseStatusDto
    {
        public T Data { get; set; }
    }

    public class ResponseStatusDto
    {
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
