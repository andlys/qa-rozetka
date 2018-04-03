using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using NUnit.Framework;
using TestRozetka.pages;
using SeleniumExtras.PageObjects;
using static TestRozetka.MyDriver;

namespace TestRozetka
{
    [TestFixture]
    public class Tests
    {
        private static string url = "http://rozetka.com.ua/",
            smartphonesUrl = "http://rozetka.com.ua/mobile-phones/c80003/preset=smartfon/";

        [OneTimeSetUp]
        public static void InitTests() {
            GetDriver().Manage().Window.Maximize();
            GetDriver().Navigate().GoToUrl(url);
        }

        [TearDown]
        public void FinishSingleTest() {
            //nothing
        }

        [OneTimeTearDown]
        public static void FinishAllTests() {
            GetDriver().Quit();
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
            RozetkaPage page = PageFactory.InitElements<RozetkaPage>(GetDriver());
            page.SearchFor(query);
            page.VerifyAllProductNamesContain(query);
            page.VerifyExistsButtonShowNext32();
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFiltered()
        {
            GetDriver().Navigate().GoToUrl(smartphonesUrl);
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());
            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFilteredAndSortedByPriceDesc() {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());

            GetDriver().Navigate().GoToUrl(smartphonesUrl);
            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");

            page.MakeSortedDesc();
            page.CheckSortedProductsDesc();
        }


    }
} 
