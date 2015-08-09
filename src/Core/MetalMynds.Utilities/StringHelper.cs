using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public static class StringHelper
    {
        public static String[] ToLines(String Input)
        {
            String[] lines = Input.Split(new String[] { "\n\r" }, StringSplitOptions.None);
            List<String> cleanedLines = new List<string>();

            foreach (String line in lines)
            {
                cleanedLines.Add(line.Replace("\n", "").Replace("\r", ""));
            }

            return cleanedLines.ToArray();
        }

        public static String[] CollectionToArray(StringCollection Collection)
        {
            String[] destination = new String[Collection.Count];

            Collection.CopyTo(destination, 0);

            return destination;

        }

        public static string NormalizeLineBreaks(string input)
        {
            // Allow 10% as a rough guess of how much the string may grow.
            // If we're wrong we'll either waste space or have extra copies -
            // it will still work
            StringBuilder builder = new StringBuilder((int)(input.Length * 1.1));

            bool lastWasCR = false;

            foreach (char c in input)
            {
                if (lastWasCR)
                {
                    lastWasCR = false;
                    if (c == '\n')
                    {
                        continue; // Already written \r\n
                    }
                }
                switch (c)
                {
                    case '\r':
                        builder.Append("\r\n");
                        lastWasCR = true;
                        break;
                    case '\n':
                        builder.Append("\r\n");
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }
            return builder.ToString();
        }

        public static String RemoveEnclosingQuotes(String Target)
        {
            String striped = Target.Trim();

            if (striped.StartsWith("\""))
            {
                striped = striped.Remove(0, 1);
            }

            if (striped.EndsWith("\""))
            {
                striped = striped.Remove(striped.Length - 1, 1);
            }

            return striped;
        }

        public static String StripQuotes(this String Target)
        {
            return RemoveEnclosingQuotes(Target);
        }

        public static System.Boolean IsNumber(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }



        public static String ToSanitisedString(this String Input)
        {
            Input = Input.Trim().Replace("&", "_and_").Replace("-", " ");

            StringBuilder builder = new StringBuilder();

            Char[] input = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(Input));

            bool firstCharNumeric = Char.IsNumber(input[0]);

            foreach (char inchar in input)
            {
                if (char.IsLetterOrDigit(inchar))
                {
                    builder.Append(inchar);
                }
                else if (char.IsWhiteSpace(inchar))
                {
                    builder.Append("_");
                }
                else if (inchar.CompareTo('_') == 0)
                {
                    builder.Append("_");
                }
            }

            if (firstCharNumeric)
            {
                builder.Insert(0, "a_");
            }

            String key = builder.ToString();

            key = key.Replace("____", "_").Replace("___", "_").Replace("__", "_");

            if (key.EndsWith("_"))
            {
                key = key.Substring(0, key.Length - 1);
            }

            return key;
        }
    }
}