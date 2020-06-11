using BankApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Database
{
        public interface IExpenseDatabase
        {
            Expense Insert(Expense expense);
            IEnumerable<Expense> GetExpenses();
            Expense GetExpense(int id);
            void Delete(int id);
            void Update(int id, Expense expense);
        }

        public class ExpenseDatabase : IExpenseDatabase
        {
            private int _counter;
            private readonly List<Expense> _expenses;

            public ExpenseDatabase()
            {
                if (_expenses == null)
                {
                    _expenses = new List<Expense>();
                }
                LoadTestData();
            }

        private void LoadTestData()
        {
            Expense expense1 =  new Expense() { 
            Date = new DateTime(2020,05,12, 21, 01, 59),
            Description = "Pizza order", 
            Amount = 25};
            Expense expense2 = new Expense()
            {
                Date = new DateTime(2020, 05, 18, 23, 20, 58),
                Description = "Shopping",
                Amount = 124.78M
            };
            Expense expense3 = new Expense()
            {
                Date = new DateTime(2020, 05, 31, 6, 07, 05),
                Description = "Furniture: garden table",
                Amount = 249.99M
            };
            Expense expense4 = new Expense()
            {
                Date = new DateTime(2020, 04, 27, 10, 44, 27),
                Description = "Furniture: cushions",
                Amount = 42.31M
            };
            Expense expense5 = new Expense()
            {
                Date = new DateTime(2020, 03, 31, 18, 17, 36),
                Description = "Restaurant Birthday",
                Amount = 103.68M
            };
            Expense expense6 = new Expense()
            {
                Date = new DateTime(2020, 06, 07, 15,23,28),
                Description = "Gas station",
                Amount = 65.45M
            };
            Insert(expense1);
            Insert(expense2);
            Insert(expense3);
            Insert(expense4);
            Insert(expense5);
            Insert(expense6);
        }

        public Expense GetExpense(int id)
            {
                return _expenses.FirstOrDefault(x => x.Id == id);
            }

            public IEnumerable<Expense> GetExpenses()
            {
                return _expenses;
            }

            public Expense Insert(Expense expense)
            {
                _counter++;
                expense.Id = _counter;
                _expenses.Add(expense);
                return expense;
            }

            public void Delete(int id)
            {
                var expense = _expenses.SingleOrDefault(x => x.Id == id);
                if (expense != null)
                {
                    _expenses.Remove(expense);
                }
            }

            public void Update(int id, Expense updatedExpense)
            {
                var expense = _expenses.SingleOrDefault(x => x.Id == id);
                if (expense != null)
                {
                    expense.Amount = updatedExpense.Amount;
                    expense.Date = updatedExpense.Date;
                    expense.Description = updatedExpense.Description;
                }
            }
        }
}
