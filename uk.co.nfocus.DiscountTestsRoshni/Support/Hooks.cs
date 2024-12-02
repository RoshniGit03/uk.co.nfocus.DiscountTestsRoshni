using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using uk.co.nfocus.DiscountTestsRoshni.Pages;

namespace uk.co.nfocus.DiscountTestsRoshni.Support
{
    [Binding]
    public class Hooks
    {
    //WebDriver instance used across tests
    private IWebDriver _driver;

    //Base URL for the application under test, with a fallback default value
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("BASE_URL") 
        ?? "https://www.edgewordstraining.co.uk/demo-site";

    //ScenarioContext instance for sharing data between steps
    private readonly ScenarioContext _scenarioContext;

    //Constructor for initializing the Hooks class with ScenarioContext
    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext; //Assign ScenarioContext to the private field
    }

    [BeforeScenario]
        public void BeforeScenario()
        {
            //Initialise WebDriver based on specified browser or default to Chrome
            // Warn if the BaseURL or Browser is not set
            if (_baseUrl == "https://www.edgewordstraining.co.uk/demo-site")
            {
                Console.WriteLine("Warning: Using default BaseURL.");
            }

            string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
            if (browser == "chrome" && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BROWSER")))
            {
                Console.WriteLine("Using default browser.");
            }

            _driver = InitialiseDriver(browser);

            //Maximize browser window and navigate to base URL
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl($"{_baseUrl}/my-account/");

            //Dismiss demo banner if it exists
            var helper = new Helper(_driver);
            helper.DismissDemoBanner();

            //Share the driver instance with the ScenarioContext for use in step definitions
            _scenarioContext["WebDriver"] = _driver;
        }

        //Takes screenshots for failed tests, logs out if necessary, and closes the browser
        [AfterScenario]
        public void AfterScenario()
        {
            //Check if WebDriver is available in the context
            if (_scenarioContext.ContainsKey("WebDriver"))
            {
                var driver = (IWebDriver)_scenarioContext["WebDriver"];

                //If scenario failed, capture a screenshot
                if (_scenarioContext.TestError != null)
                {
                    var helper = new Helper(driver);
                    helper.TakeScreenshot(driver, _scenarioContext);
                }

                //Log test results
                if (_scenarioContext.TestError == null)
                {
                    Console.WriteLine("Test Passed: All assertions were successful.");
                }
                else
                {
                    Console.WriteLine($"Test Failed: {_scenarioContext.TestError.Message}");
                }

                var cartPage = new CartPage(driver);
                cartPage.EnsureCartIsEmpty(); //method to clear the cart

                //Attempt to log out
                try
                {
                    var accountPage = new AccountPage(driver);
                    accountPage.Logout();
                    Console.WriteLine("Logged out.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during logout: {ex.Message}");
                }

                //Quit the WebDriver session
                driver.Quit();
            }
        }

 


        //Initialise webDriver for specified browser
        private static IWebDriver InitialiseDriver(string browser) => browser.ToLower() switch
        {
            "firefox" => new FirefoxDriver(),
            "edge" => new EdgeDriver(),
            _ => new ChromeDriver(), //Default to chrome
        };

    }
}


