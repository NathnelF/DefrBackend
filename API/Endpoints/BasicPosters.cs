using API.Data;
using API.Models;

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
        var contract = new Contract
        {
            CustomerId = dto.CustomerId,
            ServiceId = dto.ServiceId,
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
        return Results.Created($"contracts/{contract.Id}", contract);

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
