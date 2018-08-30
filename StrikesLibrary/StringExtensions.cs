using System;
using System.Collections.Generic;
using System.Text;

namespace StrikesLibrary
{
    public static class StringExtensions
    {
        public static string FirstIndex(this String s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Substring(0, 1);
            }
            else
            {
                return "";
            }
        }
    }
}
