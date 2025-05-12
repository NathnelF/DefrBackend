using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Data;
using System.Diagnostics.Contracts;

namespace API.Endpoints;

public static class BasicGetters
{
    public static void MapBasicGetters(this WebApplication app)
    {
        app.MapGet("/services", GetAllServices);
        app.MapGet("/customers", GetAllCustomers);
        app.MapGet("/contracts", GetAllContracts);
        app.MapGet("/monthly_balances", GetMonthlyBalances);
        app.MapGet("/schedules", GetScheduleFromContract);
        app.MapGet("/contractByNames", GetContractFromCustomerAndService);

    }

    //TODO add try catch block to handle errors more gracefully.
    public static async Task<IResult> GetAllServices(MyContext db)
    {
        var services = await db.Services
            .AsNoTracking()
            .Select(s => s.Name)
            .ToListAsync();
        return Results.Ok(services);
    }

    public static async Task<IResult> GetAllCustomers(MyContext db)
    {
        var customers = await db.Customers
            .AsNoTracking()
            .Select(c => c.Name)
            .ToListAsync();
        return Results.Ok(customers);   
    }

    public static async Task<IResult> GetAllContracts(MyContext db)
    {
        var contracts = await db.Contracts
            .AsNoTracking()
            .Select(c => new
            {
                c.CustomerId,
                c.ServiceId,
                c.Price,
                c.CurrentTermStart,
                c.CurrentTermEnd,
                c.TermLength,
                c.OriginalContractStart,
                c.InvoiceDate,
                c.IsAutoRenew,
                c.IsChurned
            })
            .ToListAsync();
        return Results.Ok(contracts);
    }

    public static async Task<IResult> GetMonthlyBalances(MyContext db)
    {
        var MonthlyBalances = await db.MonthlyBalances
            .AsNoTracking()
            .Select(mb => new { 
                mb.Month,
                mb.Balance
            })
            .ToListAsync();
        return Results.Ok(MonthlyBalances);
    }

    public static async Task<IResult> GetScheduleFromContract(MyContext db, int contractId)
    {
        var contract = await db.Contracts
            .Include(c => c.RecognitionEvents)
            .Include(c => c.Customer)
            .Include(c => c.Service)
            .FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return Results.BadRequest("Invalid Contract Id.");
        }
        var events = contract.RecognitionEvents;
        if (events == null)
        {
            return Results.BadRequest("Events not generated.");
        }
        var eventDtos = events.Select(e => new RecognitionEventDto
        (
            e.Id,
            contract.Service.Name ?? "Unknown Service",
            contract.Customer.Name ?? "Unknown Contract",
            e.ContractId,
            e.Amount ?? 0,
            e.Date
        )).ToList();
        return Results.Ok(eventDtos);
    }

    public static async Task<IResult> GetContractFromCustomerAndService(MyContext db, string customerName, string serviceName)
    {
        var customer = await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Name == customerName);
        if (customer == null)
        {
            return Results.BadRequest("Can't find customer with that name");
        }
        var service = await db.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Name == serviceName);
        if (service == null) 
        {
            return Results.BadRequest("Cant' find service with that name");
        }
        var contract = await db.Contracts
            .AsNoTracking()
            .Where(c => c.CustomerId == customer.Id && c.ServiceId == service.Id)
            .Select(c => new
            {
                c.CustomerId,
                c.ServiceId,
                c.Price,
                c.CurrentTermStart,
                c.CurrentTermEnd,
                c.TermLength,
                c.OriginalContractStart,
                c.InvoiceDate,
                c.IsAutoRenew,
                c.IsChurned
            }).FirstOrDefaultAsync();

        if (contract == null)
        {
            return Results.BadRequest($"Can't find contract of name {customerName} {serviceName}");
        }
            
        return Results.Ok(contract);
    }
}
