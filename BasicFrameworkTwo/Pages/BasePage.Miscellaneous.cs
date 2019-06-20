using System;
using BasicFrameworkTwo.Utilities; //Location of Driver class
using OpenQA.Selenium;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using System.IO.Compression;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf.parser;

using System.Data.SqlClient;

namespace BasicFrameworkTwo.Pages
{
    public partial class BasePage
    {
        #region Windows, frames and popups
        protected string GetNewOpenWindowUrl()
        {            
            var otherwindows = baseDriver.WindowHandles.Where(u => u != baseDriver.CurrentWindowHandle);
            baseDriver.Close();
            baseDriver.SwitchTo().Window(otherwindows.Last());

            return baseDriver.Url;
        }

        /// <summary>
        /// Switches focus to the last frame and returns the current URL
        /// </summary>
        /// <returns></returns>
        protected string GetNewOpenWindowUrlPopUp()
        {
            baseDriver.SwitchTo().Window(baseDriver.WindowHandles.Last());
            return baseDriver.Url;
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

        /// <summary>
        /// Finds the popup using the specified By and clicks on it
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        protected string ClickWithPopup(By by)
        {
            var finder = new PopupWindowFinder(baseDriver);
            return finder.Click(GetClickableElement(by));
        }
        #endregion

        #region Miscellaneous

        /// <summary>
        /// Navigates to the given URL
        /// </summary> 
        /// <param name="url"></param>
        public void Navigate(string url)
        {
            baseDriver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// /// Determines whether or not a file exists for the given filepath
        /// </summary>
        /// <param name="filePath">path to file are checking for</param>
        /// <param name="waitForSeconds">Timeout in seconds</param>
        /// <returns>Boolean</returns>
        protected bool FileExists(string filePath, int waitForSeconds = 30)
        {
            var i = 0;
            while (!File.Exists(filePath) && i < waitForSeconds * 1000)
            {
                Thread.Sleep(1);
                i++;
            }

            return File.Exists(filePath);
        }

        public void ExtractZipFile(string zipPath, string extractPath)
        {
            
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        /// <summary>
        /// Accepts a list of string values to check on the PDF file with the given pdfPath
        /// Checks that ALL of the specified values appear at some point in the file.
        /// </summary>
        /// <param name="pdfPath">Location of the PDF file to check</param>
        /// <param name="values">List of string values to look for in the file</param>
        /// <returns>Boolean</returns>
        protected bool NewPdfContainsValues(string pdfPath, List<string> values)//string invoiceNumber, decimal amountToBePaid)
        {
            bool bPdfContainsValues = true;
            List<string> listWords = new List<string>();

            using (var reader = new iTextSharp.text.pdf.PdfReader(pdfPath))
            {
                for (var i = 1; i <= reader.NumberOfPages; i++) //For each page, get its text
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    var currentPageText = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    currentPageText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                        Encoding.Default.GetBytes(currentPageText)));

                    //For each word, check if it is on the current page
                    for (int j = 0; j < values.Count; j++)
                    {
                        //remove spaces and new lines from currentPageText
                        var cleanedText = currentPageText.RemoveSpacesAndNewLines();

                        if (cleanedText.Contains(Regex.Replace(values[j], @"\s", "")))
                        {
                            listWords.Add(values[j]);
                        }
                    }
                }

                //Check if all the values are present in listWords
                for (int k = 0; k < values.Count; k++)
                {
                    if (!listWords.Contains(values[k]))
                    {
                        bPdfContainsValues = false;
                        break;
                    }
                }
            }
            return bPdfContainsValues;
        }

        /// <summary>
        /// Extracts zipped files from the given zippedPath, extracts them to given extractPath
        /// and returns the unzipped files from that directory
        /// </summary>
        /// <param name="zippedPath">location of zipped files</param>
        /// <param name="extractPath">location to extract unzipped files to </param>
        /// <returns>string[]</returns>
        public string[] ExtractFilesFromDirectory(string zippedPath, string extractPath)
        {
            ExtractZipFile(zippedPath, extractPath);
            string[] extractedFiles = Directory.GetFiles(extractPath);

            return extractedFiles;
        }

        public long GetFileSize(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            return fi.Length;
        }


        /// <summary>
        /// Returns URL to currently displayed page
        /// </summary>
        /// <returns>String</returns>
        public string PageUrl()
        {
            return baseDriver.Url;
        }

        /// <summary>
        /// It is better to use a WebDriverWait rather than Sleep wherever possible.
        /// This is because the operation you are waiting for has the potential to take
        /// considerably more or less time than you specify in your Sleep.
        /// It should generally only be used to assist a Wait, e.g., when an element becomes stale.
        /// Even this can normally be taken of with a try/catch instead of a Sleep.
        /// Sleep should only be used when nothing else is working
        // </summary>
        /// <param name="timeInMilliseconds">How millseconds to pause for.</param>
        public void Pause(int timeInMilliseconds)
        {
            Thread.Sleep(timeInMilliseconds);
        }

        // using Microsoft.Win32;
        protected string GetDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty).ToString();
        }

        public string GetSiteVersion()
        {
            return baseDriver.FindElement(By.ClassName("col-xs-1")).Text;
        }

        public static string GetValueFromConfigKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #endregion
        #region database

        public static SqlConnection ConnectToDatabase(string server, string instance, string database)
        {
            string connectionString = "Server=" + server + "\\" + instance + ";Database=" + database + ";Trusted_Connection = True";
            SqlConnection myconnection = new SqlConnection(connectionString);

            return myconnection;
        }

        public List<string> readDBColumn(SqlConnection dbConnection, string queryString, string columnToReturn)
        {
            List<string> resultData = new List<string>();
            SqlCommand myCommand = new SqlCommand(queryString, dbConnection);

            dbConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(); //Initialises the reader

            //Read() Advances the SqlDataReader to the next record and returns  true if there are more rows; otherwise false.
            //The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.
            //Otherwise it wil say that there are not rows to read.
            while (myReader.Read())
            {
                resultData.Add(myReader[columnToReturn].ToString());
            }

            return resultData;
        }

        #endregion 
    }
}
