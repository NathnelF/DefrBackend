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

    public List<RecognitionEvent> GenerateRecogntionEventsByContract(Contract contract)
    {
        var events = new List<RecognitionEvent>();

        if (contract.CurrentTermStart == null || contract.TermLength <= 0)
        {
            //loop can't run without CurrentStart or Term length < 0.
            throw new InvalidOperationException("Contract must have a term start. Contract must have a term length greater than 0");
        }

        for (int i = 0; i < contract.TermLength; i++)
        {
            events.Add(new RecognitionEvent
            {
                ContractId = contract.Id,
                ServiceId = contract.ServiceId,
                CustomerId = contract.CustomerId,
                Amount = (contract.Price / contract.TermLength) * -1,
                Date = contract.CurrentTermStart.Value.AddMonths(i)
            });


        }
        return events;
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
        var contract = await _db.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
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
        var events = await _db.RecognitionEvents
            .Where(e => e.ContractId == contractId).ToListAsync();
        if (events == null || !events.Any())
        {
            throw new InvalidOperationException($"Can't find any recognition event for this contract.");
        }
        //look for invoice date within the events in memory
        //this should be fine because we shouldn't ever have more than 12 - 36 events in a single schedule.
        var standardAmount = -1 * (contract.Price / contract.TermLength);
        var invoicedAmount = contract.Price + standardAmount;
        foreach (var recognitionEvent in events)
        {
            if (recognitionEvent.Date.Month == contract.InvoiceDate.Value.Month)
            {
                recognitionEvent.Amount = invoicedAmount;
            }
            else
            {
                recognitionEvent.Amount = standardAmount;
            }

        }
        await _db.SaveChangesAsync();
        
    }

}
