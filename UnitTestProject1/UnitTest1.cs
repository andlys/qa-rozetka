using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium.Interactions;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static string igWorkDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static IWebDriver driver;
        private static string url = "http://rozetka.com.ua/";
        private static bool usingChrome = true;

        [ClassInitialize]
        public static void InitTests(TestContext ctx) {
            if (usingChrome)
                driver = new ChromeDriver(igWorkDir, new ChromeOptions());
            else
                ;//driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript()); // TODO refactor, doesn't work
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
        }

        [TestCleanup]
        public void FinishSingleTest() {
            //nothing
        }

        [ClassCleanup]
        public static void FinishAllTests() {
            driver.Quit();
        }

        [TestMethod]
        public void CheckOpen() {
            HttpWebResponse response = (HttpWebResponse)WebRequest.CreateHttp(url).GetResponse();
            HttpStatusCode actualStatus = response.StatusCode;
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            Assert.AreEqual(expectedStatus, actualStatus);
        }
        
        [TestMethod]
        public void CheckSearch()
        {
            string query = "Hyundai";
            SearchFor(query);
            VerifyAllProductNamesContain(query);
            VerifyExistsButtonShowNext32();
        }

        private void SearchFor(string query)
        {
            IWebElement searchInput = driver.FindElement(By.XPath("//input[contains(@class, 'search')]"));
            searchInput.SendKeys(query);
            IWebElement searchBtn = driver.FindElement(By.CssSelector("button.js-rz-search-button"));
            searchBtn.Submit();
        }

        private void VerifyAllProductNamesContain(string query)
        {
            IReadOnlyCollection<IWebElement> products = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(drv => drv.FindElements(By.CssSelector("div.g-i-tile-i-box-desc > div.g-i-tile-i-title > a")));
            Assert.IsTrue(products.Count > 0);
            Assert.IsTrue(products.Count == 32);
            foreach (IWebElement product in products) {
                bool expectedTrue = product.Text.ToLower().Contains(query.ToLower());
                Assert.IsTrue(expectedTrue);
            }
        }

        private void VerifyExistsButtonShowNext32()
        {
            IWebElement showNext32 = driver.FindElement(By.CssSelector("span.g-i-more-link-text"));
            string expectedText = "Показать\r\nеще 32 товара",
                   actualText = showNext32.Text;
            Assert.AreEqual(expectedText, actualText);
        }

        [TestMethod]
        public void CheckSmartphoneFilters()
        {
            driver.Navigate().GoToUrl("http://rozetka.com.ua/mobile-phones/c80003/preset=smartfon/");
            ApplyFilter("Samsung");
            //VerifyAllProductNamesContain("Samsung", "Apple"); // TODO
            ApplyFilter("Apple");
        }

        private void ApplyFilter(string filterName)
        {
            string xpathTemplate = "//div[@class='filter-parametrs-i' and @param='producer']//i[text()='{0}']";
            By by = By.XPath(String.Format(xpathTemplate, filterName));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            IWebElement productFilter = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            Actions a = new Actions(driver);
            a.Click(driver.FindElement(by)).Build().Perform();
            By byFilterApplied = By.XPath(String.Format("//span[contains(@class, 'checkbox-active')]/i[text()='{0}']", filterName));
            wait.Until(ExpectedConditions.ElementIsVisible(byFilterApplied));
        }

        [TestMethod]
        public void CheckSmartphoneSortedByPriceDesc() {
            CheckSmartphoneFilters();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible((By.CssSelector("div.sort-view-container > a")))).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.sort-view-container li#filter_sortexpensive"))).Click();
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

        private int GetPriceFromString(string priceText) {
            return Int32.Parse(priceText.Remove(priceText.Length-4).Replace(" ", ""));
        }
    }
} 
