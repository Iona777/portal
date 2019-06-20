using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicFrameworkTwo.Pages;
using BasicFrameworkTwo.Utilities;
using BasicFrameworkTwo.Templates;

namespace BasicFrameworkTwo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod2()
        {
            int a = 1;
            int b = a + 2;

            RunSettings _runSettings = new RunSettings();
            var url = _runSettings.WebRoot;

            //until using specflow, need to get driver here
            Driver.OpenBrowser(_runSettings.Browser);

            //LoginPage _page = new LoginPage();

            //_page.GotoLoginPage(_runSettings.WebRoot);

            Assert.IsTrue(b==3);
            
            TestDataRepository td = new TestDataRepository();
            td.ReadFileIntoDataRepository();
           
            Driver.ShutDown();

        }
    }
}
