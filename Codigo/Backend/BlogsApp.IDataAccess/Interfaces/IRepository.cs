using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsApp.IDataAccess.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T value);

        T Get(Func<T, bool> func);


        ICollection<T> GetAll(Func<T, bool> func);

        bool Exists(Func<T, bool> func);

    }
}
