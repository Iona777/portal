using OpenQA.Selenium;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {

        #region Get Table Rows and Columns

        /// <summary>
        /// Waits for table webElement using the given By.
        /// Returns the row at index nRow 
        /// NOTE: This is zero referenced (although for some tables, row 0 is the headings).
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="nRow">Index of the row to return</param>
        /// <returns>IWebElement</returns>
        public IWebElement GetTableRow(By by, int nRow)
        {
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.AreEqual("table", tableElement.TagName, $"Element given by: {by} is not a table!");

            var webElements = GetAllVisibleElements(By.TagName("tr"), minCount: 2, waitSeconds: 2);

            try
            {
                Assert.IsNotNull(webElements.ElementAt(nRow), $"Didn't find any data at row index: {nRow}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("number of elements found = " + webElements.Count);
                Console.WriteLine("content of nRow index of webElements = " + webElements.ElementAt(nRow));
                Console.WriteLine(e);
                throw;
            }
            return webElements[nRow];
        }

        /// <summary>
        /// Waits for table webElement using the given By.
        /// Reverses the order before returning the row at index nRow
        /// This is for table where FindElements is coming back in reverse order.
        /// NOTE: This is zero referenced (although for some tables, row 0 is the headings).
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="nRow">Index of the row to return</param>
        /// <returns>IWebElement</returns>
        protected IWebElement GetTableRowReverse(By by, int nRow)
        {
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.AreEqual("table", tableElement.TagName, $"Element given by: {by} is not a table!");

            var webElements = tableElement.FindElements(By.TagName("tr")).ToList();
            webElements.Reverse();
            Assert.IsNotNull(webElements[nRow], $"Didn't find any data at row index: {nRow}");

            return !webElements.Any() ? null : webElements[nRow];
        }


        /// <summary>
        /// Useful when multiple tables on same page.
        /// Returns the row at index nRow for the given table web element
        /// NOTE: This is zero referenced (although for some tables, row 0 is the headings).
        /// </summary>
        /// <param name="tableElement">Table to find the row for</param>
        /// <param name="nRow">Index of the row to return</param>
        /// <param name="minRow">Minimum number or rows expected (including header)</param>
        /// <returns>IWebElement</returns>
        protected IWebElement GetTableRowForThisTable(IWebElement tableElement, int nRow, int minRows = 2)
        {
            var webElements = WaitForRowsForSpecificTable(tableElement, minRows);
            Assert.IsNotNull(webElements[nRow], $"Didn't find any data at row index: {nRow}");

            return !webElements.Any() ? null : webElements[nRow];
        }

        /// <summary>
        /// Waits for minimum number of rows on the given table that match the specified condition
        /// </summary>
        /// <param name="table">The specific table element for which you want rows</param>
        /// <param name="minRows">Minimum number of rows that must be returned</param>
        /// <param name="waitSeconds">Timeout for wait</param>
        /// <returns>List<IWebElement></returns>
        protected List<IWebElement> WaitForRowsForSpecificTable(IWebElement table, int minRows, int? waitSeconds = null)
        {
            //We want to wait for the row elements for a specific table, rather than the first table
            //found. So we find the table and pass into this method.
            //The wait.Until() requires a parameter in the form of a lambda expression.
            //It knows that x must be an IWebDriver (although it does not actually seem to use it).
            //This is a bit odd, so just accept that it is so.
            //Bottom line is, it will wait until the number of row elements for the given table
            //matches or exceeds the minimum number of rows found.

            var seconds = waitSeconds ?? WebDriverTimeout;
            var wait = new WebDriverWait(baseDriver, TimeSpan.FromSeconds(seconds));

            if (wait.Until(x => table.FindElements(By.TagName("tr")).Count >= minRows))
            {
                return table.FindElements(By.TagName("tr")).ToList();
            }

            //If we do not find the required number of rows. 
            return null;
        }

        /// <summary>
        /// Returns a list of columns for the given webElementRow.
        /// Could be used in tandem with GetTableRow() 
        /// </summary>
        /// <param name="webElementRow">The row from which you want to get columns</param>
        /// <returns>Collection of IWebElements</returns>
        protected ReadOnlyCollection<IWebElement> GetColumns(IWebElement webElementRow)
        {
            Assert.IsTrue(webElementRow.TagName == "tr", "Element is not a row!");
            return webElementRow.FindElements(By.TagName("td"));
        }

        /// <summary>
        /// Returns the column for the given webElementRow and nColumn index.
        /// NOTE: This is zero referenced 
        /// Could be used in tandem with GetTableRow() 
        /// </summary>
        /// <param name="webElementRow">The row from which you want to get columns</param>
        /// <param name="nColumn">Index of the column to return</param>
        /// <returns>IWebElement</returns>
        protected IWebElement GetColumn(IWebElement webElementRow, int nColumn)
        {
            Assert.IsTrue(webElementRow.TagName == "tr", "Element is not a row!");
            var list = webElementRow.FindElements(By.TagName("td"));
            return webElementRow.FindElements(By.TagName("td"))[nColumn];
        }

        /// <summary>
        /// Looks for table webElement using the given By.
        /// Returns list of rows that contains the specified text
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="textToBeFound">text that you want to row to contain</param>
        /// <returns>Collection of IWebElements</returns>
        protected IEnumerable<IWebElement> GetTableRowsContainingValue(By by, string textToBeFound)
        {
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.IsTrue(tableElement.TagName == "table", "Element is not a table!");

            return tableElement.FindElements(By.TagName("tr")).Where(row => row.Text.Contains(textToBeFound));
        }

        /// <summary>
        /// Returns the number of rows in the table element found by the given By
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="rowCount">Number or rows you expect to be present</param>
        protected int NumberOfRowsInTable(By by)
        {
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.IsTrue(tableElement.TagName == "table", "Element is not a table!");

            var rows = tableElement.FindElements(By.TagName("tr"));
            return rows.Count;
        }

        /// <summary>
        /// Asserts whether or not the table element found by the given By
        /// contains the specified text
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="textToFind">text that you want to table to contain</param>
        protected bool DoesTableContainsValue(By by, string textToFind)
        {
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.IsTrue(tableElement.TagName == "table", "Element is not a table!");

            var rows = tableElement.FindElements(By.TagName("tr"));
            return (rows.Any(row => row.Text.ToLowerInvariant().Contains(textToFind.ToLowerInvariant())));
        }

        /// <summary>
        /// Looks for table webElement using the given By.
        /// Returns text of any columns that contains the specified visibleText
        /// </summary>
        /// <param name="by">Used to locate the element, e.g. By.Id("xyz")</param>
        /// <param name="textToFind">text that you want to cell to contain</param>
        /// <returns></returns>
        protected string GetCellValueInATable(By by, string textToFind)
        {
            //Get table
            var tableElement = GetVisibleElement(by);
            Assert.IsNotNull(tableElement, $"Didn't find the table given by: {by}");
            Assert.IsTrue(tableElement.TagName == "table", "Element is not a table!");

            //Get all table rows
            ICollection<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));
            Assert.IsNotNull(rows, $"Didn't find any rows for the table given by: {by}");

            foreach (var row in rows)
            {
                //To locate columns(cells) of that specific row.
                var columns = row.FindElements(By.TagName("td"));
                foreach (var c in columns)
                {
                    if (c.Text.Equals(textToFind, StringComparison.OrdinalIgnoreCase))
                    {
                        return c.Text;
                    }
                }
            }
            return string.Empty;
        }

        #endregion

    }
}
