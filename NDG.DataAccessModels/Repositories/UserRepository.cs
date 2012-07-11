using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace NDG.DataAccessModels.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
       
        public UserRepository()
        {
            var loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<User>(u => u.Server);
            _context.LoadOptions = loadOptions;
        }

        public User TryGetUserByNamePasswordAndServerID(string userName, string password, int serverID)
        {
            return _context.User.FirstOrDefault(u => u.Name.Equals(userName) && u.Password.Equals(password) && u.ServerID == serverID);
        }

        public User TryGetCurrentUser()
        {
            return _context.User.FirstOrDefault(u => u.IsCurrent == true);
        }


        public User AddNewUser(User user)
        {
      
            _context.User.InsertOnSubmit(user);
            _context.SubmitChanges();
            //HACK to store server inside user entity
            var fake = user.Server;
            return user;
        }


        public void ResetCurrentUser()
        {
            foreach (var user in _context.User.Where(u => u.IsCurrent))
            {
                user.IsCurrent = false;
            }
            _context.SubmitChanges();
        }

        public void SetUserToCurrentByUserID(int id)
        {
            var user = _context.User.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                user.IsCurrent = true;
                _context.SubmitChanges();
            }
        }
    }
}
