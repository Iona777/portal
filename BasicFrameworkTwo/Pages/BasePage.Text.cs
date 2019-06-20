using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {

        #region Get Text or Enter Text

        /// <summary>
        /// Waits for the webElement identified by the given By to be visible
        /// then returns its visible text.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>Text</returns>
        protected string GetElementText(By by, int waitSeconds = 10)
        {
            return GetVisibleElement(by, waitSeconds: waitSeconds).Text;
        }

        /// <summary>
        /// Waits for the webElement identified by the given By to be visible
        /// then returns the text of its placeholder attribute
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="waitSeconds">Timeout in seconds</param>
        /// <returns>Text</returns>
        protected string GetElementPlaceHolderText(By by, int? waitSeconds = null)
        {
            return GetVisibleElement(by).GetAttribute("placeholder");
        }

        /// <summary>
        /// Waits for the webElement identified by the given By to be visible
        /// then returns its visibleText as an integer so it can used for integer comparison 
        /// </summary>
        /// <param name="byForCountElement">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <returns>Integer</returns>
        public int GetCountValue(By byForCountElement)
        {
            var countElement = GetVisibleElement(byForCountElement);
            return int.Parse(countElement.Text);
        }

        /// <summary>
        /// Gets visibleText of Page Title (or at least the first element with h1 tag)
        /// </summary>
        /// <returns>string</returns>
        public string GetPageTitle()
        {
            return GetElementText(By.TagName("h1"));
        }

        /// <summary>
        /// Gets visibleText of Page Heading (or at least the first element with h2 tag)
        /// </summary>
        /// <returns>string</returns>
        public string GetPageHeading()
        {
            return GetElementText(By.TagName("h2"));
        }

        /// <summary>
        /// Waits for the webElement identified by the given By to be clickable
        /// Enters given visibleText into the element. Has option of clearing field first.
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="value">text to enter into the field</param>
        /// <param name="clearExisting">Whether or not to clear the field</param>
        /// <param name="seconds">Timeout in seconds</param>
        protected void EnterText(By by, string value, bool clearExisting = false, int? seconds = null)
        {
            int? timeoutSeconds = seconds;
            if (seconds is null)
            {
                timeoutSeconds = WebDriverTimeout;
            }

            var element = GetClickableElement(by, waitSeconds: timeoutSeconds);
            if (clearExisting)
            {
                element.Clear();
            }

            element.SendKeys(value);
        }

        /// <summary>
        /// Enters given visibleText into specified element. Has option of clearing field first.
        /// The element will have already been waited for and found using a Get...Element() method
        /// </summary>
        /// <param name="element">Element to enter text into </param>
        /// <param name="value">text to enter into the field</param>
        /// <param name="clearExisting">Whether or not to clear the field</param>
        protected void EnterText(IWebElement element, string value, bool clearExisting = false)
        {
            if (clearExisting)
                element.Clear();

            element.SendKeys(value);
        }

        /// <summary>
        /// Selects a dropdown list element by visible text
        /// The dropdown element will have already been waited for and found using a Get...Element() method
        /// </summary>
        /// <param name="element">The dropdown control</param>
        /// <param name="visibleText">Text by which to identify the list item</param>
        public static void SelectElementByText(IWebElement element, string visibleText)
        {
            select = new SelectElement(element);
            select.SelectByText(visibleText);
        }

        /// <summary>
        /// Selects a dropdown list element by index
        /// The dropdown element will have already been waited for and found using a Get...Element() method
        /// </summary>
        /// <param name="element">The dropdown control</param>
        /// <param name="index">Index by which to identify the list item</param>
        public static void SelectElementByIndex(IWebElement element, int index)
        {
            select = new SelectElement(element);
            select.SelectByIndex(index);
        }

        /// <summary>
        /// Makes a date picker control visible so that you can use send keys on it.
        /// </summary>
        public void MakeDatePickerVisible()
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)baseDriver;
            jse.ExecuteScript("document.getElementById('accessUntil').removeAttribute('readonly', 0);");
        }
        #endregion
    }
}
