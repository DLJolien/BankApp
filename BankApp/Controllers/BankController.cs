using BankApp.Database;
using BankApp.Domain;
using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sun.net.www.content.image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<BankListViewModel> vmList = new List<BankListViewModel>();
            IEnumerable<Expense> expenses = await _dbContext.Expenses.Include(x =>x.Category).Include(x => x.Persons_Expenses).ThenInclude(x => x.Person).Where(expense => expense.BankAppIdentityId == userId).ToListAsync();
            IEnumerable<Expense> sortedExpenses  =  expenses.OrderBy(x =>x.Date);

            foreach (var expense in sortedExpenses)
            {
                BankListViewModel vm = new BankListViewModel() {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category.Name,
                    Date = expense.Date,
                    PhotoUrl = expense.PhotoUrl,
                    Persons = expense.Persons_Expenses.Select(pe => pe.Person.Name).ToList(),
               
            };
                vmList.Add(vm);

            }
            
            return View(vmList);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BankCreateViewModel vm = new BankCreateViewModel();
            vm.Date = DateTime.Now;

            var categories = await _dbContext.Categories.ToListAsync();
            var persons = await _dbContext.Persons.ToListAsync();

            foreach (Category category in categories)
            {
                vm.Category.Add(new SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }

            vm.Person = persons.Select(person => new SelectListItem() { Value = person.Id.ToString(), Text = person.Name }).ToList();

            return View(vm);
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BankCreateViewModel vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Expense newExpense = new Expense()
            {
                Amount = vm.Amount,
                Description = vm.Description,
                Date = vm.Date,
                PhotoUrl = vm.PhotoUrl,
                CategoryId = vm.CategoryId,
                Persons_Expenses = vm.SelectedPersons.Select(person => new Person_Expense() { PersonId = person }).ToList(),
                BankAppIdentityId = userId
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Expense expenseToEdit = await _dbContext.Expenses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            var categories = await _dbContext.Categories.ToListAsync();
            var persons = await _dbContext.Persons.ToListAsync();
            BankEditViewModel vm = new BankEditViewModel();

            foreach (Category category in categories)
            {
                vm.Category.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }

            vm.Person = persons.Select(person => new SelectListItem() { Value = person.Id.ToString(), Text = person.Name }).ToList();

            vm.Amount = expenseToEdit.Amount;
            vm.CategoryId = expenseToEdit.Category.Id;
            vm.Description = expenseToEdit.Description;
            vm.Date = expenseToEdit.Date;
            
            return View(vm);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, BankEditViewModel vm)
        {
            Expense changedExpense = await _dbContext.Expenses.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (changedExpense.BankAppIdentityId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                changedExpense.Persons_Expenses = vm.SelectedPersons.Select(person => new Person_Expense() { PersonId = person }).ToList();
                changedExpense.CategoryId = vm.CategoryId;
                changedExpense.Amount = vm.Amount;
                changedExpense.Description = vm.Description;
                changedExpense.Date = vm.Date;

                var expense = _dbContext.Expenses.Include(a => a.Persons_Expenses).SingleOrDefault(a => a.Id == id);
                _dbContext.Remove(expense);
                _dbContext.Expenses.Update(changedExpense);
                await _dbContext.SaveChangesAsync();
            }
                      
            return (RedirectToAction("Index"));
        }
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Expense expenseToDelete = _dbContext.Expenses.Find(id);
            if (expenseToDelete.BankAppIdentityId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _dbContext.Expenses.Remove(expenseToDelete);
                await _dbContext.SaveChangesAsync();
            }
            
            return (RedirectToAction("Index"));
        }
    }
}
