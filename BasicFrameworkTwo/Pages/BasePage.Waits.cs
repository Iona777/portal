using OpenQA.Selenium;
using System;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Configuration;
using BasicFrameworkTwo.Utilities;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {
        #region Waits

        //The default timout period for webdriver waits taken from AppSettings and stored here in a property.
        public int WebDriverTimeout => Convert.ToInt32(ConfigurationManager.AppSettings["DefaultTimeoutSeconds"]);

        /// <summary>
        /// Waits for the given element to be clickable. Used when element has already
        /// been found, but may not be clickable yet.
        /// </summary>
        /// <param name="element">Element you are waiting for</param>
        /// <param name="timeoutSeconds">Timeout in seconds</param>
        /// <returns>IWebElement</returns>
        protected IWebElement WaitForElementToBeClickable(IWebElement element, int timeoutSeconds = 20)
        {
            WebDriverWait waitToBeClickable = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(timeoutSeconds));
            return waitToBeClickable.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        /// <summary>
        /// Waits for a custom condition to be fulfilled.
        /// </summary>
        /// <param name="by">Used to locate the element to be waited for</param>
        /// <param name="condition">The condition that must be fulfilled, e.g. x => x.Text.Contains("Send")</param>
        /// <param name="waitSeconds">Timeout in seconds </param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitForVisibility">Whether to wait for visiblity or just presence, defaults to FALSE</param>
        /// <returns>IWebElement</returns>
        protected IWebElement WaitForWithCondition(By by, Func<IWebElement, bool> condition, int waitSeconds = 20,
            bool retry = true, bool waitForVisibility = false)
        {
            try
            {
                var wait = new WebDriverWait(baseDriver, new TimeSpan(0, 0, waitSeconds));
                Func<IWebDriver, IWebElement> waitForElement = x =>
                {
                    var elements = WaitForElements(by, waitSeconds, retry, waitForVisibility);
                    var element = elements.FirstOrDefault(e => e != null && condition(e) && e.Displayed);
                    return element;
                };
                return wait.Until(waitForElement);
            }
            catch (Exception e)
            {
                if (retry)
                {
                    try
                    {
                        WaitForWithCondition(by, condition, waitSeconds, false); //only retry once
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error                
                        throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                }
            }
            return null;
        }

        /// <summary>
        /// Function to wait check if an element contains specified text
        /// </summary>
        private Func<IWebElement, string, bool> waitForText = new Func<IWebElement, string, bool>((IWebElement element, string textToMatch) =>
        {
            if (element.Text.Contains(textToMatch))
            {
                return true;
            }

            return false;
        });

        /// <summary>
        /// Function to check if a list contains required number of elements
        /// </summary>
        private Func<IReadOnlyCollection<IWebElement>, int, bool> waitForListElements = new Func<IReadOnlyCollection<IWebElement>, int, bool>(
            (IReadOnlyCollection<IWebElement> elementList, int minCount) =>
            {
                if (elementList.Count >= minCount)
                {
                    return true;
                }

                return false;
            });


        /// <summary>
        /// Function to check if a list contains required number of elements
        /// </summary>
        private Func<IWebDriver, By, int, bool> waitForListElements2 = new Func<IWebDriver, By, int, bool>(
            (IWebDriver driver, By by, int minCount) =>
            {
                var elementList = driver.FindElements(by);

                if (elementList.Count >= minCount)
                {
                    return true;
                }

                return false;
            });


        /// <summary>
        /// Waits for ALL the element specified via the given By to be present/visible
        /// Depending on whether or not waitForVisibility = TRUE
        /// NOTE: This seems to be getting used incorrectly in places where only 1 element is expected.
        /// </summary>
        /// <param name="by">Used to locate the element to be waited for</param>
        /// <param name="waitSeconds">Timeout</param>
        /// <param name="retry">Whether to retry, defaults to TRUE</param>
        /// <param name="waitForVisibility">Whether to wait for visibility or just presence, defaults to FALSE</param>
        /// <returns>List of web elements</returns>
        protected IReadOnlyCollection<IWebElement> WaitForElements(By by, int? waitSeconds = null, bool retry = true, bool waitForVisibility = false)
        {
            int seconds = waitSeconds ?? WebDriverTimeout;
            var wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            try
            {
                if (waitForVisibility)
                {
                    return wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
                }
                return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }
            catch (Exception e)
            {
                if (retry)
                {
                    try
                    {
                        WaitForElements(by, seconds, retry: false); //Call itself, but don't do another retry
                    }
                    catch (Exception)
                    {
                        //failed again, throw original error                
                        throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                    }
                }
                else
                {
                    throw new WebDriverTimeoutException($"Cannot find element via: " + by + " Failing method = " + UtilityMethods.GetCurrentMethod(), e);
                }

            }
            return null;
        }
        #endregion
    }
}
