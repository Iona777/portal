using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicFrameworkTwo
{
    [TestClass]
    public class RunSettings
    {
        //The TestContext variables here are only relevant when run on server.
        //context is set by the framework itself. It is static so _testContext needs to be static too.
        //_testContext is passed to the non-static TestContext that cannot use static variables.
        //(Can't pass directly from context to TestContext it seems.)
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _testContext = context;
        }

        private static TestContext _testContext;

        public TestContext TestContext => _testContext;

        //The Test.Context.Propeties are set by TFS if tests run remotely on server.
        //If run locally, they will be null and the default value will be taken instead.
        public string Browser => (TestContext.Properties["Browser"] ?? "Chrome").ToString();
        public string EnvironmentKey => (TestContext.Properties["EnvironmentKey"] ?? "POR QA 01").ToString();
        public string WebRoot => (TestContext.Properties["WebRoot"] ?? "http://porapp01qa/").ToString();
        public string ApiRoot => (TestContext.Properties["ApiRoot"] ?? "http://porapp01qa:81/api/").ToString();
        public bool EnableChrome => Convert.ToBoolean(TestContext.Properties["EnableChrome"] ?? "true");
        public bool EnableIe => Convert.ToBoolean(TestContext.Properties["EnableIe"] ?? "false");

        //For use in database connections
        public string Server => (TestContext.Properties["Server"] ?? "PORSQS01DV").ToString();
        public string DbInstance => (TestContext.Properties["DbInstance"] ?? "POR_QA").ToString();
        public string DatabaseName => (TestContext.Properties["DatabaseName"] ?? "Portal").ToString();

    }
}
