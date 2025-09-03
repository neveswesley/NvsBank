namespace NvsBank.Application.Shared.Extras;

public class BankAccountGenerator
{
    private static Random _random = new Random();

    public static string GenerateBranchNumber()
    {
        return "5555";
    }

    public static string GenerateAccountNumber()
    {
        return _random.Next(10000000, 99999999).ToString();
    }
}