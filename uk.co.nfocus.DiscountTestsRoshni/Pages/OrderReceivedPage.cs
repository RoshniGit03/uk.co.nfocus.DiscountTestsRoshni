using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    public class OrderReceivedPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor to initialise WebDriver and WebDriverWait
        public OrderReceivedPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); //Initialise WebDriverWait with a 10-second timeout
        }

        // Method to extract the order number from the Order Received page
        public string GetOrderNumber()
        {
            // Wait until the order number element is located and displayed
            var orderNumberElement = _wait.Until(drv => drv.FindElement(By.CssSelector("#post-6 > div > div > div > ul > li.woocommerce-order-overview__order.order > strong")));
            return orderNumberElement.Text;
        }
    }
}

