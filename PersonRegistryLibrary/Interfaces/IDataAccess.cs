using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonRegistryLibrary.Interfaces
{
    public interface IDataAccess<T>
    {
        void Add(T item);
        void Remove(T item);

        void Update(T item);

        ReadOnlyCollection<T> GetAll();
        T Get(int id);

        bool CheckIfRecordExists(int index);
    }
}
