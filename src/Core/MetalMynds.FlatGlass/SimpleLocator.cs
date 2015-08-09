// -----------------------------------------------------------------------
// <copyright file="Locator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Windows.Automation;

    using MetalMynds.Utilities;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleLocator : Locator
    {
        private readonly How _how;
        private readonly String _usingValue;
        private readonly String _controlTypeName;
        private readonly String _description;
        private readonly Condition _condition;

        public SimpleLocator(String Name, int Order, Scope Scope, How How, String Using, String ControlTypeName = null)
            : base(Name, Order, Scope)
        {
            _how = How;
            _usingValue = Using;
            _controlTypeName = ControlTypeName;
            _description = String.Format("Simple Locator: Order: {0} Scope: [{1}] How: [{2}] Using: [{3}] Control Type: [{4}]", Order, Scope, How.ToString(), Using, ControlTypeName == null ? "All" : ControlTypeName);

            switch (How)
            {
                case How.Class:
                    _condition = new PropertyCondition(AutomationElement.ClassNameProperty, _usingValue);
                    break;
                case How.AutomationId:
                    _condition = new PropertyCondition(AutomationElement.AutomationIdProperty, _usingValue);
                    break;
                case How.AutomationName:
                    _condition = new PropertyCondition(AutomationElement.NameProperty, _usingValue);
                    break;
            }

            if (!String.IsNullOrWhiteSpace(_controlTypeName))
            {

                var controlType = AutomationHelper.GetControlType(_controlTypeName);

                if (controlType == null)
                {

                    throw new InvalidControlTypeNameException(_controlTypeName);

                }
                else
                {

                    _condition = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType), _condition);

                }

            }

        }
    
        public How How
        {
            get
            {
                return _how;
            }
        }

        public String Using
        {
            get
            {
                return _usingValue;
            }
        }

        public String ControlTypeName
        {
            get
            {
                return _controlTypeName;
            }
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

    public class InvalidControlTypeNameException : Exception
    {
        public InvalidControlTypeNameException(String InvalidControlName)
            : base(String.Format("Control Type Name: [{0}] Is Not Recognised As a Valid ControlType! Check the System.Windows.Automation.ControlType Class for Reference.", InvalidControlName))
        {

        }
    }
}
