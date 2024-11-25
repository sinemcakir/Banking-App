using BankingApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Domain.Model
{
	public class Transaction
	{
		public int Id { get; set; }
		public int BankAccountId { get; set; }
		public BankAccount BankAccount { get; set; }
		public decimal Amount { get; set; }
		public TransactionType Type { get; set; }
		public DateTime TransactionDate { get; set; }
		public string Description { get; set; }
	}
}
