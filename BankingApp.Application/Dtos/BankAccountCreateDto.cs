using BankingApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Dtos
{
	public class BankAccountCreateDto
	{
		public AccountType AccountType { get; set; }
		public decimal InitialBalance { get; set; }
	}
}
