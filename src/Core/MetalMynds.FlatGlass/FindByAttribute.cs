// -----------------------------------------------------------------------
// <copyright file="FindByAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MetalMynds.FlatGlass
{
    /// <summary>
    ///     FindBy Attribute Allows Zero-Configuration Window Definitions with Lazy Loaded Control Creation.
    /// </summary>    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class FindByAttribute : Attribute
    {
        private readonly Locator _locator;
        private readonly int _order = -1;

        /// <summary>
        /// Find UI Element By Using Expression (DSL)
        /// </summary>
        /// <param name="Order">Specifies Order of Chained Look-Up, Does Not Have to be Sequential Or Start at 0 and Are Local to PlaceHolder.</param>
        /// <param name="Expression">Simple Boolean Expression Tree</param>
        /// <param name="Scope">Scope of Search</param>
        public FindByAttribute(int Order, String Expression, Scope Scope = Scope.ChildrenOnly)
        {
            _order = Order;

            _locator = new DslLocator(String.Empty, _order, Scope, Expression);
        }

        /// <summary>
        /// Find Element By Using Common Property Conditions.
        /// </summary>
        /// <param name="Order">Specifies Order of Chained Look-Up, Does Not Have to be Sequential Or Start at 0 and Are Local to PlaceHolder.</param>
        /// <param name="How">Specifies the Method Used to Find Controls.</param>
        /// <param name="Using">Used as a Parameter to the Specified 'How' Method.</param>
        /// <param name="Scope">Scope of Search</param>
        /// <param name="ControlType">ControlType Specifies System.Windows.Automation.ControlType as String</param>
        public FindByAttribute(int Order, How How, String Using, Scope Scope = Scope.ChildrenOnly,
                               String ControlType = "")
        {
            _order = Order;

            _locator = new SimpleLocator(String.Empty, _order, Scope, How, Using, ControlType);
        }

        /// <summary>
        /// Find UI Element By Using And/Or Property Condition KeyValue Pairs.
        /// </summary>
        /// <param name="Order">Specifies Order of Chained Look-Up, Does Not Have to be Sequential Or Start at 0 and Are Local to PlaceHolder.</param>
        /// <param name="How">Specifies the Method Used to Find Controls.</param>
        /// <param name="Scope">Scope of Search</param>
        /// <param name="PropertyValuePairs">Key Value Pairs. Equals: Property==Value or Property=Value. Not Equals: Property!=Value or Property&lt;&gt;Value.</param>
        public FindByAttribute(int Order, How How, Scope Scope = Scope.ChildrenOnly, params String[] PropertyValuePairs)
        {
            _order = Order;

            if (How == How.AndProperty)
            {
                _locator = new ExpressionLocator(String.Empty, _order, Scope, ExpressionLocator.ConditionType.And,
                                                PropertyValuePairs);
            }
            else if (How == How.OrProperty)
            {
                _locator = new ExpressionLocator(String.Empty, _order, Scope, ExpressionLocator.ConditionType.Or,
                                                PropertyValuePairs);
            }
            else
            {
                throw new ArgumentException("Invalid How, for Key Value Conditions How Must Equal And or Or!");
            }
        }

        /// <summary>
        /// Find UI Element By Using Locators from a Well Known Place Holder.
        /// </summary>
        /// <param name="Parent">Name of a Well Known Place Holder, a Field/Property with WellKnownAs Attribute. (Always Selected as First FindBy).</param>
        public FindByAttribute(String Parent)
        {
            _locator = new WellKnownReferenceLocator(Parent);
        }

        /// <summary>
        /// Locator Created By the Attribute.
        /// </summary>
        public virtual Locator Locator
        {
            get { return _locator; }
        }

        /// <summary>
        /// Order of FindBy Chained Location. Does Not Have to be Sequential, But Duplicates Will Lead to Unexpected results.
        /// </summary>
        public virtual int Order
        {
            get { return _order; }
        }
    }

    /// <summary>
    /// Defines the Scope of the Search, Relative to Either the Window Root or the Previous FindBy.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// None Scope has Been Specified.
        /// </summary>
        None,
        /// <summary>
        /// The Parent AutomationElement Only.
        /// </summary>
        Self,
        /// <summary>
        /// Only the Children of Parent AutomationElement.
        /// </summary>
        ChildrenOnly,
        /// <summary>
        /// Descendants of Parent AutomationElement.
        /// </summary>
        Descendants,
        /// <summary>
        /// Search All.
        /// </summary>
        All
    }

    /// <summary>
    /// Common Location Strategies Used to FindBy.
    /// </summary>
    public enum How
    {
        /// <summary>
        /// AutomationElement Class.
        /// </summary>
        Class,
        /// <summary>
        /// AutomationElement ID.
        /// </summary>
        AutomationId,
        /// <summary>
        /// AutomationElement Name.
        /// </summary>
        AutomationName,
        /// <summary>
        /// Automation Properties (And)
        /// </summary>
        AndProperty,
        /// <summary>
        /// Automation Properties (Or)
        /// </summary>
        OrProperty
    }
}