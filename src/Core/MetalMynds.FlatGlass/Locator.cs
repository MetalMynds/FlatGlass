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
        
        protected Locator(String Name, int Order, Scope Scope)
        {
            this.Name = Name;
            this.Order = Order;
            this.Scope = Scope;
        }

        public virtual String Name
        {
            get;
            protected set;
            
        }

        public virtual int Order
        {
            get;
            protected set;
        }

        public virtual Scope Scope
        {
            get;
            protected set;
        }

        public abstract Condition Condition { get; }

        public abstract String Description { get;  }

        public override string ToString()
        {
            return Description;
        }

    }
}
