using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Database;
using BankApp.Domain;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IExpenseDatabase _expenseDatabase;

        public StatisticsController(IExpenseDatabase expenseDatabase)
        {
            _expenseDatabase = expenseDatabase;
        }
        [HttpGet]
        public IActionResult Index()
        {
            StatisticsIndexViewModel vm = new StatisticsIndexViewModel()
            {
                Expenses = _expenseDatabase.GetExpenses(),
                HighestExpense = _expenseDatabase.GetExpenses().OrderByDescending(x => x.Amount).First(),
                LowestExpense = _expenseDatabase.GetExpenses().OrderBy(x => x.Amount).First()
        };
            return View(vm);
        }

    }
}
