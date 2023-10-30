namespace BankApp.Core.Customers.Commands.LoginCommand;
public class LoginResponse
{
    public bool Success { get; set; }
    public Guid? CustomerId { get; private set; }
    public string? Token { get; private set; }

    public LoginResponse(bool isSuccess)
    {
        Success = isSuccess;
    }
    public LoginResponse(bool isSuccess, Guid? customerId, string? token)
    {
        Success = isSuccess;
        CustomerId = customerId;
        Token = token;
    }

}
