using DataLayer;
using Entities;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Controllers
{
    public class BudgetController
    {
        private readonly UnitOfWork _uow;

        public BudgetController()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
        }

        public void AddBudget(Budget budget)
        {
            _uow.BudgetRepository.Add(budget);
            _uow.Save();
        }

        public List<Budget> GetBudgetsForUser(User user)
        {
            return _uow.BudgetRepository.GetAll().Where(budget => budget.UserID == user.UserID).ToList();
        }

        public Budget? GetBudgetById(int budgetId)
        {
            return _uow.BudgetRepository.GetById(budgetId);
        }

        public void AddToTotalExpenses(int budgetID, int amount)
        {
            var currentBudget = _uow.BudgetRepository.GetById(budgetID);

            if (currentBudget != null)
            {
                currentBudget.TotalExpenses += amount;
                currentBudget.UpdatedDate = DateTime.Now;

                _uow.BudgetRepository.Update(currentBudget);
                _uow.Save();
            }
        }

        public void DeleteBudget(Budget budget)
        {
            _uow.BudgetRepository.Remove(budget);
            _uow.Save();
        }
    }
}
