using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints;

public static class BasicPosters
{
    public static void MapBasicPosters(this WebApplication app)
    {
        app.MapPost("/new_customer", AddCustomer);
        app.MapPost("/new_service", AddService);
        app.MapPost("new_contract", AddContract);
        app.MapPost("new_monthly_balance", AddMonthlyBalance);

    }

    //TODO, add try catch blocks to handle errors more gracefully ( especially unique name violations on customer / service )

    public static async Task<IResult> AddCustomer(MyContext db, CustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name
        };
        db.Add(customer);
        await db.SaveChangesAsync();
        return Results.Created($"customer/{customer.Id}", customer);
    }

    public static async Task<IResult> AddService(MyContext db, ServiceDto dto)
    {
        var service = new Service
        {
            Name = dto.Name
        };
        db.Add(service);
        await db.SaveChangesAsync();
        return Results.Created($"services/{service.Id}", service);
    }

    public static async Task<IResult> AddContract(MyContext db, ContractDto dto)
    {
        //these are indexed so lookup should be very fast.
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.Name == dto.CustomerName);
        var service = await db.Services.FirstOrDefaultAsync(s => s.Name == dto.ServiceName);

        if (customer == null && service == null)
        {
            return Results.BadRequest("Enter valid Customer and Service");
        }
        if (customer == null)
        {
            return Results.BadRequest("Enter a valid Customer name"); //eventually prompt to add customer?
        }
        if (service == null)
        {
            return Results.BadRequest("Enter a valid Service name"); //eventually prompt to add service?
        }
        
        var contract = new Contract
        { 
            CustomerId = customer.Id,
            ServiceId = service.Id,
            Price = dto.Price,
            OriginalContractStart = dto.OriginalContractStart,
            CurrentTermStart = dto.CurrentTermStart,
            CurrentTermEnd = dto.CurrentTermEnd,
            TermLength = dto.TermLength,
            IsAutoRenew = dto.IsAutoRenew,
            RenewalPriceIncrease = dto.RenewalPriceIncrease
        };
        db.Add(contract);
        await db.SaveChangesAsync();

        var responseDto = new ContractDto
        {
            CustomerName = dto.CustomerName,
            ServiceName = dto.ServiceName,
            Price = contract.Price,
            OriginalContractStart = contract.OriginalContractStart,
            CurrentTermStart = contract.CurrentTermStart,
            CurrentTermEnd = contract.CurrentTermEnd,
            TermLength = contract.TermLength,
            IsAutoRenew = contract.IsAutoRenew,
            RenewalPriceIncrease = contract.RenewalPriceIncrease
        };
        return Results.Created($"contracts/{contract.Id}", responseDto);

    }

    public static async Task<IResult> AddMonthlyBalance(MyContext db, MonthlyBalance mb)
    {
        var monthlyBalance = new MonthlyBalance
        {
            Month = mb.Month,
            Year = mb.Year,
            Balance = mb.Balance
        };
        db.Add(monthlyBalance);
        await db.SaveChangesAsync();
        return Results.Created($"monthly_balances/{monthlyBalance.Id}", monthlyBalance);
    }
}
