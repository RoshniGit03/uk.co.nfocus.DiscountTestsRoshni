using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow.Tracing;
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
            //Initialize WebDriver based on specified browser or default to Chrome
            string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
            _driver = InitializeDriver(browser);

            //Maximize browser window and navigate to base URL
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl($"{_baseUrl}/my-account/");

            //Dismiss demo banner if it exists
            DismissDemoBanner();

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
                    TakeScreenshot(driver);
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

        //Method to capture screenshot and save it
        private void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                //Generate a unique filename based on the feature and scenario
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileNameBase = $"Error_{_scenarioContext.ScenarioInfo.Title.ToIdentifier()}_{timestamp}";

                //Define the directory to store results
                string artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testresults");
                if (!Directory.Exists(artifactDirectory))
                    Directory.CreateDirectory(artifactDirectory);

                //Save a screenshot of current browser view
                if (driver is ITakesScreenshot takesScreenshot)
                {
                    var screenshot = takesScreenshot.GetScreenshot();
                    string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                    screenshot.SaveAsFile("Error Screenshot");
                    Console.WriteLine($"Screenshot saved: {screenshotFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while taking screenshot: {ex.Message}");
            }
        }


        //Initialize webDriver for specified browser
        private IWebDriver InitializeDriver(string browser)
        {
            return browser.ToLower() switch
            {
                "firefox" => new FirefoxDriver(),
                "edge" => new EdgeDriver(),
                _ => new ChromeDriver(), //Default to chrome
            };
        }

        //Method to dismiss demo banner
        private void DismissDemoBanner()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); //wait for banner to show
                var bannerDismissButton = wait.Until(driver => driver.FindElement(By.CssSelector("body > p > a"))); //Locate in page
                bannerDismissButton.Click();
                Console.WriteLine("Banner dismissed."); //Log
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Demo banner not found or already dismissed."); //Exception messages
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Demo banner dismiss button does not exist.");
            }
        }
    }
}


