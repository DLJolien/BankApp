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
            IEnumerable<Expense> expenses = await _dbContext.Expenses.Include(x =>x.Category).ToListAsync();
            IEnumerable<Expense> sortedExpenses  =  expenses.OrderBy(x =>x.Date);

            foreach (var expense in sortedExpenses)
            {
               

                BankListViewModel vm = new BankListViewModel() {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category.Name,
                    Date = expense.Date,
                    PhotoUrl = expense.PhotoUrl
                };
                vmList.Add(vm);

            }
            
            return View(vmList);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BankCreateViewModel vm = new BankCreateViewModel();
            vm.Date = DateTime.Now;

            var categories = await _dbContext.Categories.ToListAsync();

            foreach (Category category in categories)
            {
                vm.Category.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }

            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(BankCreateViewModel vm)
        {
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Description = vm.Description,
                Date = vm.Date,
                PhotoUrl = vm.PhotoUrl,
                CategoryId = vm.CategoryId
            };
            newExpense.Category =  await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == newExpense.CategoryId);
            if (String.IsNullOrEmpty(newExpense.PhotoUrl))
            {
                _photoService.AssignPicToExpense(newExpense);
            }
            _dbContext.Expenses.Add(newExpense);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Expense expenseToEdit = await _dbContext.Expenses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            var categories = await _dbContext.Categories.ToListAsync();
            BankEditViewModel vm = new BankEditViewModel();

            foreach (Category category in categories)
            {
                vm.Category.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }

            vm.Amount = expenseToEdit.Amount;
            vm.CategoryId = expenseToEdit.Category.Id;
            vm.Description = expenseToEdit.Description;
            vm.Date = expenseToEdit.Date;
            
            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, BankEditViewModel vm)
        {
            Expense changedExpense = await _dbContext.Expenses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            changedExpense.CategoryId = vm.CategoryId;
            changedExpense.Amount = vm.Amount;
            changedExpense.Description = vm.Description;
            changedExpense.Date = vm.Date;
            
            _dbContext.Expenses.Update(changedExpense);
            await _dbContext.SaveChangesAsync();
            return (RedirectToAction("Index"));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Expense expenseToDelete = await _dbContext.Expenses.FindAsync(id);
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
