using BankingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Repositories.Interfaces
{
	public interface IBankAccountRepository
	{
		Task<BankAccount> CreateBankAccountAsync(BankAccount account);
		Task<BankAccount> GetBankAccountByNumberAsync(string accountNumber);
		Task<IEnumerable<BankAccount>> GetCustomerAccountsAsync(int customerId);
		Task UpdateBalanceAsync(string accountNumber, decimal amount);
	}
}
