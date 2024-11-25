

using BankingApp.Application.Dtos;
using BankingApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankingApp.API.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		[HttpPost]
		[SwaggerOperation(
			Summary = "Create a new customer",
			Description = "Registers a new customer in the banking system"
		)]
		[SwaggerResponse(201, "Customer created successfully")]
		[SwaggerResponse(400, "Invalid customer data")]
		public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerDto)
		{
			var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
			return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
		}

		[HttpGet("{id}")]
		[SwaggerOperation(
			Summary = "Get customer by ID",
			Description = "Retrieves detailed customer information"
		)]
		[SwaggerResponse(200, "Customer found")]
		[SwaggerResponse(404, "Customer not found")]
		public async Task<IActionResult> GetCustomerById(int id)
		{
			var customer = await _customerService.GetCustomerByIdAsync(id);
			return Ok(customer);
		}

		[HttpPut("{id}/account")]
		[SwaggerOperation(
			Summary = "Open a bank account",
			Description = "Opens a new bank account for an existing customer"
		)]
		[SwaggerResponse(200, "Bank account created")]
		[SwaggerResponse(404, "Customer not found")]
		public async Task<IActionResult> OpenBankAccount(int id, [FromBody] BankAccountCreateDto accountDto)
		{
			var bankAccount = await _customerService.OpenBankAccountAsync(id, accountDto);
			return Ok(bankAccount);
		}
	}
}
