
namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile("Migrationconfig.json", optional: false, reloadOnChange: true);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddScoped<OracleDataMigrationService>();

            var app = builder.Build();
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                using var scope = app.Services.CreateScope();
                var migrationService = scope.ServiceProvider.GetRequiredService<OracleDataMigrationService>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                var tableMigrations = config.GetSection("TableMigrations").Get<List<TableMigrationConfig>>();

                if (tableMigrations is null || tableMigrations.Count == 0)
                {
                    Console.WriteLine("❌ No migration configurations found in appsettings.json");
                    return;
                }

                Console.WriteLine("🚀 Starting Oracle DB migration...\n");

                foreach (var tableMigration in tableMigrations)
                {
                    await migrationService.MigrateTableAsync(
                        tableMigration.SourceTableName,
                        tableMigration.TargetTableName,
                        tableMigration.ColumnMappings
                    );
                }

                Console.WriteLine("🎉 All tables migrated successfully!");
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
