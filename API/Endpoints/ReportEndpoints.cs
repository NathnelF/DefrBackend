using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        app.MapGet("/monthlyReport", GetMonthlyReport);
        app.MapGet("/yearlyyReport", GetYearlyReport);
    }

    public static async Task<IResult> GetMonthlyReport(MyContext db, int month, int year)
    {
        var monthlyReport = await db.RecognitionEvents
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .Select(e => new
            {
                e.ContractId,
                e.Date,
                e.Amount
            }).ToListAsync();
        if (monthlyReport == null)
        {
            return Results.NotFound($"Couldn't find any report for {month} {year}");
        }

        return Results.Ok(monthlyReport);
    }

    public static async Task<IResult> GetYearlyReport(MyContext db, int year)
    {
        var yearlyReport = await db.RecognitionEvents.Where(e => e.Date.Year == year)
            .Select(e => new
            {
                e.ContractId,
                e.Date,
                e.Amount
            }).ToListAsync();
        if (yearlyReport == null)
        {
            return Results.NotFound($"Cannot find any report for {year}");
        }

        return Results.Ok(yearlyReport);
    }
}
