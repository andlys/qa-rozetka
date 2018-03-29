using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.pages
{
    class RozetkaHomePage
    {
        private IWebDriver driver;
        public RozetkaHomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        [FindsBy(How = How.XPath, Using = "")]
        private IWebElement elt { get; set; }
    }
}
