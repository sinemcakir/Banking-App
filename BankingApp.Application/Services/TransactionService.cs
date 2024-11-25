using BankingApp.Application.Dtos;
using BankingApp.Application.Exceptions;
using BankingApp.Application.Interfaces;
using BankingApp.Domain.Enums;
using BankingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly BankingDbContext _context;
		private readonly ILogger<TransactionService> _logger;

		public TransactionService(
			BankingDbContext context,
			ILogger<TransactionService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<TransactionDto> DepositAsync(string accountNumber, decimal amount)
		{
			var account = await _context.BankAccounts
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

			if (account == null)
			{
				throw new NotFoundException("Account not found");
			}

			account.Balance += amount;

			var transaction = new Transaction
			{
				BankAccountId = account.Id,
				Amount = amount,
				Type = TransactionType.Deposit,
				TransactionDate = DateTime.UtcNow,
				Description = "Cash Deposit"
			};

			_context.Transactions.Add(transaction);
			await _context.SaveChangesAsync();

			return new TransactionDto
			{
				Id = transaction.Id,
				AccountNumber = accountNumber,
				Amount = amount,
				Type = TransactionType.Deposit,
				TransactionDate = transaction.TransactionDate
			};
		}

		public async Task<TransactionDto> WithdrawAsync(string accountNumber, decimal amount)
		{
			var account = await _context.BankAccounts
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

			if (account == null)
			{
				throw new NotFoundException("Account not found");
			}

			if (account.Balance < amount)
			{
				throw new InvalidOperationException("Insufficient funds");
			}

			account.Balance -= amount;

			var transaction = new Transaction
			{
				BankAccountId = account.Id,
				Amount = amount,
				Type = TransactionType.Withdrawal,
				TransactionDate = DateTime.UtcNow,
				Description = "Cash Withdrawal"
			};

			_context.Transactions.Add(transaction);
			await _context.SaveChangesAsync();

			return new TransactionDto
			{
				Id = transaction.Id,
				AccountNumber = accountNumber,
				Amount = amount,
				Type = TransactionType.Withdrawal,
				TransactionDate = transaction.TransactionDate
			};
		}

		public async Task<TransactionDto> TransferAsync(string fromAccount, string toAccount, decimal amount)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var sourceAccount = await _context.BankAccounts
					.FirstOrDefaultAsync(b => b.AccountNumber == fromAccount);

				var destinationAccount = await _context.BankAccounts
					.FirstOrDefaultAsync(b => b.AccountNumber == toAccount);

				if (sourceAccount == null || destinationAccount == null)
				{
					throw new NotFoundException("One or both accounts not found");
				}

				if (sourceAccount.Balance < amount)
				{
					throw new InvalidOperationException("Insufficient funds");
				}

				sourceAccount.Balance -= amount;
				destinationAccount.Balance += amount;

				var transferTransaction = new Transaction
				{
					BankAccountId = sourceAccount.Id,
					Amount = amount,
					Type = TransactionType.Transfer,
					TransactionDate = DateTime.UtcNow,
					Description = $"Transfer to {toAccount}"
				};

				_context.Transactions.Add(transferTransaction);
				await _context.SaveChangesAsync();
				await transaction.CommitAsync();

				return new TransactionDto
				{
					Id = transferTransaction.Id,
					AccountNumber = fromAccount,
					Amount = amount,
					Type = TransactionType.Transfer,
					TransactionDate = transferTransaction.TransactionDate
				};
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<IEnumerable<TransactionDto>> GetAccountTransactionsAsync(string accountNumber)
		{
			var account = await _context.BankAccounts
				.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);

			if (account == null)
			{
				throw new NotFoundException("Account not found");
			}

			return await _context.Transactions
				.Where(t => t.BankAccountId == account.Id)
				.OrderByDescending(t => t.TransactionDate)
				.Select(t => new TransactionDto
				{
					Id = t.Id,
					AccountNumber = accountNumber,
					Amount = t.Amount,
					Type = t.Type,
					TransactionDate = t.TransactionDate,
					Description = t.Description
				})
				.ToListAsync();
		}
	}
}
