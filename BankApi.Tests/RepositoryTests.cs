using BankApp.Core.Models;
using BankApp.Core.Repositories;
using FluentAssertions;
using Moq;

namespace BankApp.Tests;

public class RepositoryTests
{

    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    public RepositoryTests()
    {
        _customerRepositoryMock = new();
    }

    [Fact]
    public async Task CustomerExists_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _customerRepositoryMock.Setup(x => x.CustomerExistsAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _customerRepositoryMock.Object.CustomerExistsAsync(id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetCustomerById_ReturnsCustomerWithCorrectData()
    {
        // Arrange
        var expectedCustomer = new Customer(Guid.NewGuid(), "test", "test", 1);
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(expectedCustomer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(expectedCustomer);

        // Act
        var retrievedCustomer = await _customerRepositoryMock.Object.GetByIdAsync(expectedCustomer.Id, It.IsAny<CancellationToken>());

        // Assert
        retrievedCustomer.Should().NotBeNull();
        retrievedCustomer.Should().BeEquivalentTo(expectedCustomer);
    }
}