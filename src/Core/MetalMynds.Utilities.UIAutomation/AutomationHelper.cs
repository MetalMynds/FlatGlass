using System;
using System.Windows.Automation;

namespace MetalMynds.Utilities
{
    public static class AutomationHelper
    {

        public static AutomationProperty GetProperty(String ShortName)
        {
            try
            {
                String fullName = String.Format("{0}Property", ShortName);

                return
                    (AutomationProperty)
                    ReflectionHelper.GetStaticFieldValue(typeof (AutomationElement), fullName, true);
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
