using BankingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Domain.Model;

namespace BankingApp.Infrastructure.Repositories
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly BankingDbContext _context;

		public TransactionRepository(BankingDbContext context)
		{
			_context = context;
		}

		public async Task<Transaction> RecordTransactionAsync(Transaction transaction)
		{
			await _context.Transactions.AddAsync(transaction);
			await _context.SaveChangesAsync();
			return transaction;
		}

		public async Task<IEnumerable<Domain.Model.Transaction>> GetAccountTransactionsAsync(string accountNumber)
		{
			var account = await _context.BankAccounts
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

			List<Domain.Model.Transaction> transactions = await _context.Transactions
							.Where(t => t.BankAccountId == account.Id)
							.OrderByDescending(t => t.TransactionDate)
							.ToListAsync();
			return transactions;
		}


		public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(
	string accountNumber,
	DateTime startDate,
	DateTime endDate)
		{
			var account = await _context.BankAccounts
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

			if (account == null)
			{
				return Enumerable.Empty<Transaction>(); // Null ise boş bir koleksiyon döner
			}

			return (IEnumerable<Transaction>)await _context.Transactions
				.Where(t => t.BankAccountId == account.Id &&
							t.TransactionDate >= startDate &&
							t.TransactionDate <= endDate)
				.OrderByDescending(t => t.TransactionDate)
				.ToListAsync();
		}


	}
}
