using BankApp.Database;
using BankApp.Domain;
using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Controllers
{
    public class BankController : Controller
    {
        private readonly ExpenseDbContext _dbContext;
        private readonly IPhotoService _photoService;

        public BankController(IPhotoService photoService, ExpenseDbContext dbContext)
        {
            _photoService = photoService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BankListViewModel> vmList = new List<BankListViewModel>();
            IEnumerable<Expense> expenses = await _dbContext.Expenses.ToListAsync();
            IEnumerable<Expense> sortedExpenses  =  expenses.OrderBy(x =>x.Date);
            foreach (var expense in sortedExpenses)
            {
                BankListViewModel vm = new BankListViewModel() {
                    Id = expense.Id,
                    Category = expense.Category,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Date = expense.Date,
                    PhotoUrl = expense.PhotoUrl
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
        public async Task<IActionResult> Create(BankCreateViewModel vm)
        {
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                Date = vm.Date,
                PhotoUrl = vm.PhotoUrl
            };
            if (String.IsNullOrEmpty(newExpense.PhotoUrl))
            {
                _photoService.AssignPicToExpense(newExpense);
            }
            _dbContext.Expenses.Add(newExpense);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Expense expenseToEdit = _dbContext.Expenses.Find(id);
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
        public async Task<IActionResult> Edit(BankEditViewModel vm)
        {
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Category = vm.Category,
                Description = vm.Description,
                Date = vm.Date
            };
            _dbContext.Expenses.Update(newExpense);
            await _dbContext.SaveChangesAsync();
            return (RedirectToAction("Index"));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Expense expenseToDelete = _dbContext.Expenses.Find(id);
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
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            _dbContext.Expenses.Remove(_dbContext.Expenses.Find(id));
            await _dbContext.SaveChangesAsync();
            return (RedirectToAction("Index"));
        }
    }
}
