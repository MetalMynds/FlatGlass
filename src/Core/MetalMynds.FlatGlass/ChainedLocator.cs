// -----------------------------------------------------------------------
// <copyright file="ExpressionLocator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using MetalMynds.Utilities;

namespace MetalMynds.FlatGlass
{
    /// <summary>
    ///     This class allows x number of automation properties pairs to be either or'd or or'd together.
    /// </summary>
    public class ChainedLocator : Locator
    {
        public enum ConditionType
        {
            And,
            Or
        }

        private readonly Condition _condition;
        private readonly String _description;

        public ChainedLocator(String Name, int Order, Scope Scope, ConditionType Type,
                                 params String[] PropertyValuePairs)
            : base(Name, Order, Scope)
        {
            var descriptionBuilder = new StringBuilder();

            descriptionBuilder.AppendFormat("Expression Locator: Scope: [{0}] Condition Type: [{1}]\nProperties:\n",
                                            Scope, Type);

            var properties = new List<Condition>();

            var equalSeperators = new[] {"=", "=="};
            var notEqualSeperators = new[] { "<>", "!=" };

            foreach (String pair in PropertyValuePairs)
            {

                if (equalSeperators.Any(pair.Contains))
                {
                    String[] equalPair = pair.Split(equalSeperators, 2, StringSplitOptions.None);

                    if (equalPair.Length != 2)
                    {
                        throw new KeyValuePairInvalidFormatException(pair);
                    }

                    properties.Add(new PropertyCondition(AutomationHelper.GetProperty(equalPair[0]), equalPair[1]));

                }
                else if (notEqualSeperators.Any(pair.Contains))
                {
                    String[] notEqualPair = pair.Split(notEqualSeperators, 2, StringSplitOptions.None);

                    if (notEqualPair.Length != 2)
                    {
                        throw new KeyValuePairInvalidFormatException(pair);
                    }

                    properties.Add(
                        new NotCondition(new PropertyCondition(AutomationHelper.GetProperty(notEqualPair[0]),
                                                               notEqualPair[1])));
                }
                else
                {
                    throw new KeyValueOperatorUnrecognisedException(pair);
                }

                descriptionBuilder.AppendLine(pair);                               
                
            }

            _description = descriptionBuilder.ToString();

            if (Type == ConditionType.And)
            {
                _condition = new AndCondition(properties.ToArray());
            }
            else
            {
                _condition = new OrCondition(properties.ToArray());
            }
        }

        /// <summary>
        /// Automation Condition created from this Locator.
        /// </summary>
        public override Condition Condition
        {
            get { return _condition; }
        }

        /// <summary>
        /// Description of the locator.
        /// </summary>
        public override string Description
        {
            get { return _description; }
        }
    }

    public class KeyValuePairInvalidFormatException : Exception
    {
        public KeyValuePairInvalidFormatException(String pair)
            : base("The Key Value is Not Valid! Pair: " + pair)
        {
        }
    }

    public class KeyValueOperatorUnrecognisedException : Exception
    {
        public KeyValueOperatorUnrecognisedException(String pair)
            : base("The Key Value Operator is Not Recognised! Pair: \nAllowed:\n\tEquals: = or ==\n\tNot Equals: <> != " + pair)
        {
        }
    }
}