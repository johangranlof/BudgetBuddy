using Entities;
using EntityLayer.Enums;

namespace EntityLayer
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        public TypeOfExpense Type { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int? BudgetID { get; set; }
        public Budget? Budget { get; set; }

        public Expense() { }
    }
}
