using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Dtos
{
	public class WithdrawRequestDto
	{
		public decimal Amount { get; set; }
		public string AccountNumber { get; set; }
	}
}
