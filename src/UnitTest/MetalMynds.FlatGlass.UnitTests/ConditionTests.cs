using System;
using System.Collections.Generic;
using System.Text;

using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using MetalMynds.FlatGlass;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using MetalMynds.Utilities;

namespace MetalMynds.FlatGlass.UnitTests
{
    [TestClass]
    public class ConditionTests
    {
        [TestMethod]
        public void VerifyBasicConditions()
        {

            // Class = 'Awsum'

            Condition result1 = new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum");

            var expression = new ExpressionLocator("", 1, Scope.Descendants, "Class = 'Awsum'");

            var result1Expression = expression.Condition;

            Assert.IsTrue(AutomationHelper.Compare(result1, result1Expression));

            // Class != 'Awsum'

            Condition result2 = 
                new NotCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum"));

            var expression2 = new ExpressionLocator("", 1, Scope.Descendants, "Class != 'Awsum'");

            var result2Expression = expression2.Condition;

            Assert.IsTrue(AutomationHelper.Compare(result2, result2Expression));

            // Class = 'Awsum' And Name = 'Adi'

            Condition result3 = 
                new AndCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum"), 
                    new PropertyCondition(AutomationElement.NameProperty, "Adi"));

        }



    }

}