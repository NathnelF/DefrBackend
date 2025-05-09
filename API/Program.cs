using API.Data;
using API.Endpoints;
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

app.MapBasicGetters();
app.MapBasicPosters();
app.MapScheduleEndpoints();

app.Run();

