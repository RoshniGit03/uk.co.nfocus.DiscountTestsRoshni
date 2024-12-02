using OpenQA.Selenium;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        // Locators for the login page
        private readonly By _usernameField = By.Id("username");
        private readonly By _passwordField = By.Id("password");
        private readonly By _loginButton = By.CssSelector("button[name='login']");
        private readonly By _logoutButton = By.CssSelector(".logout"); // Example locator for logout button, adjust as needed

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Method to log in, accepts username and password as parameters
        public void Login(string username = null, string password = null)
        {
            // If username and password are not provided, fetch them from environment variables
            username = username ?? Environment.GetEnvironmentVariable("USERNAME");
            password = password ?? Environment.GetEnvironmentVariable("PASSWORD");

            // If username or password is still null, stop the test execution
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Username or password is missing. Test execution stopped.");
            }

            // Navigate to the login page using base URL from Hooks (ensure your hooks provide the base URL)
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.edgewordstraining.co.uk/demo-site";
            _driver.Navigate().GoToUrl($"{baseUrl}/my-account/");

            // Enter the username and password in the login form
            _driver.FindElement(_usernameField).SendKeys(username);
            _driver.FindElement(_passwordField).SendKeys(password);
            _driver.FindElement(_loginButton).Click();

            // Verify if login is successful by checking for the presence of a logout button (or another indication)
            if (IsLoginSuccessful())
            {
                Console.WriteLine("\tSuccessfully logged in");
            }
            else
            {
                Console.WriteLine("\tLogin failed");
                throw new InvalidOperationException("Login was not successful.");
            }
        }

        // Helper method to check if the login was successful (e.g., check if logout button is present)
        private bool IsLoginSuccessful()
        {
            try
            {
                var logoutButton = _driver.FindElement(By.LinkText("Log out"));
                return logoutButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
