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
}
