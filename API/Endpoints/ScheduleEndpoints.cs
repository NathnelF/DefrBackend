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
    }
    public static async Task<IResult> GenerateScheduleFromContract(MyContext db, int Id)
    {
        var contract = await db.Contracts
            .Include(c => c.Customer)
            .Include(c => c.Service)
            .FirstOrDefaultAsync(c => c.Id == Id);
        if (contract == null)
        {
            return Results.BadRequest("Enter a valid contract Id");
        }
        var service = new RevenueRecogntionHandler(db);
        var events = service.GenerateRecogntionEventsByContract(contract);
        if (events == null)
        {
            //TODO more detailed error here.
            return Results.BadRequest("Could not generate events from contract");
        }
        await service.CommitEventsToDb(events);

        var eventDtos = events.Select(e => new RecognitionEventDto
        (
            e.Id, 
            contract.Service.Name ?? "Unknown Service", 
            contract.Customer.Name ?? "Unknown Contract", 
            e.ContractId, 
            e.Amount ?? 0, 
            e.Date
        )).ToList();
        return Results.Created($"schedules/{Id}", eventDtos);
    }
}
