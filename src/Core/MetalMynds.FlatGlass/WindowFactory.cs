using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Automation;

using MetalMynds.Utilities.Puppeteer;

namespace MetalMynds.FlatGlass
{
    
    public class WindowFactory
    {
        public delegate Object ConstructFromElement(Type controlType, AutomationElement element, String name);

        private static ConstructFromElement _controlConstructor;

        /// <summary>
        /// Creates a Window and Initialises all of its PlaceHolders. Window Must Define a Constructor taking One Parameter of Type AutomationElement.
        /// </summary>
        /// <typeparam name="T">Window Class Type</typeparam>
        /// <param name="root">AutomationElement that is Root to the Window and All Element Searches.</param>
        /// <returns></returns>
        public static T Create<T>(AutomationElement root)
        {            
            // Create Window 

            T window = default(T);
            
            Type windowClassType = typeof(T);
            
            // Find Required Constructor (A Single Parameter which is passed the AutomationElement passed to the WindowFactory.Create Method.)

            ConstructorInfo ctor = windowClassType.GetConstructor(new [] { typeof(AutomationElement) });
            
            if (ctor == null)
            {
                throw new ArgumentException("The Window Must Define a Constructor taking a Single Argument of Type AutomationElement!");
            }

            // Instantiate the Window 

            window = (T)ctor.Invoke(new object[] { root });
            
            // Initialise the Place Holder Objects.

            InitPlaceHolders(root, window);
            
            // Over to you.

            return window;
        }

        protected static void InitPlaceHolders(AutomationElement root, object window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window", "Parameter window Cannot Be Null!");
            }

            var windowPlaceHolderList = new List<IHoldPlace>();

            var windowType = window.GetType();

            var typeMembers = new List<MemberInfo>();
            
            const BindingFlags publicBindingOptions = BindingFlags.Instance | BindingFlags.Public;
            
            typeMembers.AddRange(windowType.GetFields(publicBindingOptions));
            typeMembers.AddRange(windowType.GetProperties(publicBindingOptions));
            
            while (windowType != null)
            {
                const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;

                typeMembers.AddRange(windowType.GetFields(nonPublicBindingOptions));
                typeMembers.AddRange(windowType.GetProperties(nonPublicBindingOptions));
                
                windowType = windowType.BaseType;
            }

            foreach (var member in typeMembers)
            {
                String wellKnownName = null;                

                wellKnownName = AttributeHelper.GetWellKnownName(member);

                List<Locator> locators = CreateLocatorPath(member);

                if (locators.Count > 0)
                {
                    bool cache = ShouldCacheLookup(member);

                    object placeholderObject = null;
            
                    var field = member as FieldInfo;
                    var property = member as PropertyInfo;
                   
                    if (field != null)
                    {
                        placeholderObject = CreatePlaceHolder(windowPlaceHolderList, field.Name, field.FieldType, root, locators, window, cache, wellKnownName);

                        if (placeholderObject == null)
                        {
                            throw new CreatePlaceHolderFailedException("Create PlaceHolder for Field [" + field.Name + "] Failed! Null Returned.");
                        }

                        field.SetValue(window, placeholderObject);
                    }
                    else if (property != null)
                    {
                        placeholderObject = CreatePlaceHolder(windowPlaceHolderList, property.Name, property.PropertyType, root, locators, window, cache, wellKnownName);

                        if (placeholderObject == null)
                        {
                            throw new CreatePlaceHolderFailedException("Create PlaceHolder for Property [" + property.Name + "] Failed! Null Returned.");
                        }

                        property.SetValue(window, placeholderObject, null);
                    }
                    else
                    {

                    }
                    
                }
            }
        }

        private static object CreatePlaceHolder(List<IHoldPlace> windowPlaceHolderList, String fieldPropertyName, Type genericTypedPlaceHolder, AutomationElement parent, List<Locator> locators, Object window, bool cache, String wellKnownName = null)
        {
                       
            object placeHolderObject = null;

            var arguments = new object[7];

            arguments[0] = windowPlaceHolderList;
            arguments[1] = fieldPropertyName;
            arguments[2] = parent;
            arguments[3] = locators;
            arguments[4] = window;
            arguments[5] = cache;
            arguments[6] = wellKnownName;

            placeHolderObject = Activator.CreateInstance(genericTypedPlaceHolder, arguments);
            
            return placeHolderObject;
        }

        public static ConstructFromElement ControlConstructor
        {
            get
            {                
                return _controlConstructor;
            }
            set
            {
                _controlConstructor = value;
            }
        }

        private static List<Locator> CreateLocatorPath(MemberInfo member)
        {
            var locators = new List<Locator>();

            var attributes = (FindByAttribute[])Attribute.GetCustomAttributes(member, typeof(FindByAttribute), true);

            if (attributes.Length > 0)
            {
                // Sort By Order
                
                // Like Tab Order Gaps are Not an Issue, (but if a Duplicate Order Number Exist No Guarantee which Order Execution)

                attributes = attributes.OrderBy(x => x.Order).ToArray<FindByAttribute>();

                foreach (var attribute in attributes)
                {
                    locators.Add(attribute.Locator);
                }
            }

            return locators;
        }

        private static bool ShouldCacheLookup(MemberInfo member)
        {
            var cacheAttributeType = typeof(CacheLookupAttribute);

            var declaringType = member.DeclaringType;

            bool cache = false;

            if (declaringType != null)
            {

                cache = member.GetCustomAttributes(cacheAttributeType, true).Length != 0 || declaringType.GetCustomAttributes(cacheAttributeType, true).Length != 0;

            }

            return cache;
        }

        public static bool IsWindow(Type Class)
        {
            if (AttributeHelper.HasNotAutomationWindow(Class)) return false;

            if (AttributeHelper.HasAutomationWindow(Class)) return true; // Force Automation Window

            const BindingFlags nonPublicBindingOptions =
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var typeMembers = new List<FieldInfo>();

            typeMembers.AddRange(Class.GetFields(nonPublicBindingOptions));

            foreach (FieldInfo field in typeMembers)
            {

                if (field.FieldType.IsGenericType)
                {

                    Type type = field.FieldType.GetGenericTypeDefinition();

                    if (type.Name.StartsWith(typeof(PlaceHolder<>).Name))
                    {

                        return true;

                    }

                }
            }
            return false;
        }

        public static void Forget(Object WindowInstance)
        {

            var windowGlove = new Glove(WindowInstance);

            Type windowType = WindowInstance.GetType();            

            const BindingFlags nonPublicBindingOptions = BindingFlags.Instance | BindingFlags.NonPublic;

            var typeFields = windowType.GetFields(nonPublicBindingOptions);

            foreach (FieldInfo fieldInfo in typeFields)
            {
                var placeholder = windowGlove.GetField(fieldInfo.Name) as IHoldPlace;

                if (placeholder != null)
                {

                    placeholder.Forget();
                }
            }

            var forgetMethods = AttributeHelper.GetForgets(WindowInstance.GetType());

            foreach (MethodInfo methodInfo in forgetMethods)
            {

                methodInfo.Invoke(WindowInstance, null);

            }
        }
    }
}
