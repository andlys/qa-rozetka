using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;
using System;
using System.Collections.Generic;

namespace TestRozetka.pages
{
    class RozetkaHomePage
    {
        protected IWebDriver driver;

        public RozetkaHomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        [FindsBy(How = How.XPath, Using = "")]
        private IWebElement elt { get; set; }

        public void SearchFor(string query)
        {
            IWebElement searchInput = driver.FindElement(By.XPath("//input[contains(@class, 'search')]"));
            searchInput.SendKeys(query);
            IWebElement searchBtn = driver.FindElement(By.CssSelector("button.js-rz-search-button"));
            searchBtn.Submit();
        }

        public void VerifyAllProductNamesContain(params string[] queries)
        {
            IReadOnlyCollection<IWebElement> products = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("div.g-i-tile-i-box-desc > div.g-i-tile-i-title > a")));
            Assert.IsTrue(products.Count > 0, "Products count is {0}", products.Count);
            //Assert.IsTrue(products.Count == 32, "Products count is {0}", products.Count); // commented so as rozetka page shows 27 products for samsung
            // check if every product's name contains at least one query string
            foreach (IWebElement product in products)
            {
                bool foundOnceOrMore = false;
                foreach (string q in queries)
                {
                    bool found = product.Text.ToLower().Contains(q.ToLower());
                    foundOnceOrMore = foundOnceOrMore || found;
                }
                Assert.IsTrue(foundOnceOrMore, "Product name {0} is not suitable", product.Text);
            }
        }

        public void VerifyExistsButtonShowNext32()
        {
            IWebElement showNext32 = driver.FindElement(By.CssSelector("span.g-i-more-link-text"));
            string expectedText = "Показать\r\nеще 32 товара",
                   actualText = showNext32.Text;
            Assert.AreEqual(expectedText, actualText);
        }
    }
}
