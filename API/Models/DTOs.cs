namespace API.Models;

public class CustomerDto
{
    public string? Name { get; set; }
}

public class ServiceDto
{
    public string? Name { get; set; }
}

public class ContractDto
{
    public string? ServiceName { get; set; }

    public string? CustomerName { get; set; }

    public decimal Price { get; set; }

    public DateTime? OriginalContractStart { get; set; }

    public DateTime? CurrentTermStart { get; set; }

    public DateTime? CurrentTermEnd { get; set; }

    public int? TermLength { get; set; }

    public bool? IsAutoRenew { get; set; }

    public decimal? RenewalPriceIncrease { get; set; }

    public bool? IsChurned { get; set; }
}

public class ContractUpdateDTO
{
    //same as contract DTO but you can't update the customer or service... if you need to change htose you should be making a new contract.
    public decimal Price { get; set; }

    public DateTime? OriginalContractStart { get; set; }

    public DateTime? CurrentTermStart { get; set; }

    public DateTime? CurrentTermEnd { get; set; }

    public int? TermLength { get; set; }

    public bool? IsAutoRenew { get; set; }

    public decimal? RenewalPriceIncrease { get; set; }

    public bool? IsChurned { get; set; }

}   

public class UpdateInvoiceDateDTO
{
    public DateTime? InvoiceDate { get; set; }
}

public class InvoiceDateDto
{
    public int ContractId { get; set; }
    public DateTime InvoiceDate { get; set; }
}

public class RecognitionEventDto
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? CustomerName { get; set; }

    public int ContractId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime Date { get; set; }

    public RecognitionEventDto(int id, string serviceName, string customerName, int contractId, decimal amount, DateTime date)
    {
        Id = id;
        ServiceName = serviceName;
        CustomerName = customerName;
        ContractId = contractId;
        Amount = amount;
        Date = date;
    }
}
