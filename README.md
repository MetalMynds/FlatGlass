# FlatGlass
Sane Windows Test Automation

FlatGlass is a framework that separates the concern of locating a control from what's done when it's found.

It is automation framework agnostic and works using simple locators on attributes similar in nature to the WebDriver (Selenium 2) PageFactory https://code.google.com/p/selenium/wiki/PageFactory

FlatGlass supports the Window Object Model which is a direct counterpart to the Page Object Model found in web automation testing. 

When using a POM the pages are represented by classes which implement methods used to fill forms and press buttons.

In the WOM an applications windows are mapped to classes and methods are used to trigger button presses, check box's and more.

A WOM is not a test but a layer used between the testing framework and the application under test, it maps the user interface helping to reduce the size of the overall automation codebase.

An example from the UnitTests;

```c#
    public class PetShopMainWindowWpf
    {
        [WellKnownAs("History Tab")]
        [FindBy(1, How.AndProperty, Scope.Descendants, "ClassName=TabItem", "Name=History")]
        private PlaceHolder<Tab> history;

        [WellKnownAs("Registration Tab")]
        [FindBy(1, How.AndProperty, Scope.Descendants, "ClassName=TabItem", "Name=Registration")]
        private PlaceHolder<Tab> registration;

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petNameInput", Scope: Scope.ChildrenOnly, ControlType: "Edit")]
        private PlaceHolder<TextBox> name;

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petTypeInput", Scope: Scope.ChildrenOnly, ControlType: "ComboBox")]
        private PlaceHolder<EditableComboBox> type;
```

FlatGlass is aimed at reducing the effort required to produce and maintain a complete set of WOM's for an application.
