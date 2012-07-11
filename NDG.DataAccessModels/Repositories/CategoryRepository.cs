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
    public class CategoryRepository : Repository, ICategoryRepository
    {


        public System.Collections.Generic.IEnumerable<Category> GetAllCategories()
        {
            return _context.Category;
        }

        public Category GetCategoryByID(int id)
        {
            return _context.Category.FirstOrDefault(c => c.ID == id);
        }

        public System.Collections.Generic.IEnumerable<Question> GetCategoryQuestionsByCategoryID(int id)
        {
            return _context.Question.Where(q => q.CategoryID == id);
        }


    }
}
