using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using TestRozetka.pages;
using static TestRozetka.MyDriver;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;



namespace TestRozetka.steps
{
    class RozetkaResultSteps : RozetkaSteps
    {
        private RozetkaResultSteps(RozetkaPage page) : base(page)
        {
        }

        public RozetkaResultSteps(RozetkaResultPage page) : base(page)
        {
        }

        public RozetkaResultPage Page { get { return page as RozetkaResultPage; } }

        private By ByAppliedFilter(string filterName)
        {
            By by = By.XPath(String.Format(Page.XPathAppliedFilterTemplate, filterName));
            return by;
        }

        private By ByFilter(string filterName)
        {
            By by = By.XPath(String.Format(Page.XPathFilterTemplate, filterName));
            return by;
        }

        public new void ApplyFilter(string filterName)
        {
            By byFilter = ByFilter(filterName);
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            IWebElement productFilter = wait.Until(ExpectedConditions.ElementToBeClickable(byFilter));
            //Actions a = new Actions(driver);
            //a.Click(driver.FindElement(by)).Build().Perform();
            GetDriver().FindElement(byFilter).Click();
            By byFilterApplied = ByAppliedFilter(filterName);
            wait.Until(ExpectedConditions.ElementIsVisible(byFilterApplied));
        }

        public new void MakeSortedDesc()
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => Page.SortingDropdown.Displayed);
            Page.SortingDropdown.Click();
            wait.Until(d => Page.ExpensiveFilter.Displayed);
            Page.ExpensiveFilter.Click();
            wait.Until(d => Page.AppliedSortingLabel.Displayed);
        }

        public new void CheckSortedProductsDesc()
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            wait.Until(d => Page.ProductsPrices.All(elt => elt.Displayed));
            Assert.IsTrue(Page.ProductsPrices.Count > 0);
            var enumerator = Page.ProductsPrices.GetEnumerator();
            enumerator.MoveNext();
            int prevPrice = GetPriceFromString(enumerator.Current.Text);
            foreach (IWebElement product in Page.ProductsPrices)
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
