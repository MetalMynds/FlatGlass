// -----------------------------------------------------------------------
// <copyright file="ControlHolder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Automation;

using MetalMynds.Utilities.Puppeteer;

namespace MetalMynds.FlatGlass
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    /// 
    public class PlaceHolder<T> : IHoldPlace
    {
        private readonly bool _cache;
        private readonly String _fieldPropertyName;
        private readonly List<Locator> _locatorsList;
        private readonly AutomationElement _parent;
        private readonly String _wellKnownAs;
        private Object _control;
        private List<IHoldPlace> _windowPlaceHolderList;
        private TimeSpan _implitictWaitTimeout = new TimeSpan(0, 1, 0);
        private AutomationElement _element;
        private Object _window;

        public PlaceHolder(List<IHoldPlace> windowPlaceHolderList, String fieldPropertyName, AutomationElement parent,
                           List<Locator> locators, Object window, bool cache, String wellKnownName = null)
        {

            windowPlaceHolderList.Add(this);

            _windowPlaceHolderList = windowPlaceHolderList;
            _fieldPropertyName = fieldPropertyName;
            _parent = parent;
            _locatorsList = locators;
            _window = window;
            _cache = cache;
            _wellKnownAs = wellKnownName;
        }

        public TimeSpan ImplicitTimeout
        {
            get { return _implitictWaitTimeout; }
            set { _implitictWaitTimeout = value; }
        }

        public List<IHoldPlace> WindowPlaceHolderList
        {
            get { return _windowPlaceHolderList; }
        }

        protected virtual AutomationElement GetElement()
        {
            // Execute Chained Look Up of Locators

            AutomationElement childElement = null;

            if (_locatorsList.Count == 0)
            {
                throw new FindByAttributeLocatorsNotFoundException(_fieldPropertyName);
            }

            AutomationElement parentElement = _parent;

            int count = 0;

            var fullLocatorList = new List<Locator>();

            ExpandLocators(_windowPlaceHolderList, ref fullLocatorList, _locatorsList);

            Locator previousLocator = fullLocatorList[0];

            foreach (Locator locator in fullLocatorList)
            {
                count++;

                TreeScope searchScope;

                switch (locator.Scope)
                {
                    case Scope.ChildrenOnly:
                        searchScope = TreeScope.Children;
                        break;
                    case Scope.Descendants:
                        searchScope = TreeScope.Descendants;
                        break;
                    case Scope.Self:
                        searchScope = TreeScope.Element;
                        break;
                    case Scope.All:
                        searchScope = TreeScope.Subtree;
                        break;
                    default:
                        searchScope = TreeScope.Subtree;
                        break;
                }

                childElement = null;

                DateTime started = DateTime.Now;

                while (DateTime.Now.Subtract(started).CompareTo(_implitictWaitTimeout) < 0 && childElement == null)
                {
                    // No Delay Here (Want the element to be found as soon as its in the tree)
                    childElement = parentElement.FindFirst(searchScope, locator.Condition);
                }

                if (childElement == null)
                {
                    String previous;

                    if (count > 1)
                    {
                        previous = previousLocator.Description;
                    }
                    else
                    {
                        previous = "None";
                    }

                    String message =
                        String.Format(
                            "Break in Location Chain!\nField/Property: {0}\nFind By Position: {1} of {2}\n{3}\nPrevious:\n{4}",
                            _fieldPropertyName, count, fullLocatorList.Count, locator.Description,
                            previous);

                    throw new BreakInChainedLocationException(message);
                }

                previousLocator = locator;

                parentElement = childElement;
                
            }

            return childElement;
        }

        public T Control
        {
            get
            {
                if (_control == null)
                {

                    if (WindowFactory.ControlConstructor == null)
                    {
                        throw new WindowFactoryControlConstructorDelegateNullException();
                    }

                    Type heldControlType = typeof(T);

                    try
                    {
                        _control = WindowFactory.ControlConstructor.Invoke(heldControlType, this.Element,
                                                                          Guid.NewGuid().ToString("N"));
                    }
                    catch (Exception ex)
                    {
                        String message = String.Format("Failed to Create Control!\nClass: [{0}]\nError:\n{1}",
                                                       heldControlType.FullName, ex.Message);

                        throw new CreatePlaceHeldControlFailedException(message, ex);
                    }
                }

                return (T)_control;
            }
        }

        public Boolean IsWellKnown
        {
            get { return !String.IsNullOrWhiteSpace(_wellKnownAs); }
        }

        public String WellKnownAs
        {
            get { return _wellKnownAs; }
        }

        public List<Locator> LocatorList
        {
            get { return _locatorsList; }
        }

        public Type HeldType
        {
            get { return typeof(T); }
        }

        protected void ExpandLocators(List<IHoldPlace> WindowHolderList, ref List<Locator> FullList,
                                      List<Locator> SourceList)
        {
            foreach (Locator locator in SourceList)
            {
                if (locator.GetType() == typeof(WellKnownReferenceLocator))
                {
                    var knownLocator = locator as WellKnownReferenceLocator;

                    if (knownLocator != null)
                    {

                        bool foundReferencedPlaceHolder = false;
                        IHoldPlace wellKnownPlaceHolder = null;

                        foreach (IHoldPlace placeHolder in WindowHolderList)
                        {
                            if (placeHolder.IsWellKnown && placeHolder.WellKnownAs == knownLocator.WellKnownName)
                            {
                                foundReferencedPlaceHolder = true;
                                wellKnownPlaceHolder = placeHolder;
                                break;
                            }
                        }

                        if (foundReferencedPlaceHolder)
                        {
                            ExpandLocators(WindowHolderList, ref FullList, wellKnownPlaceHolder.LocatorList);
                        }
                        else
                        {
                            throw new ReferencedWellKnownPlaceHolderNotFoundException(_fieldPropertyName,
                                                                                      knownLocator.WellKnownName);
                        }
                    }
                }
                else
                {
                    FullList.Add(locator);
                }
            }
        }

        public virtual Boolean ShouldCache
        {
            get { return _cache; }
        }

        public Object GetControl()
        {
            return this.Control;
        }

        public void Forget()
        {
            _element = null;
            _control = null;
        }

        public AutomationElement Element
        {
            get
            {
                if (_element == null)
                {

                    _element = GetElement();

                }

                return _element;
            }
        }

        public Object Window
        {
            get
            {
                return _window;
            }
        }
    }

    public class BreakInChainedLocationException : Exception
    {
        public BreakInChainedLocationException(String Message)
            : base(Message)
        {
        }
    }

    public class CreatePlaceHeldControlFailedException : Exception
    {
        public CreatePlaceHeldControlFailedException(String Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }
    }

    public class CreatePlaceHolderFailedException : Exception
    {
        public CreatePlaceHolderFailedException(String Message)
            : base(Message)
        {
        }

        public CreatePlaceHolderFailedException(String Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }
    }

    public class WindowFactoryControlConstructorDelegateNullException : Exception
    {
        public WindowFactoryControlConstructorDelegateNullException()
            : base("Control Constructor Delegate Property on WindowFactory Has Not Been Set!")
        {
        }
    }

    public class FindByAttributeLocatorsNotFoundException : Exception
    {
        public FindByAttributeLocatorsNotFoundException(String fieldPropertyName)
            : base(String.Format("No FindBy Attributes Found On PlaceHolder!\nField/Property: {0}", fieldPropertyName))
        {
        }
    }

    public class ReferencedWellKnownPlaceHolderNotFoundException : Exception
    {
        public ReferencedWellKnownPlaceHolderNotFoundException(String fieldPropertyName, String wellKnownName)
            : base(
                String.Format(
                    "No WellKnownAs PlaceHolder Found In WindowPlaceHolderList!\nWell Known Name: [{0}]\nField/Property: {1}",
                    wellKnownName, fieldPropertyName))
        {
        }
    }

    //public class PlaceHolder
    //{
    //    public static Object GetControl(Object PlaceHolder)
    //    {

    //        if (PlaceHolder == null)
    //        {
    //            throw new ArgumentNullException("Call to PlaceHolder GetControl Failed! Placeholder Parameter is Null!");
    //        }

    //        Glove controlGlove = new Glove(PlaceHolder);

    //        if (controlGlove.HasProperty("Control"))
    //        {
    //            return controlGlove.GetProperty("Control");
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    public class PlaceHolder
    {

        public static PlaceHolder<T> Create<T>(AutomationElement Parent, List<IHoldPlace> WellKnownList, params FindByAttribute[] FindBy)
        {
            return Create<T>(Parent, null, WellKnownList, FindBy);
        }

        public static PlaceHolder<T> Create<T>(AutomationElement Parent, Object Window, List<IHoldPlace> WellKnownList, params FindByAttribute[] FindBy)
        {

            List<Locator> locators = new List<Locator>();

            foreach (FindByAttribute findBy in FindBy)
            {
                locators.Add(findBy.Locator);
            }

            var placeHolder = new PlaceHolder<T>(
                                    WellKnownList == null ? new List<IHoldPlace>() : WellKnownList,
                                    "",
                                    Parent,
                                    locators,
                                    Window,
                                    false);
            return placeHolder;
        }

        
    }
    
}
