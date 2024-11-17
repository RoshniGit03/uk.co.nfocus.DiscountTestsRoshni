using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    public class OrdersPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Constructor to initialize WebDriver and WebDriverWait
        public OrdersPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Initialize WebDriverWait with a 10-second timeout
        }

        // WebElement for the Order Link
        private IWebElement OrderLink => _driver.FindElement(By.PartialLinkText("order"));

        //Method to get the order number text
        public string GetOrderNumber()
        {
            //Wait until the order number element is located and displayed
            var orderNumberElement = _wait.Until(drv => drv.FindElement(By.CssSelector("#post-6 > div > div > div > ul > li.woocommerce-order-overview__order.order > strong")));
            return orderNumberElement.Text;
        }

        //Method to retrieve all order numbers from orders table
        public List<string> GetAllOrders()
        {
            List<string> orderNumbers = new List<string>();

            //Locate table cells that contain the order numbers in the column
            var orderNumberCells = _driver.FindElements(By.CssSelector("table tbody td.woocommerce-orders-table__cell-order-number"));

            //Iterate through the cells
            foreach (var cell in orderNumberCells)
            {
                //Add order number text to the list (trim in case of extra spaces)
                orderNumbers.Add(cell.Text.Trim().Replace("#", ""));
            }

            return orderNumbers; // Return the list of order numbers
        }

        //Method to verify if a given order number exists in the table
        public void VerifyOrderExists(string orderNumber)
        {
            var allOrders = GetAllOrders(); //Fetch all order numbers in table

            //Check if the expected order number is in the list of orders
            if (!allOrders.Contains(orderNumber))
            {
                //Log all order numbers for debugging purposes
                Console.WriteLine($"Order number '{orderNumber}' not found. All orders: {string.Join(", ", allOrders)}");

                //Throw an exception to fail test
                throw new Exception($"Order number '{orderNumber}' not found in the list of orders.");
            }
            else
            {
                Console.WriteLine($"\tOrder number '{orderNumber}' found in list of orders."); //Log
            }
        }

    }
}
