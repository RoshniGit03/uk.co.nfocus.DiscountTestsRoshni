using OpenQA.Selenium;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    public class LoginPage
    {

        private readonly IWebDriver _driver; //Private variable to hold the WebDriver instance

        public LoginPage(IWebDriver driver)
        {
            _driver = driver; //Constructor to assign driver
        }

        // Method to log in, accepts username and password as parameters
        public void Login(string username = null, string password = null)
        {
         // If username and password are not provided, fetch them from environment variables
         // username = username ?? Environment.GetEnvironmentVariable("USERNAME") ?? "email@address.com";
         // password = password ?? Environment.GetEnvironmentVariable("PASSWORD") ?? "strong!password";

            //Navigate to the login page
            _driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/demo-site/my-account/");

            //Enter the username and password in the login form
            _driver.FindElement(By.Id("username")).SendKeys(username);
            _driver.FindElement(By.Id("password")).SendKeys(password);
            _driver.FindElement(By.CssSelector("button[name='login']")).Click();
            Console.WriteLine("\tSuccessfully logged in");
        }
    }
}

