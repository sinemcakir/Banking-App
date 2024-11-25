using BankingApp.Application.Dtos;
using BankingApp.Application.Exceptions;
using BankingApp.Application.Interfaces;
using BankingApp.Domain.Model;
using BankingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly BankingDbContext _context;
		private readonly ILogger<CustomerService> _logger;

		public CustomerService(
			ICustomerRepository customerRepository,
			BankingDbContext context,
			ILogger<CustomerService> logger)
		{
			_customerRepository = customerRepository;
			_context = context;
			_logger = logger;
		}

		public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto)
		{
			// Validate customer data
			if (await _customerRepository.CustomerExistsAsync(customerDto.Email))
			{
				throw new InvalidOperationException("Customer with this email already exists.");
			}

			var customer = new Customer
			{
				FirstName = customerDto.FirstName,
				LastName = customerDto.LastName,
				Email = customerDto.Email,
				PhoneNumber = customerDto.PhoneNumber,
				DateOfBirth = customerDto.DateOfBirth,
				NationalIdentityNumber = customerDto.NationalIdentityNumber
			};

			var createdCustomer = await _customerRepository.AddCustomerAsync(customer);

			return new CustomerDto
			{
				Id = createdCustomer.Id,
				FirstName = createdCustomer.FirstName,
				LastName = createdCustomer.LastName,
				Email = createdCustomer.Email
			};
		}

		public async Task<BankAccountDto> OpenBankAccountAsync(int customerId, BankAccountCreateDto accountDto)
		{
			var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
			if (customer == null)
			{
				throw new NotFoundException("Customer not found");
			}

			var bankAccount = new BankAccount
			{
				CustomerId = customerId,
				AccountType = accountDto.AccountType,
				Balance = accountDto.InitialBalance,
				AccountNumber = GenerateUniqueAccountNumber(),
				OpenedDate = DateTime.UtcNow
			};

			_context.BankAccounts.Add(bankAccount);
			await _context.SaveChangesAsync();

			return new BankAccountDto
			{
				AccountNumber = bankAccount.AccountNumber,
				AccountType = bankAccount.AccountType,
				Balance = bankAccount.Balance
			};
		}

		private string GenerateUniqueAccountNumber()
		{
			// Simple account number generation logic
			return $"TR{DateTime.UtcNow.Ticks.ToString().Substring(0, 10)}";
		}

		public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
		{
			var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

			if (customer == null)
			{
				throw new NotFoundException("Customer not found");
			}

			return new CustomerDto
			{
				Id = customer.Id,
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Email = customer.Email
			};
		}

		public async Task<IEnumerable<BankAccountDto>> GetCustomerAccountsAsync(int customerId)
		{
			var accounts = await _context.BankAccounts
				.Where(b => b.CustomerId == customerId)
				.Select(b => new BankAccountDto
				{
					AccountNumber = b.AccountNumber,
					AccountType = b.AccountType,
					Balance = b.Balance
				})
				.ToListAsync();

			return accounts;
		}
	}

}
