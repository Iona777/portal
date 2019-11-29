using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace NewQuoteTool.Pages
{
    public class YourBusinessDetailsPage: BasePage
    {
        //Constructor- inherits from Basepage

        //WebElement locators
        private readonly By _postCodeFieldLocator = By.Id("BusinessPostCode");
        private readonly By _findAddressButtonLocator = By.Id("findAddressId");
        private readonly By _firstAddressInListLocator = By.XPath("//*[@id='contacts']/tbody/tr[1]/td/a");
        private readonly By _viewPricesButtonLocator = By.Id("postCodeSelected");

        //Methods
        public bool IsPageDisplayed()
        {
            return
                IsElementVisible(_postCodeFieldLocator) &&
                IsElementVisible(_findAddressButtonLocator);
        }

        public void EnterPostCode(string postCode)
        {
            EnterText(_postCodeFieldLocator, postCode);
        }

        public void ClickOnFindAddressButton()
        {
            ClickOnElement(_findAddressButtonLocator);
        }

        public void ClickOnFirstAddressOnList()
        {
            ClickOnElement(_firstAddressInListLocator);
        }

        public void ClickOnViewPricesButton()
        {
            ClickOnElement(_viewPricesButtonLocator);
        }
    }
}
