using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;

namespace UnitTestProject1.pages
{
    class RozetkaResultPage : RozetkaHomePage
    {
        public RozetkaResultPage(IWebDriver driver): base(driver) { }

        public void ApplyFilter(string filterName)
        {
            string xpathTemplate = "//div[@class='filter-parametrs-i' and @param='producer']//i[text()='{0}']";
            By by = By.XPath(String.Format(xpathTemplate, filterName));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            IWebElement productFilter = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            Actions a = new Actions(driver);
            a.Click(driver.FindElement(by)).Build().Perform();
            By byFilterApplied = By.XPath(String.Format("//a[@class='filter-active-i-link novisited sprite-side' and text()='{0}']", filterName));
            wait.Until(ExpectedConditions.ElementIsVisible(byFilterApplied));
        }

        public void MakeSortedDesc() {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(ExpectedConditions.ElementIsVisible((By.CssSelector("div.sort-view-container > a")))).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.sort-view-container li#filter_sortexpensive"))).Click();
        }

        public void CheckSortedProductsDesc() {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='sort-view-container']/a[contains(text(),'от дорогих к дешевым')]")));
            IReadOnlyCollection<IWebElement> products = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("div.g-i-tile-i-box-desc div.g-price-uah")));
            Assert.IsTrue(products.Count > 0);
            var enumerator = products.GetEnumerator();
            enumerator.MoveNext();
            int prevPrice = GetPriceFromString(enumerator.Current.Text);
            foreach (IWebElement product in products)
            {
                int currPrice = GetPriceFromString(product.Text);
                Assert.IsTrue(prevPrice >= currPrice, "Price {0} is lower than {1}", prevPrice, currPrice);
                prevPrice = currPrice;
            }
        }

        private int GetPriceFromString(string priceText)
        {
            string digits = priceText.Where(c => Char.IsDigit(c)).Aggregate(String.Empty, (s1, s2) => s1 + s2);
            return Int32.Parse(digits);
        }
    }
}
