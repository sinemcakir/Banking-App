using BankingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Infrastructure.Repositories.Interfaces
{
	public interface ICustomerRepository
	{
		Task<Customer> AddCustomerAsync(Customer customer);
		Task<Customer> GetCustomerByIdAsync(int customerId);
		Task<Customer> GetCustomerByEmailAsync(string email);
		Task<bool> CustomerExistsAsync(string email);
		Task UpdateCustomerAsync(Customer customer);
		Task DeleteCustomerAsync(int customerId);
	}

}
