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

            modelBuilder.Entity<Person>().HasData(new Person() { Id = 1, Name = "Kilian" }, new Person() { Id = 2, Name = "Kris" }, new Person() { Id = 3, Name = "Michael" }, new Person() { Id = 4, Name = "Aline" });

            modelBuilder.Entity<Person_Expense>().HasKey(pe => new { pe.PersonId, pe.ExpenseId });

            modelBuilder.Entity<Person_Expense>()
                .HasOne(pe => pe.Person)
                .WithMany(p => p.Persons_Expenses)
                .HasForeignKey(pe => pe.PersonId);

            modelBuilder.Entity<Person_Expense>()
                .HasOne(pe => pe.Expense)
                .WithMany(e => e.Persons_Expenses)
                .HasForeignKey(pe => pe.ExpenseId);


        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Person_Expense> Persons_Expenses{ get; set; }
    }
}
