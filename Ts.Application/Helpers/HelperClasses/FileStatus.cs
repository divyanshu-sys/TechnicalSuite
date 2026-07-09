namespace Ts.Application.Helpers.HelperClasses
{
    public class FileStatus<T> : FileErrorMessage
    {
        public T Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }

    public class FileErrorMessage
    {
        public List<string> ErrorMessage { get; set; } = [];
    }
}
