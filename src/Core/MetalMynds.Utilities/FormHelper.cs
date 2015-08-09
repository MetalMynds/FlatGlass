using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public class FormHelper
    {

        public static void Disable(Form Target, List<Control> Exceptions)
        {

            if (Target != null)
            {

                action(Target, false);

                foreach (Control exception in Exceptions)
                {
                    chain(exception,true);
                }

            }

        }

        public static void Enable(Form Target, List<Control> Exceptions)
        {

            if (Target != null)
            {

                action(Target, true);

                foreach (Control exception in Exceptions)
                {
                    chain(exception, false);
                }

            }

        }

        protected static void chain(Control Target,Boolean Enable)
        {

            Target.Enabled = Enable;

            if (Target.Parent != null && Enable)
            {
                chain(Target.Parent, Enable);
            }

        }

        protected static void action(Control Parent, Boolean Enable)
        {

            foreach (Control control in Parent.Controls)
            {

                control.Enabled = Enable;

                if (control.Controls.Count > 0)
                {
                    action(control, Enable);
                }

            }

        }

        protected static void exceptions(List<Control> Exceptions,Boolean Enable)
        {

            foreach (Control control in Exceptions)
            {

                chain(control,Enable);

            }

        }

    }
}
