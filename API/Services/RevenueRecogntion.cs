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
        var contract = await _db.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            throw new InvalidOperationException($"Can't find contract with Id {contractId}");
        }
        if (contract.InvoiceDate != null)
        {
            var invoiceEvent = await _db.RecognitionEvents
                .Where(e => e.ContractId == contractId 
                && e.Date.Month == contract.InvoiceDate.Value.Month).FirstOrDefaultAsync();
            if (invoiceEvent != null)
            {
                //TODO, make sure all other events are just Price / TermLength!! 
                invoiceEvent.Amount += contract.Price;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Can't find event associated with invoice date {contract.InvoiceDate.Value} in contract {contractId}");
            }
        }
        else
        {
            throw new InvalidOperationException($"Can't find an invoie date for contract {contractId}");
        }
        
        
    }

}
