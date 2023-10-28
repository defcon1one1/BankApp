using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace BankApp.Core.Customers.Commands.LoginCommand;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly string _jwtSecret = "YourVeryMuchSecretKeyThatShouldGrantTheAlgorithmAtLeast128BitsOfEntropy"; // Replace with your secret key

    public LoginCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByUsername(request.LoginRequest.Username);
        string passwordHash = GenerateHash(request.LoginRequest.Password);
        if (customer is not null && await _customerRepository.VerifyLogin(request.LoginRequest.Username, passwordHash))
        {
            // Generate a JWT token
            string token = GenerateJwtToken(customer.Id);

            return new LoginResponse { Token = token };
        }
        return new LoginResponse { Token = null };
    }

    private string GenerateJwtToken(Guid customerId)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(1);
        var token = new JwtSecurityToken("localhost",
            "localhost",
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);

    }


    private static string GenerateHash(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Convert the byte array to a hexadecimal string
        StringBuilder builder = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }

}
