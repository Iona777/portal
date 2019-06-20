using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;


namespace BasicFrameworkTwo.Utilities
{
    public static class Driver
    {
       //COULD TURN THESE INTO PROPERTIES LATER JUST FOR PRACTICE AND 'GOOD CODING STANDARDS'
       public static IWebDriver driver;
       public static WebDriverWait wait;

       //These values come from .runsettings file, set via WebHooks
       public static string RootURl;
       public static string ApiRootUrl;
       public static string Server;
       public static string DbInstance;
       public static string DatabaseName;


        //Different versions of OpenBrowser() depending on how you are setting
        //your browser

        /// <summary>
        /// Creates a driver based on the browser selected.
        /// </summary>
        /// <param name="selectedBrowser">Browser to create driver for</param>
        public static void OpenBrowser(string selectedBrowser)
        {
            switch (selectedBrowser.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    driver.Manage().Window.Maximize();
                    break;
                case "ie":
                    driver = new InternetExplorerDriver();
                    driver.Manage().Window.Maximize();
                    break;
                default:
                    Debug.Print("unknown browser selected");
                    break;
            }
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Creates a driver based on the browser selected.
        /// This version takes browser from the App.Config file
        /// </summary>
        /// <param name="selectedBrowser">Browser to create driver for</param>
        public static void OpenBrowser()
        {
            switch (GetBrowser().ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    driver.Manage().Window.Maximize();
                    break;
                case "ie":
                    driver = new InternetExplorerDriver();
                    driver.Manage().Window.Maximize();
                    break;
                default:
                    Debug.Print("unknown browser selected");
                    break;
            }
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetTimeoutSeconds()));
        }
        
        public static void NavigateTo(string targetURL)
        {
            driver.Navigate().GoToUrl(targetURL);
        }

 #region Used by App.config

        public static string GetValueFromConfigKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetTimeoutSeconds()
        {
            var time = GetValueFromConfigKey("DefaultTimeoutSeconds");
            return int.Parse(time);
        }

        public static string GetBrowser()
        {
            return GetValueFromConfigKey("Browser");
        }
#endregion

        public static void ShutDown()
        {
            driver.Quit();
        }
    }
}
