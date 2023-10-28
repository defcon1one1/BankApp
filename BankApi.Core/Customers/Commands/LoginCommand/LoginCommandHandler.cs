using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BankApp.Core.Customers.Commands.LoginCommand;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly string _jwtSecret = "YourSecretKey"; // Replace with your secret key

    public LoginCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByUsername(request.LoginRequest.Username);
        if (customer is not null && await _customerRepository.VerifyLogin(request.LoginRequest))
        {
            // Generate a JWT token
            var token = GenerateJwtToken(customer.Id);

            return new LoginResponse { Token = token };
        }
        return new LoginResponse { Token = null };
    }

    private string GenerateJwtToken(Guid customerId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, customerId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(30), // Set token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
