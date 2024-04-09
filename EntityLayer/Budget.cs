using Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Budget
    {
        public int BudgetID { get; set; }
        public string Name { get; set; }
        public decimal TotalExpenses { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive => DateTime.Now <= EndDate;

        public int UserID { get; set; }
        public User? User { get; set; }

        public ICollection<Expense> Expenses { get; set; }

        public Budget()
        {
            Expenses = new List<Expense>();
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
