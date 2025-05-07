using System.Diagnostics;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Startup;

public static class ConnectToDb
{
    public static void GetConnection(this WebApplicationBuilder builder)
    {
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
        if (connection != null)
        {
            builder.Services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(connection));

        }
        else
        {
            Debug.WriteLine("Connection String cannot be found");
        }

    }


}
