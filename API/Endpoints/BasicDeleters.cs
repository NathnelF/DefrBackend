using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public static class BasicDeleters
{
    public static void MapBasicDeleters(this WebApplication app)
    {
        app.MapDelete("/clear_schedules", ClearScheduleById);
    }

    public static async Task<IResult> ClearScheduleById(MyContext db, int contractId)
    {
        await db.RecognitionEvents.Where(e => e.ContractId == contractId).ExecuteDeleteAsync();
        return Results.Ok($"Cleared schedule for contract: {contractId}");


    }
}
