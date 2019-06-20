using System;
using System.IO;
using TechTalk.SpecFlow;
using BasicFrameworkTwo.Pages; //namespace for your page objects.
using BasicFrameworkTwo.Utilities; //where Driver Class is defined
using BasicFrameworkTwo.Templates; //where UserTemplate is defined
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BasicFrameworkTwo.Steps
{
    [Binding]
    class LoginSteps
    {
        private readonly LoginPage _theLoginPage;
        private readonly WelcomePage _theWelcomePage;
        private readonly TestDataRepository _theRepository;

        //Constructor
        public LoginSteps()
        {
            _theLoginPage = new LoginPage();
            _theWelcomePage = new WelcomePage();
            _theRepository = new TestDataRepository();
        }

        [Given(@"I am on the Login Page")]
        public void GivenIAmOnTheLoginPage()
        {
            _theLoginPage.GotoLoginPage(Driver.RootURl);
        }

        
        [When(@"I login as user ""(.*)""")]
        public void WhenILoginAsUser(string userKey)
        {
            var user = _theRepository.GetUserDetails(userKey);

            _theLoginPage.EnterEmail(user.Username);
            _theLoginPage.EnterPassword(user.Password);
            _theLoginPage.ClickLoginButton();
        }


        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.IsTrue(_theWelcomePage.IsLoggedIn());
        }

        [When(@"I login as a using email address ""(.*)""")]
        public void WhenILoginAsAUsingEmailAddress(string email)
        {
            var user = _theRepository.GetUserDetailsByEmail(email);

            _theLoginPage.EnterEmail(user.Username);
            _theLoginPage.EnterPassword(user.Password);
            _theLoginPage.ClickLoginButton();
        }

        
        [When(@"user is NOT a cogs user")]
        public void WhenUserIsNOTACogsUser()
        {
            Assert.IsFalse(_theWelcomePage.IsUserCogUser(Driver.Server, Driver.DbInstance, Driver.DatabaseName));
        }
        
    }
}
