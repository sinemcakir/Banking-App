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
	public class BankAccountRepository : IBankAccountRepository
	{
		private readonly BankingDbContext _context;

		public BankAccountRepository(BankingDbContext context)
		{
			_context = context;
		}

		public async Task<BankAccount> CreateBankAccountAsync(BankAccount account)
		{
			await _context.BankAccounts.AddAsync(account);
			await _context.SaveChangesAsync();
			return account;
		}

		public async Task<BankAccount> GetBankAccountByNumberAsync(string accountNumber)
		{
			return await _context.BankAccounts
				.Include(b => b.Customer)
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);
		}

		public async Task<IEnumerable<BankAccount>> GetCustomerAccountsAsync(int customerId)
		{
			return await _context.BankAccounts
				.Where(b => b.CustomerId == customerId)
				.ToListAsync();
		}

		public async Task UpdateBalanceAsync(string accountNumber, decimal amount)
		{
			var account = await GetBankAccountByNumberAsync(accountNumber);
			if (account != null)
			{
				account.Balance = amount;
				await _context.SaveChangesAsync();
			}
		}
	}

}
