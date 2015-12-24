using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KsFetch
{

    static class Extensions
    {

        public static object ReadObject(this DataContractJsonSerializer serializer, string content)
        {
            return ReadObject(serializer, content, Encoding.UTF8);
        }

        public static object ReadObject(this DataContractJsonSerializer serializer, string content, Encoding encoding)
        {
            var bytes = encoding.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.ReadObject(stream);
            }
        }

        public static T ReadClass<T>(this DataContractJsonSerializer serializer, string content) where T : class
        {
            return ReadClass<T>(serializer, content, Encoding.UTF8);
        }

        public static T ReadClass<T>(this DataContractJsonSerializer serializer, string content, Encoding encoding) where T : class
        {
            var bytes = encoding.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.ReadObject(stream) as T;
            }
        }

        public static T ReadStruct<T>(this DataContractJsonSerializer serializer, string content) where T : struct
        {
            return ReadStruct<T>(serializer, content, Encoding.UTF8);
        }

        public static T ReadStruct<T>(this DataContractJsonSerializer serializer, string content, Encoding encoding) where T : struct
        {
            var bytes = encoding.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        public static string WriteObject<T>(this DataContractJsonSerializer serializer, T obj)
        {
            return WriteObject(serializer, obj, Encoding.UTF8);
        }

        public static string WriteObject<T>(this DataContractJsonSerializer serializer, T obj, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                var bytes = new byte[stream.Length];
                Array.ConstrainedCopy(stream.GetBuffer(), 0, bytes, 0, bytes.Length);
                return encoding.GetString(bytes);
            }
        }

        public static string Join(this IEnumerable<string> strings, string delim)
        {
            _stringBuilder.Clear();
            using (var en = strings.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    _stringBuilder.Append(en.Current);
                    while (en.MoveNext())
                    {
                        _stringBuilder.Append(delim);
                        _stringBuilder.Append(en.Current);
                    }
                }
            }
            return _stringBuilder.ToString();
        }

        public static string Join(this IEnumerable<string> strings, char delim)
        {
            return Join(strings, delim.ToString());
        }

        public static string Join(this IEnumerable<string> strings)
        {
            return Join(strings, ",");
        }

        public static DateTime ToDateTime(this int timestamp)
        {
            return _unixZeroTick.AddSeconds(timestamp);
        }

        public static DateTime ToDateTime(this long timestamp)
        {
            return _unixZeroTick.AddSeconds(timestamp);
        }

        public static long ToTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime - _unixZeroTick).TotalSeconds;
        }

        private static DateTime _unixZeroTick = new DateTime(1970, 1, 1);
        private static StringBuilder _stringBuilder = new StringBuilder(1024);

    }

}
