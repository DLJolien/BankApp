﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Database;
using BankApp.Domain;
using BankApp.Models;
using java.awt.geom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sun.net.www.content.image;

namespace BankApp.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ExpenseDbContext _expenseDatabase;
        private readonly IEnumerable<Expense> _expenses;

        public StatisticsController(ExpenseDbContext dbContext)
        {
            _expenseDatabase = dbContext;
            _expenses = _expenseDatabase.Expenses.Include(x => x.Category);
        }
        [HttpGet]
        public IActionResult Index()
        {
            StatisticsIndexViewModel vm = new StatisticsIndexViewModel()
            {
                Expenses = _expenses,
                HighestExpense = _expenses.OrderByDescending(x => x.Amount).First(),
                LowestExpense = _expenses.OrderBy(x => x.Amount).First(),
                MonthlyExpenses = _expenses.GroupBy(x => new { x.Date.Date.Month, x.Date.Year } ).Select(g => new GroupedExpenses { Date = new DateTime(g.Key.Year, g.Key.Month, 01), Amount = g.Sum(m => m.Amount) }).OrderBy(x => x.Date),
                HighestDayExpense = _expenses.GroupBy(x => x.Date.Date).Select(x => new GroupedExpenses { Date = x.Key, Amount = x.Sum(m => m.Amount) }).OrderByDescending(x => x.Amount).First(),
                MostExpensive = _expenses.GroupBy(x => x.Category).Select(g => new GroupedExpenses { Category = g.Key.Name, Amount = g.Sum(m => m.Amount) }).OrderByDescending(x => x.Amount).First(),
                LeastExpensive = _expenses.GroupBy(x => x.Category).Select(g => new GroupedExpenses { Category = g.Key.Name, Amount = g.Sum(m => m.Amount) }).OrderBy(x => x.Amount).First()
            };
            return View(vm);
        }
       
    }
}
