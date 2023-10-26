using BankApp.Core.Customers.Commands.DepositCommand;
using BankApp.Core.Customers.Commands.WithdrawCommand;
using BankApp.Core.Customers.Queries.GetBalanceQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Api.Controllers;
[Route("api/customers/{id}")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        decimal balance = await _mediator.Send(new GetBalanceQuery(id), cancellationToken);
        return Ok(balance);
    }
    [HttpPatch("deposit")]
    public async Task<IActionResult> Deposit([FromRoute] Guid id, [FromBody] decimal amount)
    {
        bool result = await _mediator.Send(new DepositCommand(id, amount), CancellationToken.None);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }
    [HttpPatch("withdraw")]
    public async Task<IActionResult> Withdraw([FromRoute] Guid id, [FromBody] decimal amount)
    {
        bool result = await _mediator.Send(new WithdrawCommand(id, amount), CancellationToken.None);
        if (result)
        {
            return NoContent();
        }
        return BadRequest();
    }
}
