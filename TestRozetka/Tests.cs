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
            RozetkaSteps steps = new RozetkaSteps(page);
            steps.Open();
            string actualTitle = GetDriver().Title,
                 expectedBeginningOfTitle = "Интернет-магазин ROZETKA";
            Assert.IsTrue(actualTitle.StartsWith(expectedBeginningOfTitle));
        }

        [Test]
        [Category("rozetka")]
        public void CheckSearch()
        {
            string query = "Hyundai";
            RozetkaPage page = PageFactory.InitElements<RozetkaPage>(GetDriver());
            RozetkaSteps steps = new RozetkaSteps(page);
            steps.Open();
            steps.SearchFor(query);
            steps.VerifyAllProductNamesContain(query);
            steps.VerifyExistsButtonShowNext32();
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFiltered()
        {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());
            RozetkaSteps steps = new RozetkaResultSteps(page);
            steps.Open();
            steps.ApplyFilter("Samsung");
            steps.VerifyAllProductNamesContain("Samsung");
            steps.ApplyFilter("Apple");
            steps.VerifyAllProductNamesContain("Samsung", "Apple");
        }

        [Test]
        [Category("rozetka")]
        public void CheckSmartphonesFilteredAndSortedByPriceDesc() {
            RozetkaResultPage page = PageFactory.InitElements<RozetkaResultPage>(GetDriver());
            RozetkaSteps steps = new RozetkaResultSteps(page);
            steps.Open();

            steps.ApplyFilter("Samsung");
            steps.VerifyAllProductNamesContain("Samsung");
            steps.ApplyFilter("Apple");
            steps.VerifyAllProductNamesContain("Samsung", "Apple");

            steps.MakeSortedDesc();
            steps.CheckSortedProductsDesc();
        }
        
    }
} 
