using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    public class OrdersPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        //Constructor to initialise WebDriver and WebDriverWait
        public OrdersPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); //Initialise WebDriverWait with a 10-second timeout
        }

        // WebElement for the Order Link
        private IWebElement OrderLink => _driver.FindElement(By.PartialLinkText("order"));

        //Method to retrieve all order numbers from the orders table
        // OrdersPage.cs

        public List<string> GetAllOrders()
        {
            List<string> orderNumbers = new List<string>();

            try
            {
                // Locate table cells that contain the order numbers in the column
                var orderNumberCells = _driver.FindElements(By.CssSelector("table tbody td.woocommerce-orders-table__cell-order-number"));

                // Iterate through the cells and add the order numbers to the list
                foreach (var cell in orderNumberCells)
                {
                    orderNumbers.Add(cell.Text.Trim().Replace("#", ""));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while retrieving order numbers: " + ex.Message);
            }

            // Log all order numbers for debugging
            Console.WriteLine("Found order numbers: " + string.Join(", ", orderNumbers));

            return orderNumbers; // Return the list of order numbers
        }


        // Method to check if a specific order exists in the order history
        public bool DoesOrderExist(string orderNumber)
        {
            var allOrders = GetAllOrders(); // Fetch all order numbers from the history
            return allOrders.Contains(orderNumber); // Return true if order number exists, else false
        }
    }
}
