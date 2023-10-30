using BankApp.Api.Controllers;
using BankApp.Core.Customers.Commands.DepositCommand;
using BankApp.Core.Customers.Commands.LoginCommand;
using BankApp.Core.Customers.Commands.WithdrawCommand;
using BankApp.Core.Customers.Queries.GetBalanceQuery;
using BankApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankApp.Tests;

public class ControllerTests
{
    [Fact]
    public async Task Login_ValidUser_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new LoginResponse(true, default, default));

        // Act
        IActionResult result = await controller.Login(new LoginRequest());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetBalance_ExistingCustomer_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new CustomerDto());

        // Act
        IActionResult result = await controller.GetBalance(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Deposit_Successful_ReturnsNoContent()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<DepositCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        // Act
        IActionResult result = await controller.Deposit(Guid.NewGuid(), 100);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Withdraw_Successful_ReturnsNoContent()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<WithdrawCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        // Act
        IActionResult result = await controller.Withdraw(Guid.NewGuid(), 100);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    [Fact]
    public async Task Deposit_Unsuccessful_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<DepositCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(false);

        // Act
        IActionResult result = await controller.Deposit(Guid.NewGuid(), 100);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    [Fact]
    public async Task Withdraw_Unsuccessful_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var controller = new CustomersController(mediatorMock.Object);

        mediatorMock.Setup(m => m.Send(It.IsAny<WithdrawCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(false);

        // Act
        IActionResult result = await controller.Withdraw(Guid.NewGuid(), 100);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

}
