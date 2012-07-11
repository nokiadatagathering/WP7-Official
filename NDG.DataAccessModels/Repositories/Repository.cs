using System;
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
    public class Repository:IRepository
    {
        protected NdgDataContext _context;

        public Repository()
        {
            _context = new NdgDataContext();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
