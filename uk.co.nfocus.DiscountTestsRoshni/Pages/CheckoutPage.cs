using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    //POM class for Checkout Page interactions
    public class CheckoutPage
    {
        //WebDriver instance for interacting with browser elements
        private readonly IWebDriver _driver;

        //WebDriverWait instance for handling dynamic waits
        private readonly WebDriverWait _wait;

        //Constructor to initialize WebDriver and WebDriverWait
        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;//Assign the passed-in WebDriver to the class field
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); //Set a default timeout for element waits
        }

        //method to fill in billing details required for checkout
        public void CompleteBillingDetails(string firstName, string lastName, string address, string city, string postcode, string phoneNumber)
        {
            _driver.FindElement(By.Id("billing_first_name")).Clear();
            _driver.FindElement(By.Id("billing_first_name")).SendKeys(firstName);

            _driver.FindElement(By.Id("billing_last_name")).Clear();
            _driver.FindElement(By.Id("billing_last_name")).SendKeys(lastName);

            _driver.FindElement(By.Id("billing_address_1")).Clear();
            _driver.FindElement(By.Id("billing_address_1")).SendKeys(address);

            _driver.FindElement(By.Id("billing_city")).Clear();
            _driver.FindElement(By.Id("billing_city")).SendKeys(city);

            _driver.FindElement(By.Id("billing_postcode")).Clear();
            _driver.FindElement(By.Id("billing_postcode")).SendKeys(postcode);

            _driver.FindElement(By.Id("billing_phone")).Clear();
            _driver.FindElement(By.Id("billing_phone")).SendKeys(phoneNumber);
            Console.WriteLine($"\tBilling to {firstName} {lastName} at {address}, {city}, {postcode}. Contact: {phoneNumber}");

        }

        //Method to select a payment method
        public void SelectPaymentMethod(string paymentMethod)
        {
            //Wait for any overlay to disappear to ensure page is fully interactable
            _wait.Until(driver =>
                !driver.FindElements(By.CssSelector(".blockUI.blockOverlay"))
                       .Any(overlay => overlay.Displayed));

            //Locate  payment method by its label and ensure it is visible and interactable
            var paymentOption = _wait.Until(driver =>
                driver.FindElement(By.XPath($"//label[contains(text(), '{paymentMethod}')]")));

            //Click selected payment option
            paymentOption.Click();

            // Log
            Console.WriteLine($"{paymentMethod} selected");
        }

        //Method to place an order
        public void PlaceOrder()
        {
            //Locate and click "Place Order" button
            _driver.FindElement(By.Id("place_order")).Click();

            //Log
            Console.WriteLine($"\tOrder placed successfully");
        }
    }
}