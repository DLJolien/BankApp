using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                LowestExpense = _expenseDatabase.GetExpenses().OrderBy(x => x.Amount).First(),
                // x => x.Date.Date.Year
                MonthlyExpenses = _expenseDatabase.GetExpenses().GroupBy(x => x.Date.Date.Month).Select(g => new GroupedExpenses { Date = new DateTime(2020, g.Key,01), Amount = g.Sum(m => m.Amount) }).OrderBy(x =>x.Date),
                HighestDayExpense = _expenseDatabase.GetExpenses().GroupBy(x => x.Date.Date).Select(x => new GroupedExpenses{ Date = x.Key, Amount = x.Sum(m => m.Amount)}).OrderByDescending(x => x.Amount).First()
            };
            return View(vm);
        }

    }
}
