using BankingApp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Interfaces
{
	public interface ITransactionService
	{
		Task<TransactionDto> DepositAsync(string accountNumber, decimal amount);
		Task<TransactionDto> WithdrawAsync(string accountNumber, decimal amount);
		Task<TransactionDto> TransferAsync(string fromAccount, string toAccount, decimal amount);
		Task<IEnumerable<TransactionDto>> GetAccountTransactionsAsync(string accountNumber);
	}
}
