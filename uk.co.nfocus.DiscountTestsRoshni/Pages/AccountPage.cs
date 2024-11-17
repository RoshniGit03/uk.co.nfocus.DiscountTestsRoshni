using OpenQA.Selenium;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    //POM for the account page
    public class AccountPage
    {
        private readonly IWebDriver _driver;

        public AccountPage(IWebDriver driver) => _driver = driver;

     //Locator for the Account link on the page
     private IWebElement AccountLink => _driver.FindElement(By.PartialLinkText("account"));


        //method to log out of the account
        public void Logout()
        {
            AccountLink.Click();
            _driver.FindElement(By.LinkText("Log out")).Click();
        }

        //method to navigate to the orders section
        public void NavigateToOrders()
        {
            AccountLink.Click();
            _driver.FindElement(By.LinkText("Orders")).Click();
            Console.WriteLine("Navigating to orders page");
        }

    }
}

