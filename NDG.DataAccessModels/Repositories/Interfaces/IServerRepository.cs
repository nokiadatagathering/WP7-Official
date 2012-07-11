using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDG.DataAccessModels.Repositories
{
    interface IServerRepository
    {
        IEnumerable<Server> GetAllServers();
        Server GetServerByID(int id);
        Server TryGetServerByAddress(string address);
        Server CreateServerByAddress(string address);
    }
}
