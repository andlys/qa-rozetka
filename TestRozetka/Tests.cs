using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using NUnit.Framework;
using TestRozetka.pages;
using SeleniumExtras.PageObjects;
using static TestRozetka.MyDriver;
using TestRozetka.steps;

namespace TestRozetka
{
    [TestFixture]
    public class Tests
    {

        [OneTimeSetUp]
        public static void InitTests() {
            GetDriver().Manage().Window.Maximize();
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
            RozetkaPage page = PageFactory.InitElements<RozetkaPage>(GetDriver());
            HttpWebResponse response = (HttpWebResponse)WebRequest.CreateHttp(page.Url).GetResponse();
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
            RozetkaSteps steps = new RozetkaSteps(page);
            steps.Open();
            page.SearchFor(query);
            page.VerifyAllProductNamesContain(query);
            page.VerifyExistsButtonShowNext32();
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFiltered()
        {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());
            RozetkaSteps steps = new RozetkaSteps(page);
            steps.Open();
            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFilteredAndSortedByPriceDesc() {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());
            RozetkaSteps steps = new RozetkaSteps(page);
            steps.Open();

            page.ApplyFilter("Samsung");
            page.VerifyAllProductNamesContain("Samsung");
            page.ApplyFilter("Apple");
            page.VerifyAllProductNamesContain("Samsung", "Apple");
            
            page.MakeSortedDesc();
            page.CheckSortedProductsDesc();
        }
        
    }
} 
