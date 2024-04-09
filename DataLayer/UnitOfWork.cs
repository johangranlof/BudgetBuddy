using Entities;
using EntityLayer;

namespace DataLayer
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public Repository<Income> IncomeRepository { get; private set; }
        public Repository<Expense> ExpenseRepository { get; private set; }
        public Repository<User> UserRepository { get; private set; }
        public Repository<Budget> BudgetRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            IncomeRepository = new Repository<Income>(_context);
            ExpenseRepository = new Repository<Expense>(_context);
            UserRepository = new Repository<User>(_context);
            BudgetRepository = new Repository<Budget>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
