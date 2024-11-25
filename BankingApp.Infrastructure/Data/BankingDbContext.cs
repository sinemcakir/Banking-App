using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;

public class BankingDbContext : DbContext
{
	public BankingDbContext(DbContextOptions<BankingDbContext> options)
		: base(options) { }

	public DbSet<Customer> Customers { get; set; }
	public DbSet<BankAccount> BankAccounts { get; set; }
	public DbSet<Transaction> Transactions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure relationships and constraints
		modelBuilder.Entity<Customer>()
			.HasMany(c => c.BankAccounts)
			.WithOne(b => b.Customer)
			.HasForeignKey(b => b.CustomerId);

		modelBuilder.Entity<BankAccount>()
			.HasMany(b => b.Transactions)
			.WithOne(t => t.BankAccount)
			.HasForeignKey(t => t.BankAccountId);

		// Add unique constraints and indexes
		modelBuilder.Entity<Customer>()
			.HasIndex(c => c.Email)
			.IsUnique();

		modelBuilder.Entity<BankAccount>()
			.HasIndex(b => b.AccountNumber)
			.IsUnique();
	}
}
