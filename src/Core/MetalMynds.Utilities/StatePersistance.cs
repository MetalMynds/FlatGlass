using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public class StatePersistance
    {
        protected ApplicationSettingsBase BaseSettings;
        protected Dictionary<String, Object> BaseValues = new Dictionary<string, object>();

        public StatePersistance(ApplicationSettingsBase Settings)
        {
            BaseSettings = Settings;
        }

        protected virtual void FitToScreen(System.Windows.Forms.Form Target)
        {
            try
            {

                if (Target.Width > Screen.PrimaryScreen.WorkingArea.Width)
                {
                    Target.Width = Screen.PrimaryScreen.WorkingArea.Width;
                }
                if (Target.Height > Screen.PrimaryScreen.WorkingArea.Height)
                {
                    Target.Height = Screen.PrimaryScreen.WorkingArea.Height;
                }
            }
            catch
            {
            }

        }

        public void Restore(System.Windows.Forms.Form Target)
        {
            // Called from Form.OnLoad

            // Remember the initial window state and set it to Normal before sizing the form
            FormWindowState initialWindowState = Target.WindowState;
            Target.WindowState = FormWindowState.Normal;
            
            try
            {
                Target.Size = (System.Drawing.Size)Get(Target, "Size", Target.Size);
            }
            catch
            {
            }

            // Fit to the current screen size in case the screen resolution
            // has changed since the size was last persisted.
            FitToScreen(Target);
            
            FormWindowState savedWindowState = (FormWindowState)Get(Target, "WindowState", FormWindowState.Normal);
            bool isMaximized = savedWindowState == FormWindowState.Maximized;

            Target.WindowState = isMaximized ? FormWindowState.Maximized : FormWindowState.Normal;

        }

        public void Persist(System.Windows.Forms.Form Target)
        {
            Set(Target, "Size", Target.Size);
            Set(Target, "WindowState", Target.WindowState);
        }

        public void Restore(System.Windows.Forms.SplitContainer Target)
        {
            int initialValue = Target.SplitterDistance;

            try
            {

                Target.SplitterDistance = (int)Get(Target, "SplitterDistance", Target.SplitterDistance);

            }
            catch
            {
                Target.SplitterDistance = initialValue;
            }
        }

        public void Persist(System.Windows.Forms.SplitContainer Target)
        {
            Set(Target, "SplitterDistance", Target.SplitterDistance);
        }

        protected virtual void Set(System.Windows.Forms.Control Control, String Name, Object Value)
        {
            String key = GenerateKey(Control, Name);

            try
            {
                BaseSettings.PropertyValues[key].PropertyValue = Value;
            }
            catch
            {

                throw new InvalidOperationException(String.Format("State Settings Property Name [{0}] Type [{1}] Value [{2}] Does Not Exist!",key,Value.GetType().FullName,Value.ToString()));

                //SettingsProperty newProperty = new SettingsProperty(key);  

                //BaseSettings.Properties.Add(newProperty);

                //newProperty.PropertyType = Value.GetType();
                //newProperty.DefaultValue = Value;
                //newProperty.SerializeAs = SettingsSerializeAs.String;

            }

            //BaseSettings
        }

        //protected virtual GetSettings(SettingsContext Context) {
        //}

        protected virtual String GenerateKey(System.Windows.Forms.Control Control, String Name)
        {
            return "State_" + FullyQualifyName(Control) + "_" + Name;
        }

        protected virtual Object Get(System.Windows.Forms.Control Control, String Name, Object Default)
        {
            String key = GenerateKey(Control, Name);

            try
            {
                SettingsPropertyValue property = BaseSettings.PropertyValues[key];

                object value = property.PropertyValue;

                if (value != null)
                {
                    return value;
                }
                else
                {
                    return Default;
                }
            }
            catch
            {
                return Default;
            }

        }

        protected virtual String FullyQualifyName(System.Windows.Forms.Control Control) {

            String fullyQualifiedControlName = Control.Name;

            if (Control.Parent != null)
            {
                if (Control.Parent is System.Windows.Forms.Form)
                {
                    return fullyQualifiedControlName;
                }
                else
                {
                    if (fullyQualifiedControlName == String.Empty || fullyQualifiedControlName == null)
                    {
                        fullyQualifiedControlName = FullyQualifyName(Control.Parent);
                    }
                    else
                    {
                        fullyQualifiedControlName += "_" + FullyQualifyName(Control.Parent) ;
                    }

                    return fullyQualifiedControlName;
                }
            }
            else
            {
                return fullyQualifiedControlName; 
            }

        }

    }
}
