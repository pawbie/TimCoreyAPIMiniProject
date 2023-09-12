using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PersonRegistryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PersonRegistryLibrary.Services
{
    public class DictionaryDataAccess<T> : IDataAccess<T>
        where T: IRegistryItem, new()
    {
        private readonly Dictionary<int, T> _data;

        public DictionaryDataAccess()
        {
            _data = new();
        }

        public void Add(T item)
        {
            var itemAdded = false;
            while (itemAdded == false)
            {
                var newItemIndex = _data.Count == 0 ? 0 : _data.Keys.Max(x => x) + 1;

                itemAdded = _data.TryAdd(newItemIndex, item);
                item.Id = newItemIndex;
            }
        }

        public void Update(T item)
        {
            _data[item.Id] = item;
        }

        public void Remove(T item)
        {
            _data.Remove(item.Id);
        }

        public ReadOnlyCollection<T> GetAll()
        {
            ReadOnlyCollection<T> output;

            output = _data.Values.ToList().AsReadOnly();
            return output;
        }

        public T Get(int index)
        {
            T output;

            output = _data[index];
            return output;
            
        }

        public bool CheckIfRecordExists(int index)
        {
            bool output;

            output = _data.ContainsKey(index);
            return output;
        }
    }
}
