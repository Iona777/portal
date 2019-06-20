using System;
using BasicFrameworkTwo.Utilities;
using OpenQA.Selenium;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {
        #region Check for errors and messages       

        /// <summary>
        /// Waits for the error text element located by given By to be visible
        /// then checks to see if its text matches the specified errorText string
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="errorText">Text you are expecting the error message to have</param>
        /// <returns>Boolean</returns>
        public bool CheckForMessageText(By by, string errorText, int? timeoutSeconds = null)
        {
            try
            {
                int seconds = timeoutSeconds ?? WebDriverTimeout;
                IWebElement errorMsg = GetVisibleElement(by, waitSeconds: timeoutSeconds);
                return errorMsg.Text.RemoveSpacesAndNewLines() == errorText.RemoveSpacesAndNewLines();
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Waits for element containing specified text to be visible then checks
        /// that its text matches the specified visibleText
        /// </summary>
        /// <param name="errorText">Text used to locate and element and you expect be text contents of the element</param>
        /// <returns>Boolean</returns>
        public bool CheckForMessageText(string errorText, int? waitSeconds = null)
        {
            try
            {
                IWebElement errorMsg = GetVisibleElementByVisibleText(errorText, waitSeconds: waitSeconds);
                return (errorMsg.Text == errorText);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// /// Waits for the error text element located by given By to be visible
        /// then checks to see if its text contains the specified errorText string
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="text">Text that you expect the element to contain</param>
        /// <returns></returns>
        public bool CheckMessageContainsText(By by, string text)
        {
            try
            {
                IWebElement message = GetVisibleElement(by);
                return message.Text.Contains(text);
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }
        #endregion
    }
}
