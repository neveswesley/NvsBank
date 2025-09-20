using System.Security.Cryptography;
using System.Text;

public static class PixCodeGenerator
{
 
    public static string GenerateNumericCode(int length)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be > 0");

        var result = new StringBuilder(length);
        using (var rng = RandomNumberGenerator.Create())
        {
            var buffer = new byte[length];
            rng.GetBytes(buffer);
            for (int i = 0; i < length; i++)
            {
                int digit = buffer[i] % 10;
                result.Append((char)('0' + digit));
            }
        }
        return result.ToString();
    }

    public static int ComputeLuhnCheckDigit(string numberWithoutCheckDigit)
    {
        if (string.IsNullOrEmpty(numberWithoutCheckDigit)) throw new ArgumentNullException(nameof(numberWithoutCheckDigit));

        int sum = 0;
        bool doubleDigit = true; 
        for (int i = numberWithoutCheckDigit.Length - 1; i >= 0; i--)
        {
            int n = numberWithoutCheckDigit[i] - '0';
            if (doubleDigit)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }
            sum += n;
            doubleDigit = !doubleDigit;
        }
        int mod = sum % 10;
        int check = (10 - mod) % 10;
        return check;
    }

    public static string GenerateNumericCodeWithLuhn(int lengthWithoutCheckDigit)
    {
        if (lengthWithoutCheckDigit <= 0) throw new ArgumentOutOfRangeException(nameof(lengthWithoutCheckDigit));
        var baseCode = GenerateNumericCode(lengthWithoutCheckDigit);
        var check = ComputeLuhnCheckDigit(baseCode);
        return baseCode + check.ToString();
    }
}
