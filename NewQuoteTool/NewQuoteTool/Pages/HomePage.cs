using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace NewQuoteTool.Pages
{
    public class HomePage: BasePage
    {
        //Constructor
        //It will just use the one from BasePage unless it needs something extra
       
        //Element Locators
        private readonly By _acceptCookiesButtonLocator = By.LinkText("Got it!");
        private readonly By _getAQuoteButtonLocator = By.LinkText("Get a quote");

        //Methods
        public void ClickGetAQuoteButton()
        {
            //If cookies already accepted then this will not be visible. 
            if (IsElementVisible(_acceptCookiesButtonLocator,waitSeconds:2))
                ClickOnElement(_acceptCookiesButtonLocator);
                  
            ClickOnElement(_getAQuoteButtonLocator);
        }

    }
}
