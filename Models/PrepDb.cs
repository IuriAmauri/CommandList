using System.Linq;
using CommandList.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommandList.Models
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CommandContext>());
            }
        }

        public static void SeedData(CommandContext commandContext)
        {
            System.Console.WriteLine("Applyingm Migrations...");
            commandContext.Database.Migrate();
            System.Console.WriteLine("Finished applying migrations...");

            if (commandContext.Commands.Any())
            {
                System.Console.WriteLine("Already has data - not seeding.");
                return;
            }
            
            System.Console.WriteLine("Adding data - seeding.");
            
            commandContext.Commands.AddRange(
                new Command() {
                    HowTo = "Add a migration",
                    Line = "dotnet ef migrations add <migrationName>",
                    Platform = "EF Core"
                },
                new Command() {
                    HowTo = "Run a migration",
                    Line = "dotnet ef database update",
                    Platform = "EF Core"
                },
                new Command() {
                    HowTo = "Run a .NET core app",
                    Line = "dotnet run",
                    Platform = ".NET Core CLI"
                }
            );

            System.Console.WriteLine("Finished seeding.");

            commandContext.SaveChanges();
        }
    }
}