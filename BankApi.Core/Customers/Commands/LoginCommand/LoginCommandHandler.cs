using BankApp.Core.Models;
using BankApp.Core.Notifications;
using BankApp.Core.Repositories;
using BankApp.Core.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BankApp.Core.Customers.Commands.LoginCommand;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<LoginResponse>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly JwtService _jwtService;
    private readonly IHubContext<NotificationHub> _hubContext;
    public LoginCommandHandler(ICustomerRepository customerRepository, JwtService jwtService, IHubContext<NotificationHub> hubContext)
    {
        _customerRepository = customerRepository;
        _jwtService = jwtService;
        _hubContext = hubContext;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByUsername(request.LoginRequest.Username);
        string passwordHash = PasswordService.GenerateHash(request.LoginRequest.Password);
        if (customer is not null && await _customerRepository.VerifyLogin(request.LoginRequest.Username, passwordHash))
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"{DateTime.UtcNow} UTC: user {customer.Id} has logged in", cancellationToken: cancellationToken);
            string token = _jwtService.GenerateJwtToken();
            return new LoginResponse(true, customer.Id, token);
        }
        return new LoginResponse(false);
    }
}