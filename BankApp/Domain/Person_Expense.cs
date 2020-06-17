using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Domain
{
    public class Person_Expense
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }
    }
}
