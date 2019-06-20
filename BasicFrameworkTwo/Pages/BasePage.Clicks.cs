using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {

        #region Clicks and Key Presses

        /// <summary>
        /// /// Waits for the element specified via the given By to be clickable
        /// and then clicks on it.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <returns>IWebElement</returns>
        protected IWebElement ClickOnElement(By by)
        {
            IWebElement clickableElement = GetClickableElement(by);
            clickableElement.Click();
            return clickableElement;
        }

        /// <summary>
        /// Waits for given element to be clickable then clicks on it.
        /// </summary>
        /// <param name="element">Element to click on</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        protected void ClickOnElement(IWebElement element, int waitSeconds = 20)
        {
            WaitForElementToBeClickable(element, waitSeconds);
            element.Click();
        }

        /// <summary>
        ///  Gets the elements specified via the given By and clicks on nth element found.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="nIndex">Index of nth element</param>
        /// <param name="waitSeconds">Timeout in seconds</param>

        protected void ClickAtIndex(By by, int nIndex, int? waitSeconds = null)
        {
            GetElementAt(by, nIndex, waitSeconds).Click();
        }

        /// <summary>
        /// Waits for and clicks on the Home link
        /// </summary>
        public void ClickHome()
        {
            ClickOnElement(By.XPath("/html/body/nav/div[2]/div[2]/ul/li[2]/a"));
        }

        /// <summary>
        /// When you enter an account number into the search field, the search results
        /// need to be waited for and click on. That is the purpose of this method.
        /// </summary>
        /// <param name="searchResultText">The visibleText displayed in search results box</param>
        /// <param name="retry">Defaults to TRUE, but you can override it</param>
        public void ClickOnAccountSearchResults(string searchResultText, bool retry = true)
        {
            var locator = By.XPath("//*[@id='select2-selectAvailableAccounts-results']/li[1]"); ;

            try //Sometimes get a stale element error here
            {
                IWebElement searchResults = WaitForWithCondition(
                    locator,
                    x => x.Text.Contains(searchResultText), waitSeconds: 5, retry: true, waitForVisibility: true);

                //Just in case it is visible but not clickable
                WaitForElementToBeClickable(searchResults, timeoutSeconds: 5);
                searchResults.Click();
            }
            catch
            {
                if (retry)
                {
                    IWebElement searchResults = WaitForWithCondition(
                        locator,
                        x => x.Text.Contains(searchResultText), waitSeconds: 5, retry: true, waitForVisibility: true);

                    //Just in case it is visible but not clickable
                    WaitForElementToBeClickable(searchResults);
                    searchResults.Click();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// When you enter a string into the search field, the search results
        /// need to be waited for and clicked on. That is the purpose of this method.
        /// </summary>
        /// <param name="locator">How you locate the search results, e.g. By.Id("xyz")</param>
        /// <param name="searchResultText">The visibleText displayed in search results box</param>
        /// <param name="retry">Defaults to TRUE, but you can override it</param>
        public void ClickOnSelect2SearchResults(By locator, string searchResultText, bool retry = true)
        {

            try //Sometimes get a stale element error here
            {
                IWebElement searchResults = WaitForWithCondition(
                    locator,
                    x => x.Text.Contains(searchResultText), waitSeconds: 5, retry: true, waitForVisibility: true);

                //Just in case it is visible but not clickable
                WaitForElementToBeClickable(searchResults, timeoutSeconds: 5);
                searchResults.Click();
            }
            catch
            {
                if (retry)
                {
                    IWebElement searchResults = WaitForWithCondition(
                        locator,
                        x => x.Text.Contains(searchResultText), waitSeconds: 5, retry: true, waitForVisibility: true);

                    //Just in case it is visible but not clickable
                    WaitForElementToBeClickable(searchResults);
                    searchResults.Click();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Waits for the element specified via the given By to be clickable
        /// then presses the Tab key
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        public void PressTabKey(By by)
        {
            var element = GetClickableElement(by);
            element.SendKeys(Keys.Tab);
        }

        #endregion
    }
}
