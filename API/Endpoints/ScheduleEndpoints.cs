using System.Linq;
using API.Data;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;
public static class ScheduleEndpoints
{
    public static void MapScheduleEndpoints(this WebApplication app)
    {
        app.MapPost("/gen_schedule", GenerateScheduleFromContract);
        app.MapPut("/update_schedule_invoice_amount", AlterInvoiceDate);

    }
    public static async Task<IResult> GenerateScheduleFromContract(MyContext db, int contractId)
    {
        bool eventsExist = await db.RecognitionEvents.AnyAsync(r => r.ContractId == contractId);
        if (eventsExist)
        {
            return Results.Ok("Recognition Schedule already generated for this contract!");
        }
        var contract = await db.Contracts
            .Where(c => c.Id == contractId)
            .Select(c => new ContractDataForScheduleDto(
                c.Id,
                c.ServiceId,
                c.CustomerId,
                c.Price,
                c.TermLength,
                c.CurrentTermStart
                )).FirstOrDefaultAsync(); 


        if (contract == null)
        {
            return Results.BadRequest("Enter a valid contract Id");
        }
        var service = new RevenueRecogntionHandler(db);

        await service.GenerateRecogntionEventsByContract(contract);
        

        return Results.Ok($"Schedules generated for {contractId}");
    }

    public static async Task<IResult> AlterInvoiceDate(MyContext db, int Id)
    {
        var service = new RevenueRecogntionHandler(db);
        await service.UpdateScheduleForInvoice(Id);
        return Results.Ok($"Updated recognized amount for invoice date in contrcact {Id}");
    }
}
