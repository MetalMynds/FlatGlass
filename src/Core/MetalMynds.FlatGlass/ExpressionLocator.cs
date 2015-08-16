// -----------------------------------------------------------------------
// <copyright file="ExpressionLocator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    //using NCalc;
    //using NCalc.Domain;

    using System.Windows.Automation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExpressionLocator : Locator
    {
        private readonly PrevailLexer _lexer;
        private readonly PrevailParser _parser;

        public ExpressionLocator(String Name, int Order, Scope Scope, String expression)
            : base(Name, Order, Scope)
        {
            
        }

        public override Condition Condition
        {
            get { throw new NotImplementedException(); }
        }


        public override string Description
        {
            get { throw new NotImplementedException(); }
        }
    }
}
