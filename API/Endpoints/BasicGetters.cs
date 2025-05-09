using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Data;

namespace API.Endpoints;

public static class BasicGetters
{
    public static void MapBasicGetters(this WebApplication app)
    {
        app.MapGet("/services", GetAllServices);
        app.MapGet("/customers", GetAllCustomers);
        app.MapGet("/contracts", GetAllContracts);
        app.MapGet("/monthly_balances", GetMonthlyBalances);

    }

    //TODO add try catch block to handle errors more gracefully.
    public static async Task<IResult> GetAllServices(MyContext db)
    {
        var services = await db.Services.ToListAsync();
        return Results.Ok(services);
    }

    public static async Task<IResult> GetAllCustomers(MyContext db)
    {
        var customers = await db.Customers.ToListAsync();
        return Results.Ok(customers);   
    }

    public static async Task<IResult> GetAllContracts(MyContext db)
    {
        var contracts = await db.Contracts.ToListAsync();
        return Results.Ok(contracts);
    }

    public static async Task<IResult> GetMonthlyBalances(MyContext db)
    {
        var MonthlyBalances = await db.MonthlyBalances.ToListAsync();
        return Results.Ok(MonthlyBalances);
    }
}
