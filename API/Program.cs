using API.Data;
using API.Models;
using API.Startup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
builder.GetConnection();

var app = builder.Build();
app.UseOpenApi();
app.UseHttpsRedirection();

app.MapGet("/", () =>
    "Hello World!"
);

app.MapPost("/addCustomer", async (CustomerDto dto, MyContext db) =>
{
    var customer = new Customer
    {
        Name = dto.Name
    };
    db.Customers.Add(customer);
    await db.SaveChangesAsync();

    return Results.Created($"/customers/{customer.Id}", customer);
});

app.MapPost("/addService", async (ServiceDto dto, MyContext db) =>
{
    var service = new Service
    {
        Name = dto.Name
    };

    db.Services.Add(service);
    await db.SaveChangesAsync();

    return Results.Created($"/customers/{service.Id}", service);
});

app.MapGet("/getServices", async (MyContext db) =>
{
    var services = await db.Services.ToListAsync();
    return Results.Ok(services);

});

app.Run();

