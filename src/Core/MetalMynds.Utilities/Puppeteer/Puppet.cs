using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;

namespace MetalMynds.Utilities.Puppeteer
{
    public class Puppet
    {
        protected Strings BaseStrings;
        protected String BaseClassName;
        protected TypeBuilder BaseClass;
        protected Dictionary<String, Type> BaseProperties = new Dictionary<String, Type>();

        public Puppet(String ClassName)
        {
            BaseClassName = ClassName;
            BaseStrings = new Strings();
            BaseClass = Initialise(BaseClassName);
        }

        public Puppet(String ClassName, AppDomain Domain, String AssemblyName)
        {
            BaseClassName = ClassName;
            BaseStrings = new Strings(Domain, AssemblyName);
            BaseClass = Initialise(BaseClassName);
        }

        public Puppet(String ClassName, AppDomain Domain, String AssemblyName, String ModuleName)
        {
            BaseClassName = ClassName;
            BaseStrings = new Strings(Domain, AssemblyName, ModuleName);
            BaseClass = Initialise(BaseClassName);
        }

        protected virtual TypeBuilder Initialise(String ClassName)
        {
            return BaseStrings.CreateClass(ClassName);
        }

        public virtual System.Type NewEnum(String Name, List<String> Enumerates)
        {
            return BaseStrings.CreateEnum(Name, Enumerates).CreateType();
        }

        public virtual void AddProperty(String Name, Type PropertyType, Boolean ReadOnly, String Description, String Category, Boolean Visible)
        {
            String fieldName = Name + "_Field";

            FieldBuilder propertyField = BaseStrings.CreateField(BaseClass, PropertyType, fieldName, true);

            PropertyBuilder newProperty = BaseStrings.CreateProperty(BaseClass, Name, PropertyType);

            if (ReadOnly)
            {
                CustomAttributeBuilder ReadOnlyAttribute = BaseStrings.CreateAttribute(newProperty, typeof(ReadOnlyAttribute), new Type[] {typeof(Boolean)}, new Object[] {true});
            }

            if (Description != null && Description != String.Empty)
            {
                CustomAttributeBuilder DescriptionAttribute = BaseStrings.CreateAttribute(newProperty, typeof(DescriptionAttribute), new Type[] { typeof(String) }, new Object[] { Description });
            }

            if (!Visible)
            {
                CustomAttributeBuilder BrowsableAttribute = BaseStrings.CreateAttribute(newProperty, typeof(BrowsableAttribute), new Type[] { typeof(Boolean) }, new Object[] { false });
            }

            if (Category != null && Category != String.Empty)
            {
                CustomAttributeBuilder CategoryAttribute = BaseStrings.CreateAttribute(newProperty, typeof(CategoryAttribute), new Type[] { typeof(String) }, new Object[] { Category });
            }

            MethodBuilder getMethod = BaseStrings.CreateGetMethod(BaseClass, newProperty, propertyField, true, PropertyType);

            MethodBuilder setMethod = BaseStrings.CreateSetMethod(BaseClass, newProperty, propertyField, true, PropertyType);

            BaseProperties.Add(Name,PropertyType);
                
        }

        public virtual System.Type PuppetType
        {
            get
            {
                return BaseClass.CreateType();
            }
        }
        
        public virtual Dictionary<String, Type> Properties
        {
            get
            {
                return BaseProperties;
            }
        }
            

        public Object CreateInstance()
        {
            Object instance;

            instance = Activator.CreateInstance(this.PuppetType);

            return instance;

        }

    }
}