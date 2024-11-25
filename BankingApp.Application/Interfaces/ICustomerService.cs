using BankingApp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Interfaces
{
	public interface ICustomerService
	{
		Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto);
		Task<CustomerDto> GetCustomerByIdAsync(int customerId);
		Task<BankAccountDto> OpenBankAccountAsync(int customerId, BankAccountCreateDto accountDto);
		Task<IEnumerable<BankAccountDto>> GetCustomerAccountsAsync(int customerId);
	}

}
