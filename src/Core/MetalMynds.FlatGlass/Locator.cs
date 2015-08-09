// -----------------------------------------------------------------------
// <copyright file="Locator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Windows.Automation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class Locator
    {
        protected String BaseName;
        protected int BaseOrder;
        protected Scope BaseScope;
        
        protected Locator(String Name, int Order, Scope Scope)
        {
            BaseName = Name;
            BaseOrder = Order;
            BaseScope = Scope;
        }

        public virtual String Name
        {
            get
            {
                return BaseName;
            }
        }

        public virtual int Order
        {
            get
            {
                return BaseOrder; 
            }
        }

        public abstract Condition Condition { get; }

        public abstract String Description { get;  }

        public override string ToString()
        {
            return Description;
        }

        public virtual Scope Scope
        {
            get { return BaseScope; }
        }


    }
}
