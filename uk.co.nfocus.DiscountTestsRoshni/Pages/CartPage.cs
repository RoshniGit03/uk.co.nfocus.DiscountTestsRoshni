using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages
{
    // POM class for the Cart Page with methods for coupon application and total calculations
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        //Constructor to initialise WebDriver and WebDriverWait
        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Elements (Locators)
        private IWebElement CheckoutLink => _driver.FindElement(By.PartialLinkText("checkout"));
        private IWebElement CartLink => _driver.FindElement(By.CssSelector("a[href*='cart']"));
        private IWebElement CouponField => _driver.FindElement(By.Id("coupon_code"));
        private IWebElement ApplyCouponButton => _driver.FindElement(By.CssSelector("button[name='apply_coupon']"));
        private IWebElement CouponDiscount => _wait.Until(drv => drv.FindElement(By.CssSelector(".cart-discount.coupon-edgewords td span")));
        private IWebElement Subtotal => _wait.Until(drv => drv.FindElement(By.CssSelector(".cart-collaterals .cart-subtotal td span")));
        private IWebElement Shipping => _wait.Until(drv => drv.FindElement(By.CssSelector("#shipping_method span bdi")));
        private IWebElement Total => _wait.Until(drv => drv.FindElement(By.CssSelector(".order-total td strong span bdi")));

        // Navigate to the cart page
        public void GoToCartPage()
        {
            CartLink.Click(); // Click the cart link to navigate to the cart page
            Console.WriteLine("Navigating to cart page"); // Log action
        }

        // Ensure Cart is Empty (if there are any items)
        public void EnsureCartIsEmpty()
        {
            GoToCartPage(); // Navigate to the cart page first
            try
            {
                var removeButtons = _driver.FindElements(By.CssSelector(".remove")); // Find all remove buttons for items in the cart
                foreach (var button in removeButtons)
                {
                    button.Click(); // Click each remove button to empty the cart
                }
                Console.WriteLine("\tCart emptied"); // Log cart has been emptied
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("\tCart is already empty"); // If no items are found, log that the cart is already empty
            }
        }

        // Method to apply a discount coupon
        public void ApplyCoupon(string couponCode)
        {
            CouponField.SendKeys(couponCode); // Type coupon code into coupon field
            ApplyCouponButton.Click(); // Click button to apply coupon
            Console.WriteLine($"\tCoupon {couponCode} applied"); // Log action
        }

        // Retrieves values as a tuple (subtotal, discount, shipping, total)
        public (decimal subtotal, decimal discount, decimal shipping, decimal total) GetCartTotals()
        {
            // Parse the currency values and convert them to decimal type for calculation
            decimal subtotal = decimal.Parse(Subtotal.Text.Replace("£", "").Trim(), CultureInfo.InvariantCulture); // Subtotal value (removes "£" symbol)
            decimal discount = decimal.Parse(CouponDiscount.Text.Replace("£", "").Trim(), CultureInfo.InvariantCulture); // Discount value (removes "£" symbol)
            decimal shipping = decimal.Parse(Shipping.Text.Replace("£", "").Trim(), CultureInfo.InvariantCulture); // Shipping cost (removes "£" symbol)
            decimal total = decimal.Parse(Total.Text.Replace("£", "").Trim(), CultureInfo.InvariantCulture); // Total amount (removes "£" symbol)

            return (subtotal, discount, shipping, total); // Return all four values as a tuple
        }

        // Method to proceed to checkout page
        public void ProceedToCheckout()
        {
            GoToCartPage(); // Ensure user is on the cart page before proceeding
            CheckoutLink.Click(); // Click on checkout link
            Console.WriteLine("Proceeding to checkout"); // Log that checkout process is starting
        }
    }
}
