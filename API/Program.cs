using System;
using System.Reflection.Metadata.Ecma335;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
    app.MapOpenApi();
}

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

