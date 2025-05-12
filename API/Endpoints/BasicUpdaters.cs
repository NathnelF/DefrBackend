using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public static class BasicUpdaters
{
    public static void MapBasicUpdaters(this WebApplication app)
    {
        app.MapPut("update_service", UpdateServiceName);
        app.MapPut("update_customer", UpdateCustomerName);
        app.MapPut("update_contract", UpdateContractInfo);
        app.MapPut("update_invoice_date", UpdateInvoiceDate);

    }

    public static async Task<IResult> UpdateServiceName(MyContext db, int serviceId, string newName)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == serviceId);
        if (service == null)
        {
            return Results.NotFound($"Service with Id {serviceId} could not be not found.");
        }
        else
        {
            service.Name = newName;
            await db.SaveChangesAsync();
            return Results.Ok($"Service {serviceId} has new Name: {newName}");
        }

    }

    public static async Task<IResult> UpdateCustomerName(MyContext db, int customerId, string newName)
    {
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        if (customer == null)
        {
            return Results.NotFound($"Customer with Id {customerId} could not be found.");
        }
        else
        {
            customer.Name = newName;
            await db.SaveChangesAsync();
            return Results.Ok($"Customer {customerId} has new Name: {newName}");
        }
    }

    public static async Task<IResult> UpdateContractInfo(MyContext db, int contractId, ContractUpdateDTO dto)
    {
        var contract = await db.Contracts
            .FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return Results.NotFound($"Contract with Id{contractId} could not be found.");
        }
        else
        {
            contract.Price = dto.Price;
            contract.OriginalContractStart = dto.OriginalContractStart;
            contract.CurrentTermStart = dto.CurrentTermStart;
            contract.CurrentTermEnd = dto.CurrentTermEnd;
            contract.TermLength = dto.TermLength;
            contract.IsAutoRenew = dto.IsAutoRenew;
            contract.RenewalPriceIncrease = dto.RenewalPriceIncrease;
            contract.IsChurned = dto.IsChurned;
            await db.SaveChangesAsync();
            return Results.Created($"Contract {contractId} modified to be the following: ", dto);

        }
    }

    public static async Task<IResult> UpdateInvoiceDate(MyContext db, int contractId, UpdateInvoiceDateDTO dto)
    {
        var contract = await db.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return Results.NotFound($"Contract with Id {contractId} could not be found.");
        }
        else
        {
            contract.InvoiceDate = dto.InvoiceDate;
            await db.SaveChangesAsync();
            return Results.Ok($"Contract {contractId} now has an Invoice Date of {contract.InvoiceDate}");
        }
    }
}
