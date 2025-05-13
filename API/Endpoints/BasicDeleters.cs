using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public static class BasicDeleters
{
    public static void MapBasicDeleters(this WebApplication app)
    {
        app.MapDelete("/clear_schedule", ClearScheduleById);
        app.MapDelete("/delete_contract", DeleteContractById);
    }

    public static async Task<IResult> ClearScheduleById(MyContext db, int contractId)
    {
        await db.RecognitionEvents.Where(e => e.ContractId == contractId).ExecuteDeleteAsync();
        return Results.Ok($"Cleared schedule for contract: {contractId}");
    }
    public static async Task<IResult> DeleteContractById(MyContext db, int contractId)
    {
        await db.Contracts.Where(c => c.Id == contractId).ExecuteDeleteAsync();
        return Results.Ok($"Cleared contract: {contractId}");
    }
}
