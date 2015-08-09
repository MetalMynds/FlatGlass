using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// From an Original Idea @ http://stackoverflow.com/questions/653563/passing-command-line-arguments-in-c

namespace MetalMynds.Utilities
{
    public class EnvironmentHelper
    {
        private enum parseState : int { StartToken, InQuote, InToken };

        public static String[] CommandLineArgs { get { return parseCommandLine().ToArray(); } }

        private static List<String> parseCommandLine()
        {
            String commandLineArgs = Environment.CommandLine.ToString();

            List<String> listArgs = new List<String>();

            Regex rWhiteSpace = new Regex("[\\s]");
            StringBuilder token = new StringBuilder();

            parseState parsingState = parseState.StartToken;

            for (int i = 0; i < commandLineArgs.Length; i++)
            {
                char argsChar = commandLineArgs[i];
                //    Console.WriteLine(c.ToString()  + ", " + eps);
                //Looking for beginning of next token
                if (parsingState == parseState.StartToken)
                {
                    if (rWhiteSpace.IsMatch(argsChar.ToString()))
                    {
                        //Skip whitespace
                    }
                    else if (argsChar == '"')
                    {
                        parsingState = parseState.InQuote;
                    }
                    else
                    {
                        token.Append(argsChar);
                        parsingState = parseState.InToken;
                    }
                }

                else if (parsingState == parseState.InToken)
                {
                    if (rWhiteSpace.IsMatch(argsChar.ToString()))
                    {
                        //Console.WriteLine("Token: [" + token.ToString() + "]");
                        listArgs.Add(token.ToString().Trim());
                        parsingState = parseState.StartToken;

                        //Start new token.
                        token.Remove(0, token.Length);
                    }
                    else if (argsChar == '"')
                    {
                        // token.Append(c);
                        parsingState = parseState.InQuote;
                    }
                    else
                    {
                        token.Append(argsChar);
                        parsingState = parseState.InToken;
                    }

                }
                //When in a quote, white space is included in the token
                else if (parsingState == parseState.InQuote)
                {
                    if (argsChar == '"')
                    {
                        // token.Append(c);
                        parsingState = parseState.InToken;
                    }
                    else
                    {
                        token.Append(argsChar);
                        parsingState = parseState.InQuote;
                    }

                }


            }
            if (token.ToString() != "")
            {
                listArgs.Add(token.ToString());
            }
            return listArgs;
        }


    }
}
