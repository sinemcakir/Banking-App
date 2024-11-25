using BankingApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Domain.Model
{
	public class BankAccount
	{

		public int Id { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public string AccountNumber { get; set; }
		public AccountType AccountType { get; set; }
		public decimal Balance { get; set; }
		public DateTime OpenedDate { get; set; }
		public ICollection<Transaction> Transactions { get; set; }
	}
}
}
