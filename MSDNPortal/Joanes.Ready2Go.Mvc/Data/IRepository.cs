using System.Collections.Generic;

namespace Ahead.Data
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Delete(T entity);
        T Find(int id);
        IList<T> FindAll();
    }
}
