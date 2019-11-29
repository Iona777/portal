using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using NewQuoteTool.Pages;
using NewQuoteTool.Utilities;
    

//namespace for your page objects

namespace NewQuoteTool.Steps
{
    [Binding]
    public class GasPricesSteps
    {
        private readonly HomePage _theHomePage;
        private readonly LandingPage _theLandingPage;
        private readonly GasPackagesPage _theGasPackagesPage;
        private readonly YourBusinessDetailsPage _theYourBusinessDetailsPage;
        
        public GasPricesSteps()
        {
            _theHomePage = new HomePage();
            _theLandingPage = new LandingPage();
            _theGasPackagesPage = new GasPackagesPage();
            _theYourBusinessDetailsPage = new YourBusinessDetailsPage();          
        }

        [Given(@"I have navigated to the Quote Tool Landing page")]
        public void GivenIHaveNavigatedToTheQuoteToolLandingPage()
        {
            Driver.NavigateTo(Driver.RootURl);
           _theHomePage.ClickGetAQuoteButton();

            Assert.IsTrue(_theLandingPage.PageUrl().Contains("get-a-quote"));
            Assert.IsTrue(_theLandingPage.AreLandingPageButtonsDisplayed());
        }

        [Given(@"I have selected gas and kWh usage")]
        public void GivenIHaveSelectedGasAndKWhUsage()
        {
            _theLandingPage.ClickOnGasPricesRadioButton();
            _theLandingPage.ClickOnGasKWhUsageRadioButton();
        }

        [Given(@"I have entered ""(.*)"" value for a frequency of ""(.*)"" for Gas")]
        public void GivenIHaveEnteredValueForAFrequencyOfForGas(string consumption, string frequency)
        {
            _theLandingPage.EnterGasConsumptionInKwh(consumption);
            _theLandingPage.SelectGasFrequency(frequency);
        }

        [When(@"I click on Gas Next button")]
        public void WhenIClickOnGasNextButton()
        {
            _theLandingPage.ClickOnGasNextButton();
        }
        
        [Then(@"the Estimated Annual Cost for Gas is calculated correctly for ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" using matrix file ""(.*)""")]
        [When(@"the Estimated Annual Cost for Gas is calculated correctly for ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" using matrix file ""(.*)""")]
        public void ThenTheEstimatedAnnualCostForGasIsCalculatedCorrectlyForAndAndAndAndUsingMatrixFile(string usage, string duration, string band, string frequency, string LDZ, string matrixFile)
        {
            _theGasPackagesPage.CalculateAnnualCost(usage, duration, band, frequency, LDZ, matrixFile);
            Assert.IsTrue(_theGasPackagesPage.MonthlyCostMatchesExpected());
            Assert.IsTrue(_theGasPackagesPage.AnnualCostMatchesExpected());
        }
        
        [When(@"the Estimated Annual Cost is calculated correctly for ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" using matrix file ""(.*)""")]
        [Then(@"the Estimated Annual Cost is calculated correctly for ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" and ""(.*)"" using matrix file ""(.*)""")]
        public void WhenTheEstimatedAnnualCostIsCalculatedCorrectlyForAndAndAndAndUsingMatrixFile(string usage, string duration, string band, string frequency, string LDZ, string matrixFile)
        {
            _theGasPackagesPage.CalculateAnnualCost(usage, duration, band,frequency, LDZ, matrixFile);
            Assert.IsTrue(_theGasPackagesPage.MonthlyCostMatchesExpected());
            Assert.IsTrue(_theGasPackagesPage.AnnualCostMatchesExpected());
        }

        [Then(@"the your details screen for Gas is displayed")]
        public void ThenTheYourDetailsScreenForGasIsDisplayed()
        {
            Assert.IsTrue(_theYourBusinessDetailsPage.PageUrl().EndsWith("quote-gas"));
            Assert.IsTrue(_theYourBusinessDetailsPage.IsPageDisplayed());
        }
        
        [When(@"I enter postcode ""(.*)"" with LDZ value of ""(.*)""")]
        public void WhenIEnterPostcodeWithLDZValueOf(string postcode, string LDZ)
        {
           _theYourBusinessDetailsPage.EnterPostCode(postcode);
           _theYourBusinessDetailsPage.ClickOnFindAddressButton(); 
        }
        
        [When(@"I select the first address on the Select Address screen")]
        public void WhenISelectTheFirstAddressOnTheSelectAddressScreen()
        {
            _theYourBusinessDetailsPage.SwitchWindowFirstFrame();
            _theYourBusinessDetailsPage.ClickOnFirstAddressOnList();
        }

        [When(@"I click on View Prices button")]
        public void WhenIClickOnViewPricesButton()
        {
            _theYourBusinessDetailsPage.ClickOnViewPricesButton();
        }

        [Then(@"the Your Gas Packages micro prices screen is displayed")]
        public void ThenTheYourGasPackagesMicroPricesScreenIsDisplayed()
        {
            Assert.IsTrue(_theGasPackagesPage.IsPageDisplayed(),"Gas Packages Page is not displayed");
        }
    }
}
