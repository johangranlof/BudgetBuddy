using DataLayer;
using Entities;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Controllers
{
    public class ExpenseController
    {
        private readonly UnitOfWork _uow;

        public ExpenseController()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
        }

        public List<Expense> GetAllExpensesForUser(User LoggedInUser)
        {
            return _uow.ExpenseRepository.GetAll().Where(e => e.UserID == LoggedInUser.UserID).Include(e => e.Budget).ToList();
        }

        public void AddExpense(Expense expense)
        {
            _uow.ExpenseRepository.Add(expense);
            _uow.Save();
        }

        public void UpdateExpense(Expense expense)
        {
            _uow.ExpenseRepository.Update(expense);
            _uow.Save();
        }

        public void DeleteExpense(Expense expense)
        {
            _uow.ExpenseRepository.Remove(expense);

            if (expense.Budget != null)
            {
                expense.Budget.TotalExpenses -= expense.Amount;
            }

            _uow.Save();
        }
    }
}
