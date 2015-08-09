using System;
using System.Collections.Generic;
using System.Text;

using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
//using Antlr4.Runtime.Debug;

using MetalMynds.FlatGlass;

using EntMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetalMynds.FlatGlass.UnitTests
{
    [TestClass]
    public class ConditionTests
    {
        [TestMethod]
        public void GenerateAst()
        {
            //ANTLRStringStream expression = new ANTLRStringStream("john =='dave' AND (joe OR sue)");
            ANTLRStringStream expression = new ANTLRStringStream("john AND (joe OR sue)");
            var tokens = new CommonTokenStream(new ConditionLexer(expression));
            var parser = new ConditionParser(tokens);

            var adaptor = new CommonTreeAdaptor();

            parser.TreeAdaptor = adaptor;

            var result = parser.prog();

            var tree = (CommonTree)result.Tree;

            Print(tree, 0);

        }

        [TestMethod]
        public void GenerateBetterAst()
        {
            //ANTLRStringStream expression = new ANTLRStringStream("john =='dave' AND (joe OR sue)");
            ANTLRStringStream expression = new ANTLRStringStream("Invoice <- Order ;\n\rInvoiceDate = Order.OrderDate + 30 ;\n\rInvoiceNo = GetNextInvoiceNo() ;\n\rFreight = Order.TotalCBM * 1.5 + Order.TotalWeight * 2.2;\n\rShipVia = IF(Order.IsLocal, \"Express\", \"Mail\") ;");
            var tokens = new CommonTokenStream(new ConditionLexer(expression));
            var parser = new EntityMappingParser(tokens);

            var adaptor = new CommonTreeAdaptor();

            parser.TreeAdaptor = adaptor;

            var result = parser.prog();

            var tree = (CommonTree)result.Tree;

            Print(tree, 0);

        }

        static void Print(CommonTree tree, int level)
        {
            System.Diagnostics.Debug.WriteLine(new string('\t', level) + tree.Text);
            if (tree.Children != null)
                foreach (CommonTree child in tree.Children)
                    Print(child, level + 1);
        }

    }

}