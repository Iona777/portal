using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; //Need this for WebDriverWait
using BasicFrameworkTwo.Utilities; //Location of Driver class
//So it knows to take ExpectedConditions from here instead of (deprecated) OpenQA.Selenium
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {
        private static IWebDriver baseDriver;
        private static WebDriverWait baseWait;
        private static SelectElement select;

        //Constructor
        public BasePage()
        {
            baseDriver = Driver.driver;
            baseWait = Driver.wait;
        }

        //Methods

        #region GetElements


        /// <summary>
        /// Use for elements that are on DOM but not visible
        /// </summary>
        public IWebElement GetExistingElement(By by, bool retry = true, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));


            try
            {
                return wait.Until(ExpectedConditions.ElementExists(by));
            }
            catch (Exception e)
            {
                if (retry)
                {
                    try
                    {
                        return wait.Until(ExpectedConditions.ElementExists(by));
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Use if the wait requires to use a condition, e.g. wait for a By AND specific visibleText
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz") </param>
        /// <param name="condition">An additional condition that must be fulfilled, e.g. x => x.Text.Contains("Send")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <param name="visibility">Whether or not to wait for element to be visible or just present</param>
        /// <returns>IWebElement</returns>
        protected IWebElement GetElementByCondition(By by, Func<IWebElement, bool> condition, int? waitSeconds = null, bool visibility = false)
        {
            return waitSeconds != null ? WaitForWithCondition(by, condition, waitSeconds.Value, waitForVisibility: visibility) : baseDriver.FindElement(by);
        }

        /// <summary>
        /// Waits for, then returns, a visible element
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetVisibleElement(By by, bool retry = true, int? waitSeconds = null)
        {
            var seconds = waitSeconds ?? WebDriverTimeout;
            var wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (WebDriverTimeoutException e)
            {
                if (retry)
                {

                    try
                    {
                        return wait.Until(ExpectedConditions.ElementIsVisible(by));
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new WebDriverTimeoutException($"Cannot find element via: " + by + "Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Waits for, then returns, all visible element for given By.
        /// Will also wait 2 seconds and try again in number of elements is less than minimum number
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="minCount">Minimum number  of elements you expect to be returned. Defaults to 1</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns></returns>
        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetAllVisibleElements(By by, int minCount = 1, bool retry = true, int? waitSeconds = null)
        {
            Console.WriteLine("In GetAllVisibleElements() method");
            var seconds = waitSeconds ?? WebDriverTimeout;
            var wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));
            ReadOnlyCollection<IWebElement> elementList;

            try
            {
                Console.WriteLine("Min Count = " + minCount);

                elementList = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                if (elementList.Count < minCount)
                {
                    Console.WriteLine("Number or row elements found is" + elementList.Count);
                    Console.WriteLine("Waiting for 10 seconds");

                    Pause(10000);
                    elementList = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                    Console.WriteLine("After Waiting for 10 seconds, number of row elements now" + elementList.Count);

                }

                return elementList;
            }
            catch (Exception e)
            {
                if (retry)
                {
                    try
                    {
                        Console.WriteLine("Exception encountered" + e);
                        Console.WriteLine("In retry");
                        Console.WriteLine("Min Count in retry = " + minCount);

                        elementList = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                        if (elementList.Count < minCount)
                        {
                            Pause(10000);
                            elementList = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
                        }

                        return elementList;
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new Exception($"Cannot find element via: " + by + "Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Waits for, then returns, a visible element that contains the specified visibleText
        /// </summary>
        /// <param name="searchText">Search visibleText used to locate the element</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetVisibleElementByVisibleText(string searchText, bool retry = true, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[contains(text(),'" + searchText + "')]")));
            }
            catch (WebDriverTimeoutException e)
            {
                if (retry)
                {
                    try
                    {
                        return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[contains(text(),'" + searchText + "')]")));
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new WebDriverTimeoutException($"Timeout searching for element containing visibleText: {searchText}. {e.Message}", e);


                    }
                }
                else
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Waits for, then returns, a clickable element
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetClickableElement(By by, bool retry = true, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(by));
            }
            catch (Exception e)
            {
                if (retry)
                {
                    try
                    {
                        return wait.Until(ExpectedConditions.ElementToBeClickable(by));
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Waits for, then returns, a clickable element
        /// </summary>
        /// <param name="visibleText">Text visible on screen used to locate the element</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetClickableElementByVisibleText(string visibleText, bool retry = true, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(FindElementByText(visibleText)));
            }
            catch (WebDriverTimeoutException e)
            {
                if (retry)
                {
                    try
                    {
                        return wait.Until(ExpectedConditions.ElementToBeClickable(FindElementByText(visibleText)));
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error
                        throw new WebDriverTimeoutException($"Timeout searching for element containing visibleText: {visibleText}. {e.Message}", e);
                    }
                }
                else
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Returns an element that contains the specified searchText
        /// </summary>
        /// <param name="searchText">Text visible on screen used to locate the element</param>
        /// <returns>IWebElement</returns>
        public IWebElement FindElementByText(string searchText)
        {
            return baseDriver.FindElement(By.XPath(".//*[contains(text(),'" + searchText + "')]"));
        }

        /// <summary>
        /// Use this to find the nth element in an array of elements, e.g. invoice in a list
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="nIndex">index of element to return</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        protected IWebElement GetElementAt(By by, int nIndex, int? waitSeconds = null)
        {
            if (waitSeconds != null)
            {
                WaitForElements(by, waitSeconds, waitForVisibility: true);
            }
            return baseDriver.FindElements(by)[nIndex];
        }

        /// <summary>
        /// Selects the results from a Select2 style dropdown.
        /// Locator would be something like By.XPath("//*[@id='select2-selectBasket-results']")
        /// </summary>
        /// <param name="resultsElementLocator">Locator for the results list</param>
        /// <param name="searchResultText">Text of the result you want to select.</param>
        /// <param name="retry">Whether or not to retry on failure to find element</param>
        public void ClickOnSearchResultsForSelect2Dropdown(By resultsElementLocator, string searchResultText, bool retry = true)
        {
            try //Sometimes get a stale element error here
            {
                IWebElement searchResults = WaitForWithCondition(
                    resultsElementLocator,
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
                        resultsElementLocator,
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

        #endregion

        #region DoesElementExist and IsElementDisplayed
        /// <summary>
        /// Checks element is present on the DOM of a page.
        /// This does not necessarily mean that the element is visible
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>Boolean</returns>
        protected bool DoesElementExist(By by, int? waitSeconds = null)
        {
            try
            {
                GetExistingElement(by, retry: true, waitSeconds: waitSeconds);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if an element is visible on the page. Visibility means that the element
        /// is not only displayed but also has a height and width that is greater than 0.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>Boolean</returns>
        protected bool IsElementDisplayed(By by, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;

            try
            {
                return GetVisibleElement(by, waitSeconds: seconds).Displayed;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an element is visible and enabled on the page. Visibility means that the element
        /// is not only displayed but also has a height and width that is greater than 0.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>Boolean</returns>
        protected bool IsElementEnabled(By by, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;

            try
            {
                IWebElement element = GetVisibleElement(by, waitSeconds: seconds);
                return element.Enabled;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// Waits for visible element specified by locator, that contains specified visibleText and returns true/false if present or not
        /// This can be used to check that the visibleText in an element has been updated, e.g. a count
        /// </summary>
        /// <param name="locator">Used to locate element</param>
        /// <param name="text">The visibleText to be present in the element found by locator</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>True or false depending on whether element with specified visibleText is found</returns>
        public bool IsElementDisplayedByText(By locator, string text, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.TextToBePresentInElement(baseDriver.FindElement(locator), text));
            }
            catch (Exception e)
            {
                //Could not find an element with specified visibleText.
                return false;
            }
        }

        /// <summary>
        /// Waits for visible element specified by locator, that contains specified visibleText and returns true/false if present or not
        /// Override that takes in an element instead of a By
        /// </summary>
        /// <param name="element">Element to wait for</param>
        /// <param name="textToBePresent">Text to wait to be displayed in the element</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>True or false depending on whether element is displayed with specified text</returns>
        public bool IsElementDisplayedByText(IWebElement element, string textToBePresent, int? waitSeconds = null)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            WebDriverWait wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                return wait.Until(ExpectedConditions.TextToBePresentInElement(element, textToBePresent));
            }
            catch (Exception e)
            {
                //Could not find an element with specified visibleText.
                return false;
            }
        }

        /// <summary>
        /// Waits for visible element specified by locator, that contains specified visibleText and returns true/false if present or not
        /// Use if the wait requires to use a condition, e.g. wait for a By AND specific visibleText
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="condition">An additional condition that must be fulfilled, e.g. x => x.Text.Contains("Send")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        protected bool IsElementDisplayedByCondition(By by, Func<IWebElement, bool> condition, int? waitSeconds = null)
        {
            try
            {
                return GetElementByCondition(by, condition, waitSeconds: waitSeconds, visibility: true).Displayed;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion

    }
}
