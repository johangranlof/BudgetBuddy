using DataLayer;
using Entities;

namespace BusinessLayer.Controllers
{
    public class UserController
    {
        private readonly UnitOfWork _uow;

        public UserController()
        {
            _uow = new UnitOfWork(new ApplicationDbContext());
        }

        public User? GetUser(int userid)
        {
            return _uow.UserRepository.GetAll().FirstOrDefault(u => u.UserID == userid);
        }

        public void AddUser(User user)
        {
            _uow.UserRepository.Add(user);
            _uow.Save();
        }

        public User UpdateUser(User user)
        {
            _uow.UserRepository.Update(user);
            _uow.Save();
            return user;
        }
    }
}
