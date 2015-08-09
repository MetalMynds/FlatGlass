using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MetalMynds.Utilities.Puppeteer
{
    public class Strings
    {
        protected const MethodAttributes BasePropertyMethodAttr = MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Public;
        protected AppDomain BaseAppDomain;
        protected String BaseName = System.Guid.NewGuid().ToString("n");
        protected AssemblyBuilder BaseAssemblyBuilder;
        protected const String BASE_MAIN_MODULE_NAME = "Main";
        protected String BaseModuleName = BASE_MAIN_MODULE_NAME;
        protected ModuleBuilder BaseModuleBuilder;
        protected AssemblyName BaseAssemblyName;

        public Strings()
        {
            BaseAppDomain = Thread.GetDomain();
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }
    
        public Strings(AppDomain Domain)
        {
            BaseAppDomain = Domain;
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }

        public Strings(AppDomain Domain, String AssemblyName)
        {
            BaseName = AssemblyName;
            BaseAppDomain = Domain;
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }

        public Strings(String AssemblyName)
        {
            BaseName = AssemblyName;
            BaseAppDomain = Thread.GetDomain();
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }

        public Strings(String AssemblyName, String ModuleName)
        {
            BaseName = AssemblyName;
            BaseModuleName = ModuleName;
            BaseAppDomain = Thread.GetDomain();
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }

        public Strings(AppDomain Domain, String AssemblyName, String ModuleName)
        {
            BaseName = AssemblyName;
            BaseModuleName = ModuleName;
            BaseAppDomain = Domain;
            BaseAssemblyBuilder = Initialise(BaseAppDomain, BaseName, BaseModuleName);
        }

        public String AssemblyName
        {
            get
            {
                return BaseName;
            }
        }

        protected virtual AssemblyBuilder Initialise(AppDomain Domain, String AssembelyName, String ModuleName)
        {

            BaseAssemblyName = new AssemblyName(AssembelyName);

            BaseAssemblyBuilder = Domain.DefineDynamicAssembly(BaseAssemblyName, AssemblyBuilderAccess.Run);

            BaseModuleBuilder = BaseAssemblyBuilder.DefineDynamicModule(ModuleName);

            // Hook up the event listening.
            TypeResolveHandler typeResolveHandler = new TypeResolveHandler(BaseModuleBuilder);

            // Add a listener for the type resolve events.

            ResolveEventHandler resolveHandler = new ResolveEventHandler(typeResolveHandler.ResolveEvent);

            BaseAppDomain.TypeResolve += resolveHandler;

            return BaseAssemblyBuilder;

        }

        public EnumBuilder CreateEnum(String EnumName, List<String> Items)
        {

            EnumBuilder enumBuilder = BaseModuleBuilder.DefineEnum(EnumName, TypeAttributes.Public, typeof(int));

            int index = 0;

            enumBuilder.DefineLiteral("Unknown", index);

            foreach (String itemName in Items)
            {
                index += 1;
                enumBuilder.DefineLiteral(itemName, index);
           }

            return enumBuilder;

        }

        //public virtual Descr  CreateAttribute()
        //{

        //}

        public CustomAttributeBuilder CreateAttribute(PropertyBuilder Property, Type Attribute, Type[] Parameters, Object[] Values)
        {
            ConstructorInfo attributeConstructor = Attribute.GetConstructor(Parameters);

            CustomAttributeBuilder newAttribute = new CustomAttributeBuilder(attributeConstructor,Values);

            Property.SetCustomAttribute(newAttribute);

            return newAttribute;
        }

        public TypeBuilder CreateClass(String ClassName)
        {
            TypeBuilder typeBuilder = BaseModuleBuilder.DefineType(ClassName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                typeof(object));

            return typeBuilder;

        }

        public ConstructorBuilder CreateConstructor(TypeBuilder Class)
        {
            //ConstructorBuilder constructor = Class.DefineConstructor(
            //MethodAttributes.Public,
            //CallingConventions.Standard,
            //null);

            //ILGenerator ctor1IL = constructor.GetILGenerator();
            //// For a constructor, argument zero is a reference to the new
            //// instance. Push it on the stack before calling the base
            //// class constructor. Specify the default constructor of the 
            //// base class (System.Object) by passing an empty array of 
            //// types (Type.EmptyTypes) to GetConstructor.
            //ctor1IL.Emit(OpCodes.Ldarg_0);
            //ctor1IL.Emit(OpCodes.Call,
            //    typeof(object).GetConstructor(Type.EmptyTypes));
            //// Push the instance on the stack before pushing the argument
            //// that is to be assigned to the private field m_number.
            //ctor1IL.Emit(OpCodes.Ldarg_0);
            //ctor1IL.Emit(OpCodes.Ldarg_1);
            //ctor1IL.Emit(OpCodes.Stfld, fbNumber);
            //ctor1IL.Emit(OpCodes.Ret);


            //return constructor;
            return null;
        }

        public FieldBuilder CreateField(TypeBuilder Class, System.Type Type, String Name, Boolean Public)
        {

            FieldAttributes attributes;

            if (Public)
            {
                attributes = FieldAttributes.Public;
            }
            else
            {
                attributes = FieldAttributes.Private;
            }

            FieldBuilder fieldBuilder = Class.DefineField(Name, Type, attributes);

            return fieldBuilder;

        }

        public PropertyBuilder CreateProperty(TypeBuilder Class, String Name, System.Type Type)
        {

            PropertyBuilder propertyBuilder = Class.DefineProperty(Name, PropertyAttributes.None, Type, null);

            return propertyBuilder;

        }

        public MethodBuilder CreateGetMethod(TypeBuilder Class, PropertyBuilder Property, FieldBuilder Field, Boolean Public, System.Type ReturnType)
        {

            String methodName = "get_" + Property.Name;

            MethodBuilder getMethod = Class.DefineMethod(methodName,
                                           BasePropertyMethodAttr,
                                           ReturnType,
                                           Type.EmptyTypes);

            ILGenerator getIL = getMethod.GetILGenerator();

            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, Field);
            getIL.Emit(OpCodes.Ret);

            Property.SetGetMethod(getMethod);

            return getMethod;

        }

        public MethodBuilder CreateSetMethod(TypeBuilder Class, PropertyBuilder Property, FieldBuilder Field, Boolean Public, System.Type ReturnType)
        {
            String methodName = "set_" + Property.Name;

            MethodBuilder setMethod = Class.DefineMethod(methodName,
                               BasePropertyMethodAttr,
                               null,
                               new Type[] { ReturnType });

            ILGenerator setIL = setMethod.GetILGenerator();

            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, Field);
            setIL.Emit(OpCodes.Ret);

            Property.SetSetMethod(setMethod);

            return setMethod;

        }

        public MethodBuilder CreateMethod(TypeBuilder Class, String Name, Boolean Public, MethodAttributes Attributes, System.Type ReturnType)
        {

            MethodAttributes methodAttributes = Attributes;

            if (Public)
            {
                methodAttributes |= MethodAttributes.Public;
            }
            else
            {
                methodAttributes |= MethodAttributes.Private;
            }

            MethodBuilder methodBuilder = Class.DefineMethod(Name,
                                           methodAttributes,
                                           ReturnType,
                                           Type.EmptyTypes);

            return methodBuilder;

        }

        public AssemblyBuilder AssemblyBuilder
        {
            get
            {
                return BaseAssemblyBuilder;
            }
        }

        public ModuleBuilder ModuleBuilder
        {
            get
            {
                return BaseModuleBuilder;
            }
        }
    }

    public class TypeResolveHandler
    {
        private Module ModuleMain;

        public TypeResolveHandler(Module Module)
        {
            ModuleMain = Module;
        }

        public Assembly ResolveEvent(Object sender, ResolveEventArgs args)
        {
            Console.WriteLine(args.Name);
            // Use args.Name to look up the type name. In this case, you are getting AnEnum.
            try
            {
                //NestedEnum.tNested = NestedEnum.enumType.CreateType();
            }
            catch
            {
                // This is needed to throw away InvalidOperationException.
                // Loader might send the TypeResolve event more than once
                // and the type might be complete already.
            }

            // Complete the type.		    
            return ModuleMain.Assembly;
        }
    }
}
