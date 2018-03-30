using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using NUnit.Framework;
using TestRozetka.pages;
using SeleniumExtras.PageObjects;

namespace TestRozetka
{
    [TestFixture]
    public class Tests
    {
        private static string igWorkDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static IWebDriver driver;
        private static string url = "http://rozetka.com.ua/",
            smartphonesUrl = "http://rozetka.com.ua/mobile-phones/c80003/preset=smartfon/";
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
        [Category("rozetka")]
        public void CheckOpen() {
            HttpWebResponse response = (HttpWebResponse)WebRequest.CreateHttp(url).GetResponse();
            HttpStatusCode actualStatus = response.StatusCode;
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            Assert.AreEqual(expectedStatus, actualStatus);
        }
        
        [Test]
        [Category("rozetka")]
        public void CheckSearch()
        {
            string query = "Hyundai";
            RozetkaHomePage page = PageFactory.InitElements<RozetkaHomePage>(driver);
            page.SearchFor(query);
            page.VerifyAllProductNamesContain(query);
            page.VerifyExistsButtonShowNext32();
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFiltered()
        {
            driver.Navigate().GoToUrl(smartphonesUrl);
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(driver);
            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFilteredAndSortedByPriceDesc() {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(driver);

            driver.Navigate().GoToUrl(smartphonesUrl);
            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");

            page.MakeSortedDesc();
            page.CheckSortedProductsDesc();
        }


    }
} 
