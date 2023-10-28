namespace BankApp.Core.Customers.Commands.LoginCommand;
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
