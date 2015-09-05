using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace MetalMynds.FlatGlass
{
    public class PrevailVisitor : PrevailBaseVisitor<Condition>
    {

        public override Condition VisitAndCondition(PrevailParser.AndConditionContext context)
        {
            var conditions = context.condition();

            return base.VisitAndCondition(context);
            
        }

    }
}
