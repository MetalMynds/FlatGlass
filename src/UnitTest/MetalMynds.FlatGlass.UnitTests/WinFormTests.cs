using System;
using System.Collections.Generic;

using MetalMynds.Utilities;

using TestStack.White;
using TestStack.White.UIItems.ListBoxItems;

using WiPFlash;
using WiPFlash.Framework;
using Application = WiPFlash.Components.Application;
using WiPFlash.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetalMynds.FlatGlass.UnitTests.ViewModels.WinForms;
using TestStack.White.UIItems.Actions;

namespace MetalMynds.FlatGlass.UnitTests
{
    [TestClass]
    public class WinFormTests
    {
        ApplicationLauncher launcher;
        Application application;

        [TestInitialize]
        public void SetUp()
        {
            launcher = new ApplicationLauncher(TimeSpan.Parse("00:00:12"));

            application = launcher.LaunchOrRecycle("Example.PetShop.WinForms", @"Examples\WinForms\Example.PetShop.WinForms.exe", null);

            WindowFactory.ControlConstructor = new WindowFactory.ConstructFromElement((controlType, element, name) =>
            {

                if (controlType.Namespace.Contains("WiPFlash"))
                {

                    Object[] parameters = new Object[2];

                    parameters[0] = element;
                    parameters[1] = name;

                    return ReflectionHelper.Instantiate(controlType.Assembly, controlType.FullName, parameters);

                }
                else if (controlType.Namespace.Contains("TestStack.White"))
                {

                    Object[] parameters = new Object[2];

                    parameters[0] = element;
                    parameters[1] = new TestStack.White.UIItems.Actions.NullActionListener();

                    return ReflectionHelper.Instantiate(controlType.Assembly, controlType.FullName, parameters);

                }

                throw new CreatePlaceHeldControlFailedException(String.Format("Control Namespace Not Recognised!\nNamespace: {0}", controlType.Namespace), null);

            });

        }

        [TestMethod]
        public void PetShopWindowWinFormsGuiTest()
        {

            //try
            //{

                Window window = application.FindWindow(FindBy.UiAutomationId("FormMain"));

                var petshopWindow = WindowFactory.Create<PetShopMainWindowWinForm>(window.Element);

                ////var history = petshopWindow.historyTab.Control;

                ////history.Select();

                //var container = petshopWindow.container.Control;

                //var tabControl = petshopWindow.tabctrlAdmin.Control;

                ////var panel = petshopWindow.registrationTabContainer.Control;

                //var text = petshopWindow.name.Control;

                //text.Text = "Dave is legendery!";

                List<String> rules = new List<string>();

                rules.Add("Special Environment");

                petshopWindow.RegisterAnimal("Foghorn Leghorn", "Large Bird", "Herbivorous", 69.68, rules.ToArray());

                petshopWindow.ShowHistory();

                petshopWindow.RegisterAnimal("Chickin Lic'in", "Small Bird", "Herbivorous", 666.99, rules.ToArray());

                petshopWindow.ShowHistory();

                rules.Clear();

                rules.Add("Dangerous");

                rules.Add("Sell In Pairs");

                petshopWindow.RegisterAnimal("Capistrano", "Cat", "Carnivorous", 9.99, rules.ToArray());

                petshopWindow.ShowHistory();

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
    }

}
