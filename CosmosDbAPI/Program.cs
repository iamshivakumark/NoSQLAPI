using CosmosDbAPI.Contracts;
using CosmosDbAPI.Service;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Configuration;
using System.Security.Principal;

namespace CosmosDbAPI
{
    public class Program
    {

        private static async Task<CosmosDBService> InitializeCosmosDBClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databasename = configurationSection["databasename"];
            var containername = configurationSection["container"];
            var accounturi = configurationSection["accounturi"];
            var primarykey = configurationSection["primarykey"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(accounturi, primarykey);
            /*var database = await client.CreateDatabaseIfNotExistsAsync(databasename);
            await database.Database.CreateContainerIfNotExistsAsync(containername, "/firstname");*/
            var cosmosDbService = new CosmosDBService(client, databasename, containername);
            return cosmosDbService;
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<ICosmosDBService>(InitializeCosmosDBClientInstanceAsync(builder.Configuration.GetSection("Cosmosdb")).GetAwaiter().GetResult());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                /*app.UseSwagger();
                app.UseSwaggerUI();*/

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}