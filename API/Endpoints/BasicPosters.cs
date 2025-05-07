using API.Data;
using API.Models;

namespace API.Endpoints;

public static class BasicPosters
{
    public static void MapBasicPosters(this WebApplication app)
    {
        app.MapPost("/new_customer", AddCustomer);
        app.MapPost("/new_service", AddService);

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
}
