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

public class ContractDataForScheduleDto
{
    public int ContractId { get; set; }
    public int ServiceId { get; set; }
    public int CustomerId { get; set; }
    public decimal Price { get; set; }

    public int? TermLength { get; set; }
    public DateTime? CurrentTermStart { get; set; }

    public ContractDataForScheduleDto(int contractId, int serviceId, int customerId, decimal price, int? termLength, DateTime? currentTermStart)
    {
        ContractId = contractId;
        ServiceId = serviceId;
        CustomerId = customerId;
        Price = price;
        TermLength = termLength;
        CurrentTermStart = currentTermStart;
    }
}



