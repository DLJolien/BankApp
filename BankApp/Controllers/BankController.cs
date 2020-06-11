using BankApp.Database;
using BankApp.Domain;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Controllers
{
    public class BankController : Controller
    {
        private readonly IExpenseDatabase _expenseDatabase;

        public BankController(IExpenseDatabase expenseDatabase)
        {
            _expenseDatabase = expenseDatabase;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<BankListViewModel> vmList = new List<BankListViewModel>();
            IEnumerable<Expense> expenses = _expenseDatabase.GetExpenses().OrderBy(x =>x.Date);
            foreach (var expense in expenses)
            {
                BankListViewModel vm = new BankListViewModel() {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Date = expense.Date
                };
                vmList.Add(vm);

            }
            
            return View(vmList);
        }
    }
}
