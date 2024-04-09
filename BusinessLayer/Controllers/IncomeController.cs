using DataLayer;
using Entities;
using EntityLayer;

namespace BusinessLayer.Controllers
{
    public class IncomeController
    {
        private readonly UnitOfWork _uow;

        public IncomeController()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
        }

        public List<Income> GetAllIncomesForUser(User LoggedInUser)
        {
            return _uow.IncomeRepository.GetAll().Where(i => i.UserID == LoggedInUser.UserID).ToList();
        }

        public void AddIncome(Income income)
        {
            _uow.IncomeRepository.Add(income);
            _uow.Save();
        }

        public void UpdateIncome(Income income)
        {
            _uow.IncomeRepository.Update(income);
            _uow.Save();
        }

        public void DeleteIncome(Income income)
        {
            _uow.IncomeRepository.Remove(income);
            _uow.Save();
        }
    }
}
