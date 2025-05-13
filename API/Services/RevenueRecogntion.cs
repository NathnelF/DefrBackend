using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class RevenueRecogntionHandler
{
    private readonly MyContext _db;

    public RevenueRecogntionHandler(MyContext db)
    {
        _db = db; 
    }

    public List<RecognitionEvent> GenerateRecogntionEventsByContract(ContractDataForScheduleDto contract)
    {

        if (contract.CurrentTermStart == null || contract.TermLength <= 0)
        {
            //loop can't run without CurrentStart or Term length < 0.
            throw new InvalidOperationException("Contract must have a term start. Contract must have a term length greater than 0");
        }

        var monthlyAmount = (contract.Price / contract.TermLength * -1);
        return Enumerable.Range(0, (int)contract.TermLength!)
            .Select(i => new RecognitionEvent
            {
                ContractId = contract.ContractId,
                ServiceId = contract.ServiceId,
                CustomerId = contract.CustomerId,
                Amount = monthlyAmount,
                Date = contract.CurrentTermStart.Value.AddMonths(i)

            }).ToList();
    }

    public async Task CommitEventsToDb(List<RecognitionEvent> events)
    {
        await _db.AddRangeAsync(events);
        await _db.SaveChangesAsync();
    }

    public async Task ClearSchedulesFromDb(int contractId)
    {
        await _db.RecognitionEvents.Where(e => e.ContractId == contractId).ExecuteDeleteAsync();
    }

    public async Task UpdateScheduleForInvoice(int contractId)
    {
        //get contract and check if it exists
        var contract = await _db.Contracts
            .Where(c => c.Id == contractId)
            .Select(c => new {
                c.Id,
                c.Price,
                c.TermLength,
                c.InvoiceDate
            })
            .FirstOrDefaultAsync();
        if (contract == null)
        {
            throw new InvalidOperationException($"Can't find contract with Id {contractId}");
        }
        //check if contract has an invoice date and term length.
        if (contract.InvoiceDate == null || contract.TermLength == null)
        {
            throw new InvalidOperationException($"Can't find an invoie date or term length for contract {contractId}");
        }

        if (contract.TermLength <= 0 || contract.Price <= 0) 
        {
            throw new InvalidOperationException($"Price and term length must be greater than 0");
        }
        //query events from that contract and check if they exist
        var eventsExist = await _db.RecognitionEvents
            .AnyAsync(e => e.ContractId == contractId);
        if (!eventsExist)
        {
            throw new InvalidOperationException($"Can't find any recognition event for this contract.");
        }
        //look for invoice date within the events in memory
        //this should be fine because we shouldn't ever have more than 12 - 36 events in a single schedule.
        var standardAmount = -1 * (contract.Price / contract.TermLength);
        var invoicedAmount = contract.Price + standardAmount;
        var invoiceMonth = contract.InvoiceDate.Value.Month;

        await _db.RecognitionEvents
            .Where(e => e.ContractId == contractId && e.Date.Month == invoiceMonth)
            .ExecuteUpdateAsync(s =>
            s.SetProperty(e => e.Amount, invoicedAmount));

        await _db.RecognitionEvents
            .Where(e => e.ContractId == contractId && e.Date.Month != invoiceMonth)
            .ExecuteUpdateAsync(s => 
            s.SetProperty(e => e.Amount, standardAmount));
        
        await _db.SaveChangesAsync();
        
    }

}
