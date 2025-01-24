using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T Get(int id);
        List<T> GetAll();
        List<T> Find(Expression<Func<T, bool>> filter);

    }
}
