using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MetalMynds.Utilities;

namespace MetalMynds.Utilities.Puppeteer
{
    public class Glove
    {
        protected Puppet BasePuppet;
        protected Object BaseFingers;

        public Glove(Puppet Puppet)
        {
            BasePuppet = Puppet;
            BaseFingers = BasePuppet.CreateInstance();
        }

        public Glove(Object Fingers)
        {
            BaseFingers = Fingers;
        }

        public Puppet Puppet
        {
            get
            {
                return BasePuppet;
            }
        }

        public virtual Boolean HasProperty(String Name)
        {
            PropertyInfo property = BaseFingers.GetType().GetProperty(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            return (property != null);
        }

        public virtual Boolean HasMethod(String Name)
        {
            MethodInfo method = BaseFingers.GetType().GetMethod(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            return (method != null);
        }

        public virtual Boolean HasField(String Name)
        {
            FieldInfo field = BaseFingers.GetType().GetField(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            return (field != null);
        }

        public virtual Object GetMethod(String Name)
        {

            MethodInfo method = BaseFingers.GetType().GetMethod(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (method == null)
            {
                throw new MethodNotRecognisedException(Name);
            }

            return method.Invoke(BaseFingers, null);

        }

        public virtual Object GetProperty(String Name)
        {

            PropertyInfo property = BaseFingers.GetType().GetProperty(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (property == null)
            {
                throw new PropertyNotRecognisedException(Name);
            }

            return property.GetValue(BaseFingers, null);

        }

        public virtual Object GetField(String Name)
        {

            FieldInfo field = BaseFingers.GetType().GetField(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (field  == null)
            {
                throw new FieldNotRecognisedException(Name);
            }

            return field.GetValue(BaseFingers);

        }

        public virtual void SetProperty(String Name, Object Value)
        {

            PropertyInfo property = BaseFingers.GetType().GetProperty(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (property == null)
            {
                throw new PropertyNotRecognisedException(Name);
            }

            if (property.PropertyType.IsEnum)
            {
                Value = Enum.Parse(property.PropertyType, (String)Value, true);
            }

            property.SetValue(BaseFingers, Value, null);

        }

        public virtual void SetField(String Name, Object Value)
        {

            FieldInfo field = BaseFingers.GetType().GetField(Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (field == null)
            {
                throw new PropertyNotRecognisedException(Name);
            }

            if (field.FieldType.IsEnum)
            {
                Value = Enum.Parse(field.FieldType, (String)Value, true);
            }

            field.SetValue(BaseFingers, Value);

        }
        public Object Fingers
        {
            get
            {
                return BaseFingers;
            }
        }

    }

    public class PropertyNotRecognisedException : Exception
    {

        public PropertyNotRecognisedException(String PropertyName) 
            : base(String.Format("Property [{0}] is Not Recognised!",PropertyName))
        {
        }

    }

    public class MethodNotRecognisedException : Exception
    {

        public MethodNotRecognisedException(String MethodName)
            : base(String.Format("Method [{0}] is Not Recognised!", MethodName))
        {
        }

    }

    public class FieldNotRecognisedException : Exception
    {

        public FieldNotRecognisedException(String PropertyName)
            : base(String.Format("Field [{0}] is Not Recognised!", PropertyName))
        {
        }

    }
}
