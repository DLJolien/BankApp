using BankApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class BankCreateViewModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Category { get; set; } = new List<SelectListItem>();
        public string PhotoUrl { get; set; }
    }
}
