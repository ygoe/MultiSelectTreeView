using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Windows.Controls
{
    public static class ListExtensions
    {
        public static object Last(this IList list)
        {
            if (list.Count < 1)
            {
                return null;
            }

            return list[list.Count - 1];
        }

        public static object First(this IList list)
        {
            if (list.Count < 1)
            {
                return null;
            }

            return list[0];
        }
    }
}
