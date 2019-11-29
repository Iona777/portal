using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using NewQuoteTool.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewQuoteTool.Steps
{
    [Binding]
    public class ElectricityPricesSteps
    {
        private readonly LandingPage _theLandingPage;
        private readonly YourBusinessDetailsPage _theYourBusinessDetailsPage;
        private readonly ElectricityPackagesPage _theElectricityPackagesPage;

        public ElectricityPricesSteps()
        {
            _theLandingPage = new LandingPage();
            _theYourBusinessDetailsPage = new YourBusinessDetailsPage();
            _theElectricityPackagesPage = new ElectricityPackagesPage();
        }

        [Given(@"I have selected electricity and kWh usage")]
        public void GivenIHaveSelectedElectricityAndKWhUsage()
        {
            _theLandingPage.ClickOnElectricityPricesRadioButton();
            _theLandingPage.ClickOnElectricityKWhUsageRadioButton();      
        }

        [When(@"I click on Electricity Next button")]
        public void WhenIClickOnElectricityNextButton()
        {
            _theLandingPage.ClickOnElectricityNextButton();
        }
        
        [Then(@"the your details screen for Electricity is displayed")]
        public void ThenTheYourDetailsScreenForElectricityIsDisplayed()
        {
            Assert.IsTrue(_theYourBusinessDetailsPage.PageUrl().EndsWith("quote-electricity"));
            Assert.IsTrue(_theYourBusinessDetailsPage.IsPageDisplayed());
        }
        
        [Given(@"I have entered ""(.*)"" value for a frequency of ""(.*)"" for Electricity")]
        public void GivenIHaveEnteredValueForAFrequencyOfForElectricity(string consumption, string frequency)
        {
            _theLandingPage.EnterElectricityConsumptionInKwh(consumption);
            _theLandingPage.SelectElectricFrequency(frequency);
        }

        [When(@"I enter postcode ""(.*)"" with DNO value of ""(.*)""")]
        public void WhenIEnterPostcodeWithDNOValueOf(string postcode, string DNO)
        {
            _theYourBusinessDetailsPage.EnterPostCode(postcode);
            _theYourBusinessDetailsPage.ClickOnFindAddressButton();
        }

        [Then(@"the Your Electricity Packages micro prices screen is displayed")]
        public void ThenTheYourElectricityPackagesMicroPricesScreenIsDisplayed()
        {
            Assert.IsTrue(_theElectricityPackagesPage.IsPageDisplayed(), "Electricity Packages Page is not displayed");
        }

        [Then(@"the Estimated Annual Cost for Electricity is calculated correctly for ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" using matrix file ""(.*)""")]
        public void ThenTheEstimatedAnnualCostForElectricityIsCalculatedCorrectlyForAndAndAndUsingMatrixFile(string usage, string duration, string frequency, string DNO, string matrixFile)
        {
            _theElectricityPackagesPage.CalculateAnnualCost(usage, duration, frequency, DNO, matrixFile);
            Assert.IsTrue(_theElectricityPackagesPage.MonthlyCostMatchesExpected());
            Assert.IsTrue(_theElectricityPackagesPage.AnnualCostMatchesExpected());
        }
    }
}
