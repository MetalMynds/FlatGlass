// -----------------------------------------------------------------------
// <copyright file="AttributeHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AttributeHelper
    {

        public static MethodInfo[] GetForgets(Type Window)
        {
            var forgetMethods = new List<MethodInfo>();

            const BindingFlags allInstanceBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var methods = Window.GetMethods(allInstanceBindingOptions);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ForgetsAttribute), false);

                if (attributes.Any())
                {
                    forgetMethods.Add(method);
                }
            }

            return forgetMethods.ToArray();
        }

        public static String GetWellKnownName(MemberInfo Member)
        {
            var attributes = Member.GetCustomAttributes(typeof(WellKnownAsAttribute), false);

            if (attributes.Any())
            {
                var wellKnown = attributes[0] as WellKnownAsAttribute;

                if (wellKnown != null)
                {
                    return wellKnown.Name;
                }
            }

            return null;
        }

        public static Boolean HasNotAutomationWindow(Type Class)
        {
            var attributes = Class.GetCustomAttributes(typeof(NotAutomationWindowAttribute), false);

            if (attributes.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static Boolean HasAutomationWindow(Type Class)
        {
            var attributes = Class.GetCustomAttributes(typeof(AutomationWindowAttribute), false);

            if (attributes.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
