namespace BankApp.Core.Dtos;
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal Balance { get; set; }

}
