using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Dtos
{

	public class BankAccountDto
	{
		public string AccountNumber { get; set; }
		public AccountType AccountType { get; set; }
		public decimal Balance { get; set; }
	}
}
