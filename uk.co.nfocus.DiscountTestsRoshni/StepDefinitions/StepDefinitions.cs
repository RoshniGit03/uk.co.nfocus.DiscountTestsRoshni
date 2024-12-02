using NUnit.Framework;
using OpenQA.Selenium;
using uk.co.nfocus.DiscountTestsRoshni.Pages;
using uk.co.nfocus.DiscountTestsRoshni.Support;

namespace uk.co.nfocus.DiscountTestsRoshni.StepDefinitions
{
    [Binding]
    public class StepDefinitions
    {
        //Declare fields for all page objects and necessary data
        private readonly IWebDriver _driver;  //WebDriver instance for controlling the browser
        private LoginPage _loginPage;  //Page object for login page
        private ShopPage _shopPage;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;
        private OrderReceivedPage _orderReceivedPage;
        private OrdersPage _ordersPage;
        private AccountPage _accountPage;
        private Helper _helper;
        private decimal _subtotal, _discount, _shipping, _total;  //Variables to hold cart totals and order calculations

        //Constructor with dependency injection via ScenarioContext
        [Obsolete]  //Marking the constructor as obsolete due to direct use of ScenarioContext

        //Constructor with dependency injection via ScenarioContext
        public StepDefinitions()
        {
            //Retrieve the WebDriver from ScenarioContext
            _driver = (IWebDriver)ScenarioContext.Current["WebDriver"];
        }

        //Step definition for logging into the store
        [Given(@"I am logged into the store as ""(.*)"" and ""(.*)""")]
        public void LogIn(string email, string password)
        {
            Console.WriteLine($"Attempting to log in with email: {email}, password: {password}");

            // Initialise LoginPage only when logging in
            _loginPage = new LoginPage(_driver);

            //Call Login method on LoginPage to perform login with provided credentials
            _loginPage.Login(email, password);
        }

        //Step definition for ensuring the cart is empty
        [Given(@"the cart is empty")]
        public void ClearCart()
        {
            //Initialise CartPage only when interacting with the cart
            _cartPage = new CartPage(_driver);

            // Navigate to the Cart page and ensure it's empty
            _cartPage.EnsureCartIsEmpty();
        }

        //Step definition for adding an item to the shopping cart
        [When(@"I add a ""(.*)"" to the cart")]
        public void AddToCart(string itemName)
        {
            //Initialise Helper and ShopPage only when navigating to and interacting with the shop page
            _helper = new Helper(_driver);
            _shopPage = new ShopPage(_driver);

            //Navigate to the shop page and add the specified item to the cart
            _helper.NavigateToShopPage();
            _shopPage.AddItemToCart(itemName);
        }

        //Step definition for applying a coupon to the cart
        [When(@"I apply a ""(.*)""")]
        public void ApplyCoupon(string couponCode)
        {
            //Initialise CartPage only when interacting with the cart
            _cartPage = new CartPage(_driver);

            _cartPage.GoToCartPage(); //Navigate to the cart page 
            _cartPage.ApplyCoupon(couponCode); //Apply specified coupon code
        }

        //Step definition to validate the applied discount
        [Then(@"the discount should be (.*)% of the subtotal")]
        public void DiscountValidation(decimal expectedPercentage)
        {
            //Initialise CartPage only when retrieving cart totals
            _cartPage = new CartPage(_driver);

            // Get current cart totals
            (_subtotal, _discount, _shipping, _total) = _cartPage.GetCartTotals();

            // Convert expected percentage to a decimal multiplier
            decimal expectedMultiplier = expectedPercentage / 100;

            // Calculate expected discount based on the subtotal
            decimal expectedDiscount = _subtotal * expectedMultiplier;

            // Calculate the actual percentage discount
            decimal actualPercentage = (_discount / _subtotal) * 100;

            // Log the details
            Console.WriteLine($"[Subtotal: £{_subtotal:F2} | Expected Discount: {expectedPercentage}% (£{expectedDiscount:F2}) | Actual Discount: {actualPercentage:F2}% (£{_discount:F2})]");

            // Assert that the actual discount matches the expected discount within 0.01m tolerance
            if (Math.Abs(actualPercentage - expectedPercentage) > 0.01m)
            {
                // Log a message indicating a mismatch
                Console.WriteLine($"\tThe actual discount ({actualPercentage:F2}%) does not match the expected {expectedPercentage}%. Returning actual percentage.");
                Assert.Fail($"Discount validation failed. Expected: {expectedPercentage}%, Actual: {actualPercentage:F2}%");
            }

            // Log success if the discount matches
            Console.WriteLine("\tThe discount is correctly applied.");
        }

        //Step definition to validate final total calculation
        [Then(@"the total should be calculated correctly")]
        public void TotalValidation()
        {
            //Calculate expected total (subtotal - discount + shipping)
            decimal expectedTotal = _subtotal - _discount + _shipping;

            //Log actual and expected totals
            Console.WriteLine($"[Subtotal: £{_subtotal:F2} | Discount: £{_discount:F2} | Shipping: £{_shipping:F2} | Expected Total: £{expectedTotal:F2} | Actual Total: £{_total:F2}]");

            //Assert that actual total matches expected total
            Assert.That(_total, Is.EqualTo(expectedTotal).Within(0.01m), "Total amount after discount and shipping is incorrect.");

            //Log success if calculated correctly
            Console.WriteLine("\tThe total is correctly calculated after applying discount and adding shipping.");
        }

        //Step definition to proceed to the checkout page
        [When(@"I checkout with the details of ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)"" ""(.*)""")]
        public void CheckoutDetails(string firstName, string lastName, string address, string city, string postcode, string phoneNumber)
        {
            // Initialise CartPage and CheckoutPage when proceeding to checkout
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);

            _cartPage.ProceedToCheckout();
            _checkoutPage.CompleteBillingDetails(firstName, lastName, address, city, postcode, phoneNumber);
        }

        //Step definition to select the payment method and order
        [When(@"I select ""(.*)"" then place order")]
        public void CompleteOrder(string paymentMethod)
        {
            //Initialise CheckoutPage when interacting with payment methods
            _checkoutPage = new CheckoutPage(_driver);

            //select the specified payment method
            _checkoutPage.SelectPaymentMethod(paymentMethod);
            //Place order using checkout page's PlaceOrder method
            _checkoutPage.PlaceOrder();
        }

        //Step definition to validate order number appearance in order history
        [Then(@"the order number should appear in my order history")]
        public void OrderValidation()
        {
            // Initialise OrderReceivedPage, AccountPage, and OrdersPage when verifying order history
            _orderReceivedPage = new OrderReceivedPage(_driver);
            _accountPage = new AccountPage(_driver);
            _ordersPage = new OrdersPage(_driver);

            // Extract order number from the Order Received page
            string orderNumber = _orderReceivedPage.GetOrderNumber();

            // Navigate to the Orders page to verify the order number appears in the history
            _accountPage.NavigateToOrders();

            // Perform the assertion in the step definition
            bool orderExists = _ordersPage.DoesOrderExist(orderNumber);
            Assert.IsTrue(orderExists, $"Order number '{orderNumber}' not found in order history.");
        }
    }
}
