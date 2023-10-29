using BankApp.Core.Customers.Commands.DepositCommand;
using BankApp.Core.Customers.Commands.LoginCommand;
using BankApp.Core.Customers.Commands.WithdrawCommand;
using BankApp.Core.Customers.Queries.GetBalanceQuery;
using BankApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Api.Controllers;
[Route("api/customer")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        LoginResponse response = await _mediator.Send(new LoginCommand(loginRequest));

        return response.Success ? Ok(response) : Unauthorized();
    }

    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalance([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        CustomerDto customerDto = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
        return customerDto is null ? NotFound() : Ok(customerDto);
    }
    [HttpPatch("{id}/deposit")]
    public async Task<IActionResult> Deposit([FromRoute] Guid id, [FromBody] decimal amount)
    {
        bool result = await _mediator.Send(new DepositCommand(id, amount), CancellationToken.None);
        return result ? NoContent() : BadRequest();
    }
    [HttpPatch("{id}/withdraw")]
    public async Task<IActionResult> Withdraw([FromRoute] Guid id, [FromBody] decimal amount)
    {
        bool result = await _mediator.Send(new WithdrawCommand(id, amount), CancellationToken.None);
        return result ? NoContent() : BadRequest();
    }

}
