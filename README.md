# FlatGlass
Sane Windows Test Automation

FlatGlass is an attempt to separate the concerns of location of automation element and what is done with the element found. 

It is automation framework agnostic and works using simple locators on attributes similar in nature to the SelementPageFactory.

The Window Object Model is a direct counterpart of the Page Object Model found in web automation testing. 
POM's are where pages are represented by classes which implement methods used to fill forms etc.

In the WOM an applications windows are mapped to classes and methods are used to trigger button presses, check box's etc.

A WOM is not a test but a layer used between testing frameworks and the application under tests GUI.

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
'''

FlatGlass is aimed at reducing the effort required to produce and maintain a Window Object Model for an application.
