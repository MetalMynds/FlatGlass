// -----------------------------------------------------------------------
// <copyright file="TestWindow.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass.UnitTests.ViewModels.Wpf
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

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petFoodInput", Scope: Scope.ChildrenOnly, ControlType: "ComboBox")]
        private PlaceHolder<ComboBox> eats;

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petPriceInput", Scope: Scope.ChildrenOnly, ControlType: "Edit")]
        private PlaceHolder<TextBox> price;

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petRulesInput", Scope: Scope.ChildrenOnly, ControlType: "List")]
        private PlaceHolder<ListView> rules;

        [FindBy(Parent: "Registration Tab")]
        [FindBy(1, How: How.AutomationId, Using: "petSaveButton", Scope: Scope.ChildrenOnly, ControlType: "Button")]
        private PlaceHolder<Button> save;

        public PetShopMainWindowWpf(AutomationElement root)
        {            
        }
       
        public void RegisterAnimal(String name, String type, String eats, double registrationPrice, String[] rules)
        {

            this.registration.Control.Select();

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
            this.history.Control.Select();
        }
    }
}
