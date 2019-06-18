using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wa_hecsa
{
    public class left_right_mid
    {
        public static string left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }

        public static string right(string param, int length)
        {
            int value = param.Length - length;
            string result = param.Substring(value, length);
            return result;
        }

        public static string mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }

        public static string mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex);
            return result;
        }
    }
}