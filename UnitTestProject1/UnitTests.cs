using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium.Interactions;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;
using System.Linq;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTests
    {
        private static string igWorkDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static IWebDriver driver;
        private static string url = "http://rozetka.com.ua/";
        private static bool usingChrome = true;

        [OneTimeSetUp]
        public static void InitTests() {
            if (usingChrome)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--lang=ru");
                driver = new ChromeDriver(igWorkDir, options);
            }
            else
            {
                //driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript()); // TODO refactor, doesn't work
            }
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void FinishSingleTest() {
            //nothing
        }

        [OneTimeTearDown]
        public static void FinishAllTests() {
            driver.Quit();
        }

        [Test]
        public void CheckOpenOld() {
            HttpWebResponse response = (HttpWebResponse)WebRequest.CreateHttp(url).GetResponse();
            HttpStatusCode actualStatus = response.StatusCode;
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            Assert.AreEqual(expectedStatus, actualStatus);
        }
        
        [Test]
        public void CheckSearchOld()
        {
            string query = "Hyundai";
            SearchFor(query);
            VerifyAllProductNamesContain(query);
            VerifyExistsButtonShowNext32();
        }

        [Test]
        public void CheckSearch() {

        }

        private void SearchFor(string query)
        {
            IWebElement searchInput = driver.FindElement(By.XPath("//input[contains(@class, 'search')]"));
            searchInput.SendKeys(query);
            IWebElement searchBtn = driver.FindElement(By.CssSelector("button.js-rz-search-button"));
            searchBtn.Submit();
        }

        private void VerifyAllProductNamesContain(params string[] queries)
        {
            IReadOnlyCollection<IWebElement> products = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("div.g-i-tile-i-box-desc > div.g-i-tile-i-title > a")));
            Assert.IsTrue(products.Count > 0, "Products count is {0}", products.Count);
            //Assert.IsTrue(products.Count == 32, "Products count is {0}", products.Count); // commented so as rozetka page shows 27 products for samsung
            // check if every product's name contains at least one query string
            foreach (IWebElement product in products) {
                bool foundOnceOrMore = false;
                foreach (string q in queries)
                {
                    bool found = product.Text.ToLower().Contains(q.ToLower());
                    foundOnceOrMore = foundOnceOrMore || found;
                }
                Assert.IsTrue(foundOnceOrMore, "Product name {0} is not suitable", product.Text);
            }
        }

        private void VerifyExistsButtonShowNext32()
        {
            IWebElement showNext32 = driver.FindElement(By.CssSelector("span.g-i-more-link-text"));
            string expectedText = "Показать\r\nеще 32 товара",
                   actualText = showNext32.Text;
            Assert.AreEqual(expectedText, actualText);
        }

        [Test]
        public void CheckSmartphoneFiltersOld()
        {
            driver.Navigate().GoToUrl("http://rozetka.com.ua/mobile-phones/c80003/preset=smartfon/");
            ApplyFilter("Samsung");
            VerifyAllProductNamesContain("Samsung");
            ApplyFilter("Apple");
            VerifyAllProductNamesContain("Samsung", "Apple");
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
            By byFilterApplied = By.XPath(String.Format("//a[@class='filter-active-i-link novisited sprite-side' and text()='{0}']", filterName));
            wait.Until(ExpectedConditions.ElementIsVisible(byFilterApplied));
        }

        [Test]
        public void CheckSmartphoneSortedByPriceDescOld() {
            CheckSmartphoneFiltersOld();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
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
            string digits = priceText.Where(c => Char.IsDigit(c)).Aggregate(String.Empty, (s1, s2) => s1 + s2);
            return Int32.Parse(digits);
        }
    }
} 
