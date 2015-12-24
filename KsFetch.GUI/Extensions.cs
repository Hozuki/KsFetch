using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsFetch.GUI
{

    static class Extensions
    {

        public static void ForEach(this IEnumerable en, Action<object> action)
        {
            foreach (var v in en)
            {
                action(v);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> en, Action<T> action)
        {
            foreach (var v in en)
            {
                action(v);
            }
        }

    }

}
