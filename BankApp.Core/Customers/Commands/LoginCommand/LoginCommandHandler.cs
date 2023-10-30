using BankApp.Core.Models;
using BankApp.Core.Repositories;
using BankApp.Core.Services;
using MediatR;

namespace BankApp.Core.Customers.Commands.LoginCommand;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordService _passwordService;
    private readonly INotificationService _notificationService;
    public LoginCommandHandler(ICustomerRepository customerRepository, IJwtService jwtService, IPasswordService passwordService, INotificationService notificationService)
    {
        _customerRepository = customerRepository;
        _jwtService = jwtService;
        _passwordService = passwordService;
        _notificationService = notificationService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByUsernameAsync(request.LoginRequest.Username);
        string passwordHash = _passwordService.GenerateHash(request.LoginRequest.Password);
        if (customer is not null && await _customerRepository.VerifyLoginAsync(request.LoginRequest.Username, passwordHash))
        {
            await _notificationService.SendOperationNotification($"{DateTime.UtcNow} UTC: user {customer.Id} logged in");
            string token = _jwtService.GenerateJwtToken();
            return new LoginResponse(true, customer.Id, token);
        }
        return new LoginResponse(false);
    }
}