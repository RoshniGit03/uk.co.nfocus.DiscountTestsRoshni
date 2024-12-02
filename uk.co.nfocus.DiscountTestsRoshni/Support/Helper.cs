using OpenQA.Selenium;
using TechTalk.SpecFlow.Tracing;

namespace uk.co.nfocus.DiscountTestsRoshni.Support
{
    [Binding]
    public class Helper
    {
        private readonly IWebDriver _driver;

        public Helper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void DismissDemoBanner()
        {
            try
            {
                // Use partial link text to find the dismiss button
                var bannerDismissButton = _driver.FindElement(By.PartialLinkText("Dismiss"));

                if (bannerDismissButton.Displayed)
                {
                    bannerDismissButton.Click();
                    Console.WriteLine("Demo banner dismissed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No demo banner to dismiss.");
            }
        }

        //Method to capture screenshot and save it
        public void TakeScreenshot(IWebDriver driver, ScenarioContext _scenarioContext)
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

        // Navigation method to go to the Shop page
        public void NavigateToShopPage()
        {
            try
            {
                var shopLink = _driver.FindElement(By.LinkText("Shop"));
                shopLink.Click();
                Console.WriteLine("Navigating to the Shop Page.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Shop link not found.");
            }
        }






    }
}
