using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace KsFetch
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    sealed class AssociatedTextAttribute : Attribute
    {

        public AssociatedTextAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public static string ExtractFromEnum<T>(T e) where T : struct
        {
            var name = Enum.GetName(typeof(T), e);
            var mi = typeof(T).GetField(name, BindingFlags.Static | BindingFlags.Public);
            var attr = Attribute.GetCustomAttribute(mi, typeof(AssociatedTextAttribute)) as AssociatedTextAttribute;
            return attr != null ? attr.Text : null;
        }

    }

}
