using OpenQA.Selenium;

namespace uk.co.nfocus.DiscountTestsRoshni.Pages

{
    //POM class to interact with the Shop Page elements
    public class ShopPage
    {
        private readonly IWebDriver _driver;


        public ShopPage(IWebDriver driver) => _driver = driver;

        //Page elements related to adding items to the cart
        private IWebElement AddToCartButton => _driver.FindElement(By.CssSelector("button[name='add-to-cart']"));

        // Navigate to shop page
        public void GoToShopPage()
        {
            _driver.FindElement(By.LinkText("Shop")).Click(); //Click shop link
            Console.WriteLine("Navigating to shop page"); //Log action
        }

        //Method to select a specific item and add it to the cart
        public void AddItemToCart(string itemName)
        {
            var item = _driver.FindElement(By.PartialLinkText(itemName)); //Find item by its partial link text
            item.Click(); //Click item
            AddToCartButton.Click(); //Click "Add to Cart" button
            Console.WriteLine($"\t{itemName} added to cart"); //Log
        }
    }
}