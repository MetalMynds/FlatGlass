using System;
using System.Collections;
using System.Windows.Automation;

namespace MetalMynds.Utilities
{
    public static class AutomationHelper
    {

        public static AutomationProperty GetProperty(String ShortName)
        {

            if (String.IsNullOrWhiteSpace(ShortName))
            {
                throw new ArgumentNullException("GetProperty ShortName Parameter can't be Null or Empty or Whitespace!");
            }

            try
            {
                //
                //
                //  Handle 2 Shortcuts 
                // 
                //        ClassName as Class 
                //        
                //        AutomationId as Id
                //
                //

                if (String.Equals(ShortName, "Class", StringComparison.InvariantCultureIgnoreCase))
                {
                    ShortName = "ClassName";
                }

                if (String.Equals(ShortName, "Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    ShortName = "AutomationId";
                }

                String fullPropertyName = String.Format("{0}Property", ShortName);

                return
                    (AutomationProperty)
                    ReflectionHelper.GetStaticFieldValue(typeof(AutomationElement), fullPropertyName, true);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ControlType GetControlType(String ShortName)
        {
            try
            {
                String fullName = String.Format("{0}", ShortName);

                return (ControlType)ReflectionHelper.GetStaticFieldValue(typeof(ControlType), fullName, true);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static Boolean Compare(Condition x, Condition y)
        {

            var and = x as AndCondition;

            if (and != null)
            {

                var and2 = y as AndCondition;

                if (and2 == null)
                {
                    return false;
                }

                var andConditions = and.GetConditions();

                var andConditions2 = and2.GetConditions();

                int count = 0;

                foreach (var condition in andConditions)
                {
                    if (!Compare(condition, andConditions2[count]))
                    {
                        return false;
                    }
                }
            }

            var or = x as OrCondition;

            if (or != null)
            {

                var or2 = y as OrCondition;

                if (or2 == null)
                {
                    return false;
                }

                var orConditions = or.GetConditions();

                var orConditions2 = or2.GetConditions();

                int count = 0;

                foreach (var condition in orConditions)
                {
                    if (!Compare(condition, orConditions2[count]))
                    {
                        return false;
                    }
                }
            }

            var not = x as NotCondition;

            if (not != null)
            {

                var not2 = y as NotCondition;

                if (not2 == null)
                {
                    return false;
                }

                if (!Compare(not.Condition, not2.Condition))
                {
                    return false;
                }

            }

            var property = x as PropertyCondition;

            if (property != null)
            {

                var property2 = y as PropertyCondition;

                if (property2 == null)
                {
                    return false;
                }

                if (property.Property.ProgrammaticName != property2.Property.ProgrammaticName)
                {
                    return false;
                }

                if (property.Value.ToString() != property2.Value.ToString())
                {
                    return false;
                }

                if (property.Flags != property2.Flags)
                {
                    return false;
                }
            }

            return true;
        }            

    }
}
