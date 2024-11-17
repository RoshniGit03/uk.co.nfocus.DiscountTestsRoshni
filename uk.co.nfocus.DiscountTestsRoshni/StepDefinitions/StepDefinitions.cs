using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using uk.co.nfocus.DiscountTestsRoshni.Pages;
using System;
using uk.co.nfocus.DiscountTestsRoshni.Support;
using OpenQA.Selenium.DevTools.V128.Audits;

namespace uk.co.nfocus.DiscountTestsRoshni.StepDefinitions
{
    [Binding]
    public class StepDefinitions
    {
        //Declare fields for all page objects and necessary data
        private readonly IWebDriver _driver;  //WebDriver instance for controlling the browser
        private readonly LoginPage _loginPage;  //Page object for login page
        private readonly ShopPage _shopPage; 
        private readonly CartPage _cartPage;
        private readonly CheckoutPage _checkoutPage;
        private readonly OrdersPage _ordersPage;
        private readonly AccountPage _accountPage;
        private decimal _subtotal, _discount, _shipping, _total;  //Variables to hold cart totals and order calculations

        //Constructor with dependency injection via ScenarioContext
        [Obsolete]  //Marking the constructor as obsolete due to direct use of ScenarioContext

        //Constructor with dependency injection via ScenarioContext
        public StepDefinitions()
        {
            //Retrieve the WebDriver from ScenarioContext
            _driver = (IWebDriver)ScenarioContext.Current["WebDriver"];

            // Initialize the page objects with the WebDriver
            _loginPage = new LoginPage(_driver);
            _shopPage = new ShopPage(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
            _ordersPage = new OrdersPage(_driver);
            _accountPage = new AccountPage(_driver);
        }

        //Step definition for logging into the store
        [Given(@"I am logged into the store as ""(.*)"" and ""(.*)""")]
        public void LogIn(string email, string password)
        {
            Console.WriteLine($"Attempting to log in with email: {email}, password: {password}");

            //Call Login method on LoginPage to perform login with provided credentials
            _loginPage.Login(email, password);

            //Ensure the cart is empty before starting the test to maintain a clean state
            _cartPage.EnsureCartIsEmpty();
        }

        //Step definition for adding an item to the shopping cart
        [When(@"I add a ""(.*)"" to the cart")]
        public void AddToCart(string itemName)
        {
            //Navigate to the shop page and add the specified item to the cart
            _shopPage.GoToShopPage();
            _shopPage.AddItemToCart(itemName);
        }

        //Step definition for applying a coupon to the cart
        [When(@"I apply the coupon ""(.*)""")]
        public void ApplyCoupon(string couponCode)
        {
            _cartPage.GoToCartPage(); //Navigate to the cart page 
            _cartPage.ApplyCoupon(couponCode); //Apply specified coupon code
        }

        //Step definition to validate the applied discount
        [Then(@"the discount should be 15% of the subtotal")]
        public void DiscountValidation()
        {
            //Get current cart totals
            (_subtotal, _discount, _shipping, _total) = _cartPage.GetCartTotals();

            //Calculate expected discount as 15% of subtotal
            decimal expectedDiscount = _subtotal * 0.15m;

            // Log actual and expected discount in the output
            Console.WriteLine($"[Subtotal: £{_subtotal:F2} | Expected Discount: £{expectedDiscount:F2} | Actual Discount: £{_discount:F2}]");

            //Assert that actual discount is within 0.01m of expected discount (financial calculations)
            Assert.That(_discount, Is.EqualTo(expectedDiscount).Within(0.01m), "Discount is not 15% of the subtotal.");

            //Log success IF discount is correct
            Console.WriteLine("\tThe discount is correctly applied as 15% of the subtotal.");
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
        [When(@"I checkout with valid billing details")]
        public void Checkout()
        {
            //Navigate to the checkout page from the cart page
            _cartPage.ProceedToCheckout();
            //Complete the billing details form with valid data
            _checkoutPage.CompleteBillingDetails(
                firstName: "Jane",
                lastName: "Doe",
                address: "123 Street",
                city: "CityVille",
                postcode: "TF29FT",
                phoneNumber: "07123456789"
            );
        }

        //Step definition to select the payment method and order
        [When(@"I select ""(.*)"" then place order")]
        public void CompleteOrder(string paymentMethod)
        {
            //select the specified payment method
            _checkoutPage.SelectPaymentMethod(paymentMethod);
            //Place order using checkout page's PlaceOrder method
            _checkoutPage.PlaceOrder();
        }

        //Step definition to validate order number appearance in order history
        [Then(@"the order number should appear in my order history")]
        public void OrderValidation()
        {
            //Extract order number from the order confirmation page
            string orderNumber = _ordersPage.GetOrderNumber();

            //Navigate to orders page to verify the order number appears in order history
            _accountPage.NavigateToOrders();
            _ordersPage.VerifyOrderExists(orderNumber);
        }
    }
}
