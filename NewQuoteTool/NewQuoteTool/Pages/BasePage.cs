using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; //Need this for WebDriverWait and SelectElement
using NewQuoteTool.Utilities; //Location of your Driver class
//So it knows to take ExpectedConditions from here instead of (deprecated) OpenQA.Selenium
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;


namespace NewQuoteTool.Pages
{
    public class BasePage
    {
        //You will set the value of baseDriver to value in Driver Class 
        //using the constructor below. This will be inherited by all your pages
        //It is just a bit neater the having to write Driver.driver 
        protected IWebDriver baseDriver;
        private int webDriverTimeout;
        

        public BasePage()
        {
            baseDriver = Driver.driver;
            webDriverTimeout = Driver.GetTimeoutSeconds();
        }

        //Methods
        /// <summary>
        /// Waits for, then returns, a clickable element
        /// </summary>
        /// <param name="elementLocator">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetClickableElement(By elementLocator, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? webDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
        }


        /// <summary>
        /// Waits for, then returns, a visible element
        /// </summary>
        /// <param name="elementLocator">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetVisibleElement(By elementLocator, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? webDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            return wait.Until(ExpectedConditions.ElementIsVisible(elementLocator));
        }


        /// <summary>
        /// Waits for given element to be clickable then clicks on it.
        /// </summary>
        /// <param name="elementLocator">Used to locate the element, e.g. By.Id("xyz")</param>
        public void ClickOnElement(By elementLocator)
        {
            GetClickableElement(elementLocator).Click();
        }

        /// <summary>
        /// Checks if an element is visible on the page. Visibility means that the element
        /// is not only displayed but also has a height and width that is greater than 0.
        /// </summary>
        /// <param name="elementLocator">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <returns>boolean</returns>

        public bool IsElementVisible(By elementLocator, int? waitSeconds=null)
        {
            int seconds = waitSeconds ?? webDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return
                    wait.Until(ExpectedConditions.ElementIsVisible(elementLocator)).Displayed;

            }
            catch (Exception)
            {
                return false; //Element not found, so Displayed is false
            }
            
        }

        /// <summary>
        /// Returns URL to currently displayed page
        /// </summary>
        /// <returns>String</returns>
        public string PageUrl()
        {
            return Driver.driver.Url;
        }

        public void EnterText(By elementLocator, string inputText, int? waitSeconds = null)
        {
            GetClickableElement(elementLocator).SendKeys(inputText);
        }

        /// <summary>
        /// Will select item in normal dropdown by visible text
        /// </summary>
        /// <param name="element">dropdown element</param>
        /// <param name="visibleText">text in dropdown</param>
        public void SelectElementByValue(IWebElement element, string visibleText)
        {
            SelectElement selectDropDown = new SelectElement(element);
            selectDropDown.SelectByText(visibleText);
        }

        /// <summary>
        /// Switches focus to the last frame
        /// </summary>
        public void SwitchWindowLastFrame()
        {
            baseDriver.SwitchTo().Window(baseDriver.WindowHandles.Last());
        }

        /// <summary>
        /// Switches focus to the first frame
        /// </summary>
        public void SwitchWindowFirstFrame()
        {
            baseDriver.SwitchTo().Window(baseDriver.WindowHandles.First());
        }



    }
}
