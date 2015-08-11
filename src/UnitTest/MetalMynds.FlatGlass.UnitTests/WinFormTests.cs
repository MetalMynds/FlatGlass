using System;
using System.Collections.Generic;

using MetalMynds.Utilities;

using WiPFlash;
using WiPFlash.Framework;
using WiPFlash.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetalMynds.FlatGlass.UnitTests.ViewModels.WinForms;

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

                Object[] parameters = new Object[2];

                parameters[0] = element;
                parameters[1] = name;

                return ReflectionHelper.Instantiate(controlType.Assembly, controlType.FullName, parameters);

            });

        }
       
        [TestMethod]
        public void PetShopWindowWinFormsGuiTest()
        {

            try
            {

                Window window = application.FindWindow(FindBy.UiAutomationId("FormMain"));

                var petshopWindow = WindowFactory.Create<PetShopMainWindowWinForm>(window.Element);

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

            }
            catch (Exception ex)
            {

            }
        }
    }

}
