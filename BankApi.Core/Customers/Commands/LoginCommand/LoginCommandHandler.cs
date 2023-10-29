using BankApp.Core.Models;
using BankApp.Core.Repositories;
using BankApp.Core.Services;
using MediatR;

namespace BankApp.Core.Customers.Commands.LoginCommand;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;
    public LoginCommandHandler(ICustomerRepository customerRepository, JwtService jwtService, PasswordService passwordService)
    {
        _customerRepository = customerRepository;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByUsername(request.LoginRequest.Username);
        string passwordHash = _passwordService.GenerateHash(request.LoginRequest.Password);
        if (customer is not null && await _customerRepository.VerifyLogin(request.LoginRequest.Username, passwordHash))
        {
            string token = _jwtService.GenerateJwtToken();
            return new LoginResponse(true, customer.Id, token);
        }
        return new LoginResponse(false);
    }
}