using EntityLayer;

namespace Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public decimal Balance { get; set; }
        public decimal Savings { get; set; }
        public decimal TotalAssets => Balance + Savings;

        public ICollection<Income>? Incomes { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<Budget>? Budgets { get; set; }

        public User()
        {
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
            Budgets = new List<Budget>();
        }
    }
}
