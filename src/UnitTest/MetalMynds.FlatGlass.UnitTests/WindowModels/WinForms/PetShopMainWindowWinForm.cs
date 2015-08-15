// -----------------------------------------------------------------------
// <copyright file="TestWindow.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass.UnitTests.ViewModels.WinForms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Automation;

    using WiPFlash.Components;
    using TestStack.White;
    using TestStack.White.UIItems.ListBoxItems;

    using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;
    using ListBox = TestStack.White.UIItems.ListBoxItems.ListBox;
    //using TextBox = TestStack.White.UIItems.TextBox;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>

    public class PetShopMainWindowWinForm
    {


        [FindBy(1, How: How.AutomationId, Using: "tolstrpcntMain", Scope: Scope.Descendants)]
        [WellKnownAs("Container")]
        public PlaceHolder<Panel> container;

        [FindBy(Parent: "Container")]
        [FindBy(1, How.AutomationId, Using: "tabctrlAdmin", Scope: Scope.Descendants, ControlType: "Tab")]
        [WellKnownAs("Registration History Tabs")]
        public PlaceHolder<Tab> tabctrlAdmin;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How.AutomationName, Using: "History", Scope: Scope.ChildrenOnly, ControlType: "TabItem")]
        [WellKnownAs("History Tab")]
        public PlaceHolder<Tab> historyTab;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How.AutomationName, Using: "Registration", Scope: Scope.ChildrenOnly, ControlType: "TabItem")]
        [WellKnownAs("Registration Tab")]
        private PlaceHolder<Tab> registrationTab;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How.AutomationId, Using: "tabpgRegistration", Scope: Scope.All, ControlType: "Pane")]
        [WellKnownAs("Registration Pane")]
        public PlaceHolder<Panel> registrationTabContainer;

        //[FindBy(Parent: "Registration History Tabs")]
        //[FindBy(1, How.AndProperty, Scope.ChildrenOnly, "Name=Registration", "AutomationId=tabpgRegistration")]
        //[WellKnownAs("Registration Pane")]
        //public PlaceHolder<Panel> registrationTabContainer;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "txtName", Scope: Scope.Descendants, ControlType: "Edit")]
        public PlaceHolder<TextBox> name;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "cmbType", Scope: Scope.Descendants, ControlType: "ComboBox")]
        private PlaceHolder<EditableComboBox> type;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "cmbEats", Scope: Scope.Descendants, ControlType: "ComboBox")]
        private PlaceHolder<ComboBox> eats;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "txtPrice", Scope: Scope.Descendants, ControlType: "Edit")]
        private PlaceHolder<TextBox> price;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "lstRules", Scope: Scope.Descendants, ControlType: "List")]
        private PlaceHolder<ListView> rules;

        [FindBy(Parent: "Registration History Tabs")]
        [FindBy(1, How: How.AutomationId, Using: "butSave", Scope: Scope.Descendants, ControlType: "Button")]
        private PlaceHolder<Button> save;

        public PetShopMainWindowWinForm(AutomationElement root)
        {
        }

        public void RegisterAnimal(String name, String type, String eats, double registrationPrice, String[] rules)
        {

            this.registrationTab.Control.Select();

            this.name.Control.Text = name;

            var items = this.type.Control.Items;

            this.type.Control.Text = String.Format("{0}", type);

            this.eats.Control.SetValue((String.Format("{0}", eats)));

            this.price.Control.Text = String.Format("{0:0.00}", registrationPrice);

            this.rules.Control.ClearSelection();

            this.rules.Control.Select(rules);

            this.rules.Control.EnsureVisible();

            this.save.Control.Click();
        }


        public void ShowHistory()
        {
            this.historyTab.Control.Select();
        }
    }
}
