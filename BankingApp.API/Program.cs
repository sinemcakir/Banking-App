using BankingApp.Application.Interfaces;
using BankingApp.Application.Services;
using BankingApp.Infrastructure.Repositories;
using BankingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Database Configuration
		builder.Services.AddDbContext<BankingDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		// Dependency Injection
		builder.Services.AddScoped<ICustomerService, CustomerService>();
		builder.Services.AddScoped<ITransactionService, TransactionService>();
		builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

		// Add controllers
		builder.Services.AddControllers();

		// Swagger Configuration
		builder.Services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "Banking Application API",
				Version = "v1"
			});
			c.EnableAnnotations();
		});

		var app = builder.Build();

		// Configure Swagger
		app.UseSwagger();
		app.UseSwaggerUI(c =>
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API V1"));

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();

		app.Run();
	}
}