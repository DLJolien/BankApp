using BankApp.Domain;
using BankApp.Services;
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
        private IPhotoService _photoService;
        private readonly List<Expense> _expenses;
       

        public ExpenseDatabase(IPhotoService photoService)
        {
            if (_expenses == null)
            {
                _expenses = new List<Expense>();
            }
            _photoService = photoService;
            LoadTestData();
            
        }

        private void LoadTestData()
        {
            Expense expense1 = new Expense()
            {
                Date = new DateTime(2020, 05, 12, 21, 01, 59),
                Category = Category.Food,
                Description = "Pizza order",
                Amount = 25
            };
            Expense expense2 = new Expense()
            {
                Date = new DateTime(2020, 05, 18, 23, 20, 58),
                Category = Category.Food,
                Description = "Shopping",
                Amount = 124.78M
            };
            Expense expense3 = new Expense()
            {
                Date = new DateTime(2020, 05, 31, 6, 07, 05),
                Category = Category.Household,
                Description = "Furniture: garden table",
                Amount = 249.99M
            };
            Expense expense4 = new Expense()
            {
                Date = new DateTime(2020, 04, 27, 10, 44, 27),
                Category = Category.Household,
                Description = "Furniture: cushions",
                Amount = 42.31M
            };
            Expense expense5 = new Expense()
            {
                Date = new DateTime(2020, 03, 31, 18, 17, 36),
                Category = Category.Food,
                Description = "Restaurant Birthday",
                Amount = 103.68M
            };
            Expense expense6 = new Expense()
            {
                Date = new DateTime(2020, 06, 07, 15, 23, 28),
                Category = Category.Car,
                Description = "Gas station",
                Amount = 65.45M
            };
            Expense expense7 = new Expense()
            {
                Date = new DateTime(2020, 06, 07, 20, 23, 28),
                Category = Category.Food,
                Description = "Restaurant Jackie",
                Amount = 167.48M
            };
            Expense expense8 = new Expense()
            {
                Date = new DateTime(2020, 06, 07, 17, 23, 28),
                Category = Category.Gifts,
                Description = "Present Father's day",
                Amount = 58.49M
            };
            Expense expense9 = new Expense()
            {
                Date = new DateTime(2020, 01, 10, 17, 23, 28),
                Category = Category.UtilityBill,
                Description = "Electricity",
                Amount = 101M
            };
            _photoService.AssignPicToExpense(expense1);
            _photoService.AssignPicToExpense(expense2);
            _photoService.AssignPicToExpense(expense3);
            _photoService.AssignPicToExpense(expense4);
            _photoService.AssignPicToExpense(expense5);
            _photoService.AssignPicToExpense(expense6);
            _photoService.AssignPicToExpense(expense7);
            _photoService.AssignPicToExpense(expense8);
            _photoService.AssignPicToExpense(expense9);
            expense9.PhotoUrl = @"\expense-pics\shopping.tif";
            expense8.PhotoUrl = @"\expense-pics\holiday.png";
            Insert(expense1);
            Insert(expense2);
            Insert(expense3);
            Insert(expense4);
            Insert(expense5);
            Insert(expense6);
            Insert(expense7);
            Insert(expense8);
            Insert(expense9);
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
                expense.Category = updatedExpense.Category;
            }
        }
    }
}
