// -----------------------------------------------------------------------
// <copyright file="ExpressionLocator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    //using NCalc;
    //using NCalc.Domain;

    using System.Windows.Automation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExpressionLocator : Locator
    {

        private readonly String _description;

        private readonly Condition _condition;

        public ExpressionLocator(String Name, int Order, Scope Scope, String Expression)
            : base(Name, Order, Scope)
        {

            _description = String.Format("Expression Locator: Order: {0} Scope: [{1}] Expression: [{2}]", Order, Scope, Expression);

            PrevailLexer lexer = new PrevailLexer(new AntlrInputStream(Expression));
            
            PrevailParser parser = new PrevailParser(new CommonTokenStream(lexer));

            IParseTree tree = parser.expression();

            ParseTreeWalker walker = new ParseTreeWalker();

            PrevailListener prevailListener = new PrevailListener();

            walker.Walk(prevailListener, tree);

            _condition = prevailListener.Result;

        }

        public override Condition Condition
        {
            get { return _condition; }
        }


        public override string Description
        {
            get { return _description; }
        }


    }
}
