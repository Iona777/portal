using System;
using OpenQA.Selenium;
using BasicFrameworkTwo.Utilities; //Location of Driver class

namespace BasicFrameworkTwo.Pages
{
    class LoginPage : BasePage
    {
        //Constructor
        //It will just use the one from BasePage unless it needs something extra.

        //WebElements
        private readonly By _emailLocator = By.Id("Email");
        private readonly By _passwordLocator = By.Id("Password");
        private readonly By _loginButtonLocator = By.Id("Login-Btn");

        //Methods
        public void GotoLoginPage(string rootURL)
        {
            Driver.NavigateTo(rootURL);
        }

        public void EnterEmail(string emailAddress)
        {
            IWebElement email = GetClickableElement(_emailLocator);
            email.SendKeys(emailAddress);
        }

        public void EnterPassword(string passwordText)
        {
            IWebElement password = GetClickableElement(_passwordLocator);
            password.SendKeys(passwordText);
        }

        public void ClickLoginButton()
        {
            ClickOnElement(_loginButtonLocator);
        }
    }

}
