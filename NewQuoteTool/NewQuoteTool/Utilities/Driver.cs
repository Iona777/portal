using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using static NewQuoteTool.Steps.Shared.Hooks; //This allows all static vars in Hooks to be accessed without adding Hooks before them

namespace NewQuoteTool.Utilities
{
    public class Driver
    {
        public static IWebDriver driver;
        //This values come from .runsettings file, set via WebHooks (explained later)
        public static string RootURl;

        
        public static void OpenBrowser(string selectedBrowser)
        {
            switch (selectedBrowser.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver(".");
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
        }

        public static void NavigateTo(string targetURL)
        {
            driver.Navigate().GoToUrl(targetURL);
        }

        public static int GetTimeoutSeconds()
        {
            var time = _runSettings.DefaultTimeoutSeconds;
            return int.Parse(time);
        }

        public static void ShutDown()
        {
            driver.Quit();
        }

    }
}
