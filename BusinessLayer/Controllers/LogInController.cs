using DataLayer;
using Entities;

namespace BusinessLayer.Controllers
{
    public class LogInController
    {
        private readonly UnitOfWork _uow;

        public LogInController()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
        }

        public User? LogIn(string username, string password)
        {
            var user = _uow.UserRepository.GetAll().FirstOrDefault(u => u.Username == username);

            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }

    }
}
