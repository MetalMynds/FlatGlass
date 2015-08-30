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
        private readonly Condition _condition;

        public ExpressionLocator(String Name, int Order, Scope Scope, String Expression)
            : base(Name, Order, Scope)
        {

            PrevailLexer lexer = new PrevailLexer(new AntlrInputStream(Expression));
            
            PrevailParser parser = new PrevailParser(new CommonTokenStream(lexer));

            //IParseTree tree = parser;

            ParseTreeWalker walker = new ParseTreeWalker();

            PrevailCommandListener andiWalker = new PrevailCommandListener();

            //walker.Walk(andiWalker, tree);

            //return andiWalker.Commands;


        }

        public override Condition Condition
        {
            get { return _condition; }
        }


        public override string Description
        {
            get { throw new NotImplementedException(); }
        }
    }
}
