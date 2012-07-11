using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDG.DataAccessModels.Repositories
{
    interface IUserRepository
    {
        User TryGetUserByNamePasswordAndServerID(string userName, string password, int serverID);
        User TryGetCurrentUser();
        User AddNewUser(User user);
        void ResetCurrentUser();
        void SetUserToCurrentByUserID(int id);


    }
}
