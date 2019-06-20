using System;
using BasicFrameworkTwo.Utilities; //location of Driver class
using TechTalk.SpecFlow;


namespace BasicFrameworkTwo.Steps.Shared
{
    [Binding]
    public class WebHooks
    {
        //Since RunSettings class is non-static and the Driver class is static,
        //we need a static instance of RunSettings to pass details to Driver class
        public static RunSettings _runSettings = new RunSettings();

       //Constructor
       //Don't seem to need one. BeforeScenario is run before the constructor anyway.

        [BeforeScenario()]
        public static void StartBrowsers()
        {
            Driver.RootURl = _runSettings.WebRoot;
            Driver.ApiRootUrl = _runSettings.ApiRoot;
            Driver.Server = _runSettings.Server;
            Driver.DatabaseName = _runSettings.DatabaseName;
            Driver.DbInstance = _runSettings.DbInstance;

            Driver.OpenBrowser(_runSettings.Browser);

        }

        [AfterScenario()]
        public static void CloseBrowsers()
        {
            Driver.ShutDown();
        }
    }
}
    
