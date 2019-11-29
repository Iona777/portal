using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NewQuoteTool.Utilities; //Location of your Driver class


namespace NewQuoteTool.Pages
{
    public class LandingPage : BasePage
    {   
        //Constructor
        //It will just use the one from BasePage unless it needs something extra

        //Web Element Locators
        private readonly By _gasPrices = By.Id("GasOptionButton");
        private readonly By _electricPrices = By.Id("ElectricityOptionButton");
        private readonly By _gasAndElectricPrices = By.Id("GasAndElectricityOptionButton");

        private readonly By _gasKwhRadioButton = By.CssSelector("[name='Gasconsumption'][value='kwh']");
        private readonly By _gasPoundRadioButton = By.CssSelector("[name='Gasconsumption'][value='money']");

        private readonly By _electricKwhRadioButton = By.CssSelector("[name='ElectricQuoteConsumption'][value='kwh']");
        private readonly By _electricPoundRadioButton = By.CssSelector("[name='ElectricQuoteConsumption'][value='money']");

        private readonly By _comboGasKwhRadioButton = By.CssSelector("[name='GasconsumptionCombo'][value='kwh']");
        private readonly By _comboGasPoundRadioButton = By.CssSelector("[name='GasconsumptionCombo'][value='money']");


        private readonly By _gasKwhConsumption = By.Id("GetaQuoteJustGasKWhConsumption");
        private readonly By _gasPoundConsumption = By.Id("GetaQuoteJustGasPriceConsumption");
        private readonly By _electricKwhConsumption = By.Id("GetaQuoteJustElectricityConsumption");
        private readonly By _electricPoundConsumption = By.Id("GetaQuoteJustElectricityPriceConsumption");
        private readonly By _comboGasConsumption = By.Id("GetaQuoteGasKWhConsumption");
        private readonly By _comboElectricConsumption = By.Id("GetaQuoteElectricityConsumption");

        private readonly By _gasFrequency = By.Id("TimePeriod");
        private readonly By _electricFrequency = By.Id("ElectricityTimePeriod");
        private readonly By _comboGasFrequency = By.Id("GasComboTimePeriod");
        private readonly By _comboElectricFrequency = By.Id("ElectricComboTimePeriod");

        private readonly By _nextButton = By.Id("NextStep1");

       
        

        //Methods
        public bool AreLandingPageButtonsDisplayed()
        {
            return
                IsElementVisible(_gasPrices) &&
                IsElementVisible(_electricPrices) &&
                IsElementVisible(_gasAndElectricPrices) &&
                IsElementVisible(_gasKwhRadioButton) &&
                IsElementVisible(_gasPoundRadioButton) &&
                IsElementVisible(_gasKwhConsumption) &&
                IsElementVisible(_gasFrequency) &&
                IsElementVisible(_nextButton);
        }

        public void ClickOnGasPricesRadioButton()
        {
            ClickOnElement(_gasPrices);  
        }

        public void ClickOnElectricityPricesRadioButton()
        {
            ClickOnElement(_electricPrices);
        }
        
        public void ClickOnGasKWhUsageRadioButton()
        {
            ClickOnElement(_gasKwhRadioButton);
        }

        public void ClickOnElectricityKWhUsageRadioButton()
        {
            ClickOnElement(_electricKwhRadioButton);
        }

        public void EnterGasConsumptionInKwh(string consumption)
        {
            EnterText(_gasKwhConsumption,consumption, waitSeconds:2);
        }

        public void EnterElectricityConsumptionInKwh(string consumption)
        {
            EnterText(_electricKwhConsumption, consumption, waitSeconds: 2);
        }

        public void SelectGasFrequency(string frequency)
        {
            var dropdown = GetClickableElement(_gasFrequency, waitSeconds: 2);
            SelectElementByValue(dropdown, frequency);
        }

        public void SelectElectricFrequency(string frequency)
        {
            var dropdown = GetClickableElement(_electricFrequency, waitSeconds: 2);
            SelectElementByValue(dropdown, frequency);
        }

        public void SelectComboGasFrequency(string frequency)
        {
            var dropdown = GetClickableElement(_comboGasFrequency, waitSeconds: 2);
            SelectElementByValue(dropdown, frequency);
        }

        public void SelectComboElectricFrequency(string frequency)
        {
            var dropdown = GetClickableElement(_comboElectricFrequency, waitSeconds: 2);
            SelectElementByValue(dropdown, frequency);
        }

        public void ClickOnNextButton()
        {
            ClickOnElement(_nextButton);
        }

        public void ClickOnGasNextButton()
        {
            var nextButtons = baseDriver.FindElements(By.Id("NextStep1"));
            //There are several buttons all with the same id.
            //The first one is for Gas
            nextButtons[0].Click();
        }

        public void ClickOnElectricityNextButton()
        {
            var nextButtons = baseDriver.FindElements(By.Id("NextStep1"));
            //There are several buttons all with the same id.
            //The second one is for Electricity
            nextButtons[1].Click();
        }
    }
}
