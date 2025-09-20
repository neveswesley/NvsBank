using System.Text;

namespace NvsBank.Domain.Extras;

public class BankSlipGenerator
{
    private static Random _random = new Random();

    public static string GenerateDigitableLine(decimal amount, DateTime dueDate, Guid payeeId)
    {
        // Banco fictício (3 dígitos) + moeda (1 dígito)
        string bankCode = "001"; // Banco do Brasil (exemplo)
        string currencyCode = "9"; // Real (padrão)
        
        // Fator de vencimento (diferença de dias a partir de 07/10/1997)
        DateTime baseDate = new DateTime(1997, 10, 7);
        int days = (dueDate - baseDate).Days;
        string dueFactor = days.ToString().PadLeft(4, '0');
        
        // Valor do boleto (10 dígitos, sem vírgula)
        string amountField = ((int)(amount * 100)).ToString().PadLeft(10, '0');
        
        // Identificação do beneficiário (últimos 8 dígitos do GUID)
// Pega os bytes do GUID, soma e pega os 8 últimos dígitos
        string payeeField = Math.Abs(BitConverter.ToInt32(payeeId.ToByteArray(), 0))
            .ToString()
            .PadLeft(8, '0')
            .Substring(0, 8);
        
        // Campo livre (números aleatórios para simular)
        string freeField = _random.Next(10000000, 99999999).ToString();
        
        // Monta linha digitável fake
        var sb = new StringBuilder();
        sb.Append(bankCode);
        sb.Append(currencyCode);
        sb.Append(dueFactor);
        sb.Append(amountField);
        sb.Append(payeeField);
        sb.Append(freeField);
        
        return sb.ToString();
    }
}