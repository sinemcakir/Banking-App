using BankingApp.Domain.Model;
using BankingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly BankingDbContext _context;

		public CustomerRepository(BankingDbContext context)
		{
			_context = context;
		}

		public async Task<Customer> AddCustomerAsync(Customer customer)
		{
			await _context.Customers.AddAsync(customer);
			await _context.SaveChangesAsync();
			return customer;
		}

		public async Task<Customer> GetCustomerByIdAsync(int customerId)
		{
			return await _context.Customers
				.Include(c => c.BankAccounts)
				.FirstOrDefaultAsync(c => c.Id == customerId);
		}

		public async Task<Customer> GetCustomerByEmailAsync(string email)
		{
			return await _context.Customers
				.FirstOrDefaultAsync(c => c.Email == email);
		}

		public async Task<bool> CustomerExistsAsync(string email)
		{
			return await _context.Customers
				.AnyAsync(c => c.Email == email);
		}

		public async Task UpdateCustomerAsync(Customer customer)
		{
			_context.Customers.Update(customer);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCustomerAsync(int customerId)
		{
			var customer = await GetCustomerByIdAsync(customerId);
			if (customer != null)
			{
				_context.Customers.Remove(customer);
				await _context.SaveChangesAsync();
			}
		}
	}

}
