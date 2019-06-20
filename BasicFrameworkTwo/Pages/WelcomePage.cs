using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace BasicFrameworkTwo.Pages
{
    class WelcomePage : BasePage
    {

        //Constructor
        public WelcomePage()
        {}

        //WebElement Locators
        private readonly By _GasButtonLocator = By.XPath(".//*[contains(text(),'View your gas account')]");
        private readonly By _LogOutLinkLocator = By.Id("logOutLink");

        //Methods
        public bool CheckGasAccountButtonDisplayed()
        {
            return IsElementDisplayed(_GasButtonLocator);
        }

        public bool IsLoggedIn()
        {
            return IsElementDisplayed(_LogOutLinkLocator);
        }

        public bool IsUserCogUser(string server, string dbInstance, string database)
        {
            var theConnection = ConnectToDatabase(server, dbInstance, database);
            const string theQueryString =
                "SELECT u.[Email], CASE WHEN c.[Id] IS NULL THEN 'No' ELSE 'Yes' END AS [HasCogAccess] "
                + "FROM[dbo].[AspNetUsers] u "
                + "LEFT JOIN[dbo].[AspNetUserClaims] c ON c.[UserId] = u.[Id] AND c.[ClaimType] = 'CanDownloadCogReports' "
                + "WHERE u.[Email] = 'greg.macdonald@gazprom-energy.com'";
            const string resultColumnName = "HasCogAccess";

            var result = readDBColumn(theConnection, theQueryString, resultColumnName);

            return (result[0] == "Yes");
        }

    }
}
