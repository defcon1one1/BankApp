using BankApp.Core.Customers.Commands.DepositCommand;
using BankApp.Core.Customers.Commands.WithdrawCommand;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using FluentAssertions;
using Moq;

namespace BankApp.Tests;
public class HandlerTests
{
    [Fact]
    public async Task Handle_SuccessfulDeposit_ReturnsTrue()
    {
        // Arrange
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var eventRepositoryMock = new Mock<IEventRepository>();
        var notificationServiceMock = new Mock<INotificationService>();

        var id = Guid.NewGuid();
        decimal amount = 100;

        var customer = new Customer(id, "test", "test", 0);

        customerRepositoryMock.Setup(x => x.AddAsync(customer)).Returns(Task.CompletedTask);
        customerRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);
        customerRepositoryMock.Setup(x => x.AddToBalanceAsync(id, 100)).ReturnsAsync(true);
        notificationServiceMock.Setup(x => x.SendOperationNotification(It.IsAny<string>())).Returns(Task.CompletedTask);

        var depositCommand = new DepositCommand(id, amount);

        var handler = new DepositCommandHandler(customerRepositoryMock.Object, eventRepositoryMock.Object, notificationServiceMock.Object);

        // Act
        var result = await handler.Handle(depositCommand, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        customerRepositoryMock.Verify(x => x.AddToBalanceAsync(id, amount), Times.Once);
    }
    [Fact]
    public async Task Handle_UnSuccessfulDeposit_ReturnsFalse()
    {
        // Arrange
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var eventRepositoryMock = new Mock<IEventRepository>();
        var notificationServiceMock = new Mock<INotificationService>();

        Guid id = Guid.NewGuid();
        decimal amount = -1;

        var customer = new Customer(id, "test", "test", amount);

        customerRepositoryMock.Setup(x => x.AddAsync(customer)).Returns(Task.CompletedTask);
        customerRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);
        customerRepositoryMock.Setup(x => x.AddToBalanceAsync(id, amount)).ReturnsAsync(false);
        notificationServiceMock.Setup(x => x.SendOperationNotification(It.IsAny<string>())).Returns(Task.CompletedTask);

        var depositCommand = new DepositCommand(id, amount);

        var handler = new DepositCommandHandler(customerRepositoryMock.Object, eventRepositoryMock.Object, notificationServiceMock.Object);

        // Act
        var result = await handler.Handle(depositCommand, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_SuccesfulWithdrawal_ReturnsTrue()
    {
        // Arrange
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var eventRepositoryMock = new Mock<IEventRepository>();
        var notificationServiceMock = new Mock<INotificationService>();

        Guid id = Guid.NewGuid();
        decimal amount = 10;

        var customer = new Customer(id, "test", "test", amount);

        customerRepositoryMock.Setup(x => x.AddAsync(customer)).Returns(Task.CompletedTask);
        customerRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);
        customerRepositoryMock.Setup(x => x.DeductFromBalanceAsync(id, amount)).ReturnsAsync(true);
        notificationServiceMock.Setup(x => x.SendOperationNotification(It.IsAny<string>())).Returns(Task.CompletedTask);

        var withdrawCommand = new WithdrawCommand(id, amount);

        var handler = new WithdrawCommandHandler(customerRepositoryMock.Object, eventRepositoryMock.Object, notificationServiceMock.Object);

        // Act
        var result = await handler.Handle(withdrawCommand, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_NonPositiveAmountWithdrawal_ReturnsFalse()
    {
        // Arrange
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var eventRepositoryMock = new Mock<IEventRepository>();
        var notificationServiceMock = new Mock<INotificationService>();

        Guid id = Guid.NewGuid();
        decimal withdrawalAmount = -1;

        var customer = new Customer(id, "test", "test", 10);

        customerRepositoryMock.Setup(x => x.AddAsync(customer)).Returns(Task.CompletedTask);
        customerRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);
        customerRepositoryMock.Setup(x => x.DeductFromBalanceAsync(id, withdrawalAmount)).ReturnsAsync(false);
        notificationServiceMock.Setup(x => x.SendOperationNotification(It.IsAny<string>())).Returns(Task.CompletedTask);

        var withdrawCommand = new WithdrawCommand(id, withdrawalAmount);

        var handler = new WithdrawCommandHandler(customerRepositoryMock.Object, eventRepositoryMock.Object, notificationServiceMock.Object);

        // Act
        var result = await handler.Handle(withdrawCommand, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ExceedingAmountWithdrawal_ReturnsFalse()
    {
        // Arrange
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var eventRepositoryMock = new Mock<IEventRepository>();
        var notificationServiceMock = new Mock<INotificationService>();

        Guid id = Guid.NewGuid();
        decimal withdrawalAmount = 100;

        var customer = new Customer(id, "test", "test", 10);

        customerRepositoryMock.Setup(x => x.AddAsync(customer)).Returns(Task.CompletedTask);
        customerRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);
        customerRepositoryMock.Setup(x => x.DeductFromBalanceAsync(id, withdrawalAmount)).ReturnsAsync(false);
        notificationServiceMock.Setup(x => x.SendOperationNotification(It.IsAny<string>())).Returns(Task.CompletedTask);

        var withdrawCommand = new WithdrawCommand(id, withdrawalAmount);

        var handler = new WithdrawCommandHandler(customerRepositoryMock.Object, eventRepositoryMock.Object, notificationServiceMock.Object);

        // Act
        var result = await handler.Handle(withdrawCommand, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

}
