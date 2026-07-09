using System.Text;
using Ts.Common.AppInterfaces;
using Ts.Common.HelperExtensions;
namespace Ts.Common.AppServices
{
    public class RandomService : IRandomService
    {
        public Task<int> RandomNumberAsync(int min, int max)
        {
            Random random = new();
            return Task.FromResult(random.Next(min, max));
        }

        public async Task<string> RandomPasswordAsync()
        {
            var passwordBuilder = new StringBuilder();

            // 2-Letters upper case  
            var str = await RandomStringAsync(2, false).ConfigureAwait(false);
            passwordBuilder.Append(str);

            // 2-Letters lower case
            var randString = await RandomStringAsync(2).ConfigureAwait(false);
            passwordBuilder.Append(randString);

            // 2-Digits between 10 and 99  
            var randNum = await RandomNumberAsync(10, 99).ConfigureAwait(false);
            passwordBuilder.Append(randNum);

            // 2-Special Characters
            var randAlpha = await RandomSpecialCharsAsync().ConfigureAwait(false);
            passwordBuilder.Append(randAlpha);

            Random random = new();
            string password = new string(passwordBuilder.ToString().OrderBy(s => random.Next(2) % 2 == 0).ToArray());
            return password;
        }

        public Task<string> RandomSpecialCharsAsync(int size = 2)
        {
            var builder = new StringBuilder(size);
            var validChars = "!@#$%^&*?_-";
            Random random = new();
            var chars = new char[size];
            for (var i = 0; i < size; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                builder.Append(chars[i]);
            }
            return Task.FromResult(builder.ToString());
        }

        public Task<string> RandomStringAsync(int size, bool lowerCase = true)
        {
            return Task.Run(() =>
            {
                var builder = new StringBuilder(size);
                char offset = lowerCase ? 'a' : 'A';
                const int lettersOffset = 26; // A...Z or a..z: length = 26  
                Random random = new();
                for (var i = 0; i < size; i++)
                {
                    var charData = (char)random.Next(offset, offset + lettersOffset);
                    builder.Append(charData);
                }
                return Task.FromResult(lowerCase ? builder.ToString().ToLower() : builder.ToString());
            });
        }

        public string GenerateUserName(string text, int? length, string nextNumber)
        {
            if (!string.IsNullOrEmpty(nextNumber))
            {
                if (length.HasValue)
                    return $"{text.GetFirst(length.Value)}{DateTime.Now.Year}{nextNumber:D:1}";
                else
                    return $"{text}{DateTime.Now.Year}{nextNumber:D:1}";
            }
            return null;
        }

        public string GenerateOrderNumber(string text, int? length, string nextNumber)
        {
            if (!string.IsNullOrEmpty(nextNumber))
            {
                if (length.HasValue)
                    return $"{text.GetFirst(length.Value)}{DateTime.Now.Year}{nextNumber:D:1}";
                else
                    return $"{text}{DateTime.Now.Year}{nextNumber:D:1}";
            }
            return null;
        }
    }
}
