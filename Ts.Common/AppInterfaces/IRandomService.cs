namespace Ts.Common.AppInterfaces
{
    public interface IRandomService
    {
        Task<int> RandomNumberAsync(int min, int max);

        Task<string> RandomStringAsync(int size, bool lowerCase = true);

        Task<string> RandomSpecialCharsAsync(int size = 2);

        Task<string> RandomPasswordAsync();

        public string GenerateUserName(string text, int? length, string nextNumber);
        public string GenerateOrderNumber(string text, int? length, string nextNumber);
    }
}
