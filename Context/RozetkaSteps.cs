using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using PageObject;
using static Core.MyDriver;
using WebDriverWait = OpenQA.Selenium.Support.UI.WebDriverWait;


namespace Context
{
    public class RozetkaSteps
    {
        protected RozetkaPage page;

        public RozetkaSteps(RozetkaPage page) {
            this.page = page;
        }

        public void Open() {
            GetDriver().Navigate().GoToUrl(page.Url);
        }

        public void SearchFor(string query)
        {
            page.SearchInput.SendKeys(query);
            page.SearchBtn.Submit();
        }

        public void VerifyAllProductNamesContain(params string[] queries)
        {
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => page.Products.All(elt => elt.Displayed));
            //wait.Until(d => page.ProductsContainer.Displayed);
            Assert.IsTrue(page.Products.Count > 0, "Products count is {0}", page.Products.Count);
            foreach (IWebElement product in page.Products)
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
            string expectedText = "Показать\r\nеще 32 товара",
                   actualText = page.BtnNext32.Text;
            Assert.AreEqual(expectedText, actualText);
        }

        public virtual void ApplyFilter(String arg) { }

        public virtual void MakeSortedDesc() { }

        public virtual void CheckSortedProductsDesc() { }

    }
}
