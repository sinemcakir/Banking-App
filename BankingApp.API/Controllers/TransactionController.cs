using BankingApp.Application.Dtos;
using BankingApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")] //Yorum
	public class TransactionController : ControllerBase
	{
		private readonly ITransactionService _transactionService;

		public TransactionController(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}

		[HttpPost("deposit")]
		public async Task<IActionResult> Deposit([FromBody] DepositRequestDto request)
		{
			var transaction = await _transactionService.DepositAsync(request.AccountNumber, request.Amount);
			return Ok(transaction);
		}

		[HttpPost("withdraw")]
		public async Task<IActionResult> Withdraw([FromBody] WithdrawRequestDto request)
		{
			var transaction = await _transactionService.WithdrawAsync(request.AccountNumber, request.Amount);
			return Ok(transaction);
		}
	}
}
