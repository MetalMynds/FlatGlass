using System;
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

                // Handle 2 Shortcuts 
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

    }
}
