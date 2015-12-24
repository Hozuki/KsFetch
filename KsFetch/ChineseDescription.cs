using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ChineseDescriptionAttribute : Attribute
    {

        public ChineseDescriptionAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public static string ExtractFromEnum<T>(T e) where T : struct
        {
            var name = Enum.GetName(typeof(T), e);
            var mi = typeof(T).GetField(name, BindingFlags.Static | BindingFlags.Public);
            var attr = Attribute.GetCustomAttribute(mi, typeof(ChineseDescriptionAttribute)) as ChineseDescriptionAttribute;
            return attr != null ? attr.Text : null;
        }

    }

}
