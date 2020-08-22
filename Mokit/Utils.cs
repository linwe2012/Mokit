using System;
using System.Collections.Generic;
using System.Linq;

namespace Mokit
{
    public static class Utils
    {

        static Dictionary<char, char> StringSplitDefaultEscape = new Dictionary<char, char>{
            { '"', '"' },
            { '\'', '\'' },
            { '[' , ']' },
            { '{' , '}' },
            { '(' , ')' }
        };

        public static List<string> SplitEscaped(this string str, char delims, Dictionary<char, char> escapes = null)
        {
            return SplitEscaped(str, new string(delims, 1), escapes);
        }

        public static List<string> SplitEscaped(this string str, string delims, Dictionary<char, char> escapes = null)
        {
            var delims_table = new HashSet<char>();
            foreach(var delim in delims)
            {
                delims_table.Add(delim);
            }

            if(escapes == null)
            {
                escapes = StringSplitDefaultEscape;
            }

            var escape_stack = new List<char>();
            var results = new List<string>();

            var len = str.Length;
            for(var i = 0; i < len; )
            {
                for(var j = i; j < len; ++j)
                {
                    if(escapes.TryGetValue(str[j], out var value))
                    {
                        escape_stack.Add(value);
                    }
                    else if(escape_stack.Count != 0)
                    {
                        if(escape_stack.Last() == str[j])
                        {
                            escape_stack.RemoveAt(escape_stack.Count - 1);
                        }
                    }
                    else if ( delims_table.Contains(str[j]) )
                    {
                        results.Add(str.Substring(i, j - i));
                        i = j;
                        break;
                    }
                }
            }

            return results;
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
    }



}
