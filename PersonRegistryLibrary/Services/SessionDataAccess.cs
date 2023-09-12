using PersonRegistryLibrary.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Reflection;


namespace PersonRegistryLibrary.Services
{
    public class SessionDataAccess<T> : IDataAccess<T>
        where T : IRegistryItem, new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public SessionDataAccess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public void Add(T item)
        {
            var currentKeys = _session.Keys
                .Where(x => x.Contains(typeof(T).FullName))
                .Select(x => x.Replace($"${typeof(T).FullName}_", "")
                .Cast<int>().OrderByDescending(x => x)).ToArray();

            var itemIndex = currentKeys.Count() == 0 ? 0 : currentKeys.Count();
            var itemKey = $"{typeof(T).FullName}_" + itemIndex;

            item.Id = itemIndex;
            _session.SetString(itemKey.ToString(), JsonConvert.SerializeObject(item));
        }

        public void Update(T item)
        {
            string key = $"{typeof(T).FullName}_" + item.Id;
            _session.SetString(key, JsonConvert.SerializeObject(item));
        }

        public void Remove(T item)
        {
            string key = $"{typeof(T).FullName}_" + item.Id;
            _session.Remove(key);
        }

        public ReadOnlyCollection<T> GetAll()
        {
            var objectList = new List<T>();

            var keys = _session.Keys.Where(x => x.Contains(typeof(T).FullName));
            foreach (var key in keys)
            {
                objectList.Add(JsonConvert.DeserializeObject<T>(_session.GetString(key)));
            }

            return objectList.AsReadOnly();
        }

        public T Get(int index)
        {
            
            string key = $"{typeof(T).FullName}_" + index;
            T output = JsonConvert.DeserializeObject<T>(_session.GetString(key));

            return output;
        }

        public bool CheckIfRecordExists(int index)
        {
            string key = $"{typeof(T).FullName}_" + index;
            bool output = _session.Keys.Where(x => x.Equals(key)).Any();

            return output;
        }
    }
}
