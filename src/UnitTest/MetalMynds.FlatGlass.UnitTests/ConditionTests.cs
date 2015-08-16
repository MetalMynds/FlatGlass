using System;
using System.Collections.Generic;
using System.Text;

using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using MetalMynds.FlatGlass;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;

namespace MetalMynds.FlatGlass.UnitTests
{
    [TestClass]
    public class ConditionTests
    {
        [TestMethod]
        public void ConstructComplexConditionsProgrammatically()
        {

            // Class = 'Awsum'

            Condition result1 = new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum");

            // Class != 'Awsum'

            Condition result2 = 
                new NotCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum"));

            // Class = 'Awsum' And Name = 'Adi'

            Condition result3 = 
                new AndCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Awsum"), 
                    new PropertyCondition(AutomationElement.NameProperty, "Adi"));

        }



    }

}