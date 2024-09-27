using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RestaurantData
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SQLKey : Attribute
    {
        public KeyType KeyType { get; set; }
        public Type Type { get; set; }

        public SQLKey(KeyType key, Type type) 
        {
            KeyType = key;
            Type = type;
        }
    }

    public enum KeyType { PK, FK }
}
