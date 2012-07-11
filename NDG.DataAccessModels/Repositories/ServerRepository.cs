using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NDG.DataAccessModels.Repositories
{
    public class ServerRepository : Repository, IServerRepository
    {


        public System.Collections.Generic.IEnumerable<Server> GetAllServers()
        {
            return _context.Server;
        }

        public Server GetServerByID(int id)
        {

            return _context.Server.FirstOrDefault(s => s.ID == id);
        }


        public Server TryGetServerByAddress(string address)
        {
            address = address.ToLower();
            return _context.Server.FirstOrDefault(s => s.Address.ToLower().Equals(address));
        }


        public Server CreateServerByAddress(string address)
        {
            var existingServer = TryGetServerByAddress(address);
            if (existingServer == null)
            {
                existingServer = new Server { Address = address.ToLower() };
                _context.Server.InsertOnSubmit(existingServer);
                _context.SubmitChanges();
            }
            return existingServer;
        }
    }
}
