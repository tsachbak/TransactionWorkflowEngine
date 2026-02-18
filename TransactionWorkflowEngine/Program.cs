
using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Handlers.TransactionsHandler;
using TransactionWorkflowEngine.Services.StatusesService;
using TransactionWorkflowEngine.Services.TransactionsService;
using TransactionWorkflowEngine.Services.TransitionsService;

namespace TransactionWorkflowEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Configure Entity Framework Core with SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            // Register application services
            builder.Services.AddScoped<ITransactionsHandler, TransactionsHandler>();
            builder.Services.AddScoped<IStatusesService, StatusesService>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<ITransitionsService, TransitionsService>();

            var app = builder.Build();

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
