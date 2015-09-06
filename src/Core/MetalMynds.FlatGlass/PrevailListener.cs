using MetalMynds.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace MetalMynds.FlatGlass
{
    public class PrevailListener : PrevailBaseListener
    {

        protected Condition _condition;

        protected readonly Stack<Condition> _conditionStack = new Stack<Condition>();

        protected Object _value;

        public virtual Condition Result
        {
            get
            {
                return _condition;
            }
        }

        public override void ExitExpression(PrevailParser.ExpressionContext context)
        {
            base.ExitExpression(context);

            if (_conditionStack.Count != 1)
            {
                throw new PrevailException(String.Format("Expect Result of Parse Expression. Expecting 1 Condition Found {0}.", _conditionStack.Count));
            }
            else
            {

                var result = _conditionStack.Pop();

                _condition = result;
            }
        }

        public override void ExitAndCondition(PrevailParser.AndConditionContext context)
        {
            base.ExitAndCondition(context);
            
            var right = _conditionStack.Pop();

            var left = _conditionStack.Pop();

            var and = new AndCondition(new Condition[] { left, right });

            _conditionStack.Push(and);
        }

        public override void ExitOrCondition(PrevailParser.OrConditionContext context)
        {
            base.ExitOrCondition(context);

            var right = _conditionStack.Pop();

            var left = _conditionStack.Pop();            

            var or = new OrCondition(new Condition[] { left, right });

            _conditionStack.Push(or);

        }

        public override void ExitNotCondition(PrevailParser.NotConditionContext context)
        {
            base.ExitNotCondition(context);

            var condition = _conditionStack.Pop();

            var not = new NotCondition(condition);

            _conditionStack.Push(not);

        }

        public override void ExitPropertyCondition(PrevailParser.PropertyConditionContext context)
        {
            base.ExitPropertyCondition(context);

            MetalMynds.FlatGlass.PrevailParser.PropertyContext propertyContext = (MetalMynds.FlatGlass.PrevailParser.PropertyContext)context.children[0];

            var propertyName = propertyContext.IDENTIFIER().GetText();

            Boolean equals = propertyContext.equals() != null;

            var property = AutomationHelper.GetProperty(propertyName);

            var propertyCondition = new PropertyCondition(property, _value);

            var condition = (Condition)propertyCondition;

            if (!equals)
            {
                condition = (Condition)new NotCondition(condition);
            }

            _conditionStack.Push(condition);

        }

        public override void ExitValue(PrevailParser.ValueContext context)
        {
            base.ExitValue(context);

            if (context.number() != null)
            {

                var floatingNumber = context.number().FLOAT();

                if (floatingNumber != null)
                {
                    _value = float.Parse(floatingNumber.GetText());

                    return;
                }

                var intNumber = context.number().INT();

                if (intNumber != null)
                {
                    _value = int.Parse(intNumber.GetText());

                    return;
                }

            }

            var stringLiteral = context.STRING();

            if (stringLiteral != null)
            {
                var value = stringLiteral.GetText();

                if (value.StartsWith("'"))
                {
                    if (value.EndsWith("'"))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                }

                _value = value;

                return;
            }

        }

    }



    public class PrevailException : Exception
    {

        public PrevailException(String Message)
            : base(Message)
        {

        }


    }
}
