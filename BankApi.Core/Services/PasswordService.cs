using System.Security.Cryptography;
using System.Text;

namespace BankApp.Core.Services;

public interface IPasswordService
{
    string GenerateHash(string password);
}
public class PasswordService : IPasswordService
{
    public string GenerateHash(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}