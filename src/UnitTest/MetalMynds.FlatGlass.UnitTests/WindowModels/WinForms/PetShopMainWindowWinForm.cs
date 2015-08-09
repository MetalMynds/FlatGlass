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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>

    public class PetShopMainWindowWinForm
    {

        
        [FindBy(1, How: How.AutomationId, Using: "tolstrpcntMain", Scope: Scope.Descendants)]
        [WellKnownAs("Container")]
        private PlaceHolder<Object> container;

        [FindBy(Parent: "Container")]
        [FindBy(1, How.AutomationId, Scope.Descendants, "Name=tabpgeBasket")]
        [WellKnownAs("History Tab")]
        private PlaceHolder<Tab> historyTab;

        [WellKnownAs("Registration Tab")]
        [FindBy(Parent: "Container")]
        [FindBy(1, How.AutomationId, Scope.Descendants, "Name=tabpgRegistration")]
        private PlaceHolder<Tab> registrationTab;
     
        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "txtName", Scope: Scope.ChildrenOnly, ControlType: "Edit")]
        private PlaceHolder<TextBox> name;

        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "cmbType", Scope: Scope.ChildrenOnly, ControlType: "ComboBox")]
        private PlaceHolder<EditableComboBox> type;

        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "cmbEats", Scope: Scope.ChildrenOnly, ControlType: "ComboBox")]
        private PlaceHolder<ComboBox> eats;

        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "lblPrice", Scope: Scope.ChildrenOnly, ControlType: "Edit")]
        private PlaceHolder<TextBox> price;

        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "lstRules", Scope: Scope.ChildrenOnly, ControlType: "List")]
        private PlaceHolder<ListView> rules;

        [FindBy(Parent: "Registration Tab Pane")]
        [FindBy(1, How: How.AutomationId, Using: "butSave", Scope: Scope.ChildrenOnly, ControlType: "Button")]
        private PlaceHolder<Button> save;

        public PetShopMainWindowWinForm(AutomationElement root)
        {
        }

        public void RegisterAnimal(String name, String type, String eats, double registrationPrice, String[] rules)
        {

            this.registrationTab.Control.Select();

            this.name.Control.Text = name;

            this.type.Control.Select(String.Format("PetType[{0}]", type));

            this.eats.Control.Select(String.Format("PetFood[{0}]", eats));

            this.price.Control.Text = String.Format("{0:0.00}", registrationPrice);

            this.rules.Control.ClearSelection();

            foreach (String rule in rules)
            {
                this.rules.Control.Select(String.Format("Rule[{0}]", rule));
            }

            this.rules.Control.EnsureVisible();

            this.save.Control.Click();

        }

        public void ShowHistory()
        {
            this.historyTab.Control.Select();
        }
    }
}
