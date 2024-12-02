using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    // POM class for Checkout Page interactions
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        //Constructor to initialise WebDriver and WebDriverWait
        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;  // Assign the passed-in WebDriver to the class field
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));  // Set a default timeout for element waits
        }

        // Locators
        private IWebElement BillingFirstName => _driver.FindElement(By.Id("billing_first_name"));
        private IWebElement BillingLastName => _driver.FindElement(By.Id("billing_last_name"));
        private IWebElement BillingAddress => _driver.FindElement(By.Id("billing_address_1"));
        private IWebElement BillingCity => _driver.FindElement(By.Id("billing_city"));
        private IWebElement BillingPostcode => _driver.FindElement(By.Id("billing_postcode"));
        private IWebElement BillingPhone => _driver.FindElement(By.Id("billing_phone"));
        private IWebElement PlaceOrderButton => _driver.FindElement(By.Id("place_order"));
        private IWebElement OrderHistoryLink => _driver.FindElement(By.LinkText("View your order history"));

        // Method to complete billing details
        public void CompleteBillingDetails(string firstName, string lastName, string address, string city, string postcode, string phoneNumber)
        {
            BillingFirstName.Clear();
            BillingFirstName.SendKeys(firstName);

            BillingLastName.Clear();
            BillingLastName.SendKeys(lastName);

            BillingAddress.Clear();
            BillingAddress.SendKeys(address);

            BillingCity.Clear();
            BillingCity.SendKeys(city);

            BillingPostcode.Clear();
            BillingPostcode.SendKeys(postcode);

            BillingPhone.Clear();
            BillingPhone.SendKeys(phoneNumber);

            Console.WriteLine($"\tBilling to {firstName} {lastName} at {address}, {city}, {postcode}. Contact: {phoneNumber}");
        }

        // Method to select a payment method
        public void SelectPaymentMethod(string paymentMethod)
        {
            // Wait for any overlay to disappear to ensure page is fully interactable
            WaitForElementToDisappear(By.CssSelector(".blockUI.blockOverlay"));

            // Wait for the payment option to be visible and interactable
            var paymentOption = _wait.Until(driver =>
                driver.FindElement(By.XPath($"//label[contains(text(), '{paymentMethod}')]")));

            // Click the selected payment option
            paymentOption.Click();

            // Log
            Console.WriteLine($"{paymentMethod} selected");
        }

        // Method to place the order
        public void PlaceOrder()
        {
            PlaceOrderButton.Click(); // Locate and click "Place Order" button

            // Log
            Console.WriteLine($"\tOrder placed successfully");
        }

        // Wait Helper Method to ensure an element is not present (e.g., overlay disappears)
        private void WaitForElementToDisappear(By locator)
        {
            _wait.Until(driver =>
                !driver.FindElements(locator).Any(element => element.Displayed));
        }

        // Method to navigate to order history and check for the order
        public bool VerifyOrderInHistory()
        {
            OrderHistoryLink.Click();  // Navigate to order history page
            Console.WriteLine("\tNavigating to order history page");

            // Add logic to verify if the order number is displayed in the history
            // This depends on how the order number is displayed on the page
            var orderNumber = _driver.FindElement(By.CssSelector(".order-number")).Text;
            Console.WriteLine($"\tFound order number: {orderNumber}");

            // Return true if order number is found, else false
            return !string.IsNullOrEmpty(orderNumber);
        }
    }
}
