using IBot.Core.Interfaces.DTOs;
using IBot.Core.Interfaces.Services.BusinessLogic;

namespace IBot.BLL.Services;

public class TransactionValidator:ITransactionValidator
{
    private string _secretKey;

    public TransactionValidator(string secretKey)
    {
        _secretKey = secretKey;
    }

    public bool Validate(TransactionDto transaction)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes); // .NET 5 +

        // Convert the byte array to hexadecimal string prior to .NET 5
        // StringBuilder sb = new System.Text.StringBuilder();
        // for (int i = 0; i < hashBytes.Length; i++)
        // {
        //     sb.Append(hashBytes[i].ToString("X2"));
        // }
        // return sb.ToString();
    }
}