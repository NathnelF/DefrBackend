using API.Data;
using API.Endpoints;
using API.Models;
using API.Startup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
//builder.GetConnection();
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlite("Data Source=localtest.db");
});

var app = builder.Build();
app.UseOpenApi();
app.UseHttpsRedirection();

app.MapGet("/", () =>
    "Hello World!"
);

app.MapBasicGetters();
app.MapBasicPosters();
app.MapScheduleEndpoints();
app.MapBasicDeleters();
app.MapBasicUpdaters();

app.Run();

