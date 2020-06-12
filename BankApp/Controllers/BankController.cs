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
                    Category = expense.Category,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Date = expense.Date
                };
                vmList.Add(vm);

            }
            
            return View(vmList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(BankCreateViewModel vm)
        {
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                Date = vm.Date
            };
            _expenseDatabase.Insert(newExpense);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Expense expenseToEdit = _expenseDatabase.GetExpense(id);
            BankEditViewModel vm = new BankEditViewModel()
            {
                Amount = expenseToEdit.Amount,
                Category = expenseToEdit.Category,
                Description = expenseToEdit.Description,
                Date = expenseToEdit.Date
            };

            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(BankEditViewModel vm)
        {
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                Date = vm.Date
            };
            _expenseDatabase.Update(vm.Id, newExpense);
            return (RedirectToAction("Index"));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Expense expenseToDelete = _expenseDatabase.GetExpense(id);
            BankDeleteViewModel vm = new BankDeleteViewModel()
            {
                Id= expenseToDelete.Id,
                Amount = expenseToDelete.Amount,
                Description = expenseToDelete.Description,
                Date = expenseToDelete.Date
            };

            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            _expenseDatabase.Delete(id);
            return (RedirectToAction("Index"));
        }
    }
}
