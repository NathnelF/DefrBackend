using API.Data;
using API.Models;

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
            //return empty if we don't have a current start or the term length doesn't make sense. We'll check for empty events and throw an error later.
            throw new InvalidOperationException("Contract must have a term start. Contract must have a term length greater than 0");
        }

        for (int i = 0; i < contract.TermLength; i++)
        {
            events.Add(new RecognitionEvent
            {
                ContractId = contract.Id,
                ServiceId = contract.ServiceId,
                CustomerId = contract.CustomerId,
                Amount = contract.Price / contract.TermLength,
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

}
