using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch
{

    static class Serializers
    {

        public static DataContractJsonSerializer GetSerializer(Type type)
        {
            if (!_serializers.ContainsKey(type))
            {
                var serializer = new DataContractJsonSerializer(type);
                _serializers.Add(type, serializer);
            }
            return _serializers[type];
        }

        private static Dictionary<Type, DataContractJsonSerializer> _serializers = new Dictionary<Type, DataContractJsonSerializer>();

    }

}
