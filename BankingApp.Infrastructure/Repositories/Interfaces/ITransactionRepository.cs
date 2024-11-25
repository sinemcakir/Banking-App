using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Repositories.Interfaces
{
	public interface ITransactionRepository
	{
		Task<Transaction> RecordTransactionAsync(Transaction transaction);
		Task<IEnumerable<Transaction>> GetAccountTransactionsAsync(string accountNumber);
		Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(
			string accountNumber,
			DateTime startDate,
			DateTime endDate
		);
	}
}
