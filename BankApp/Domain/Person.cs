﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Person_Expense> Persons_Expenses { get; set; }
    }
}
