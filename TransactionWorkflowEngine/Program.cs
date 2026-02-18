
using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Data;
using TransactionWorkflowEngine.Handlers.TransactionsHandler;
using TransactionWorkflowEngine.Middleware;
using TransactionWorkflowEngine.Services.HistoryService;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Configure EF Core with SQL Server connection from configuration.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            // Register workflow layers (handler + services).
            builder.Services.AddScoped<ITransactionsHandler, TransactionsHandler>();
            builder.Services.AddScoped<IStatusesService, StatusesService>();
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<ITransitionsService, TransitionsService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Keep centralized error mapping early in the pipeline.
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
