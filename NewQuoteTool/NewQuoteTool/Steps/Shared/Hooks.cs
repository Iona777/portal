using TechTalk.SpecFlow;
using NewQuoteTool.Utilities; //Location of Driver class

namespace NewQuoteTool.Steps.Shared
{
    [Binding]
    public class Hooks
    {
        //Since RunSettings class is non-static and the Driver class is static,
        //we need a static instance of RunSettings to pass details to Driver class
        public static RunSettings _runSettings = new RunSettings();

        [BeforeScenario()]
        public static void StartBrowsers()
        {
            Driver.RootURl = _runSettings.WebRoot;
            Driver.OpenBrowser(_runSettings.Browser);
            
        }

        [AfterScenario]
        public void ShutDown()
        {
            Driver.ShutDown();
        }

    }
}
