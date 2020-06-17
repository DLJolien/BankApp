using BankApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Database
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Household"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Car"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Food"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Gift"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Hobbies"
                },
                new Category()
                {
                    Id = 6,
                    Name = "Holiday"
                },
                new Category()
                {
                    Id = 7,
                    Name = "UtilityBill"
                }
                );
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories{ get; set; }
    }
}
