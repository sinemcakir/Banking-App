using BankingApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Application.Dtos
{
	public class TransactionDto
	{
		public int Id { get; set; }
		public string AccountNumber { get; set; }
		public decimal Amount { get; set; }
		public TransactionType Type { get; set; }
		public DateTime TransactionDate { get; set; }
		public string Description { get; set; }
	}
}
