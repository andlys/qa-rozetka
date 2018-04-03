using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;


namespace TestRozetka.pages
{
    class RozetkaPage
    {
        protected IWebDriver driver;
        protected string url;
        public string Url { get { return url; } }

        public RozetkaPage(IWebDriver driver)
        {
            this.driver = driver;
            this.url = "http://rozetka.com.ua/";
        }

        [FindsBy(How = How.XPath, Using = "//input[contains(@class, 'search')]")]
        public IWebElement SearchInput;

        [FindsBy(How = How.CssSelector, Using = "button.js-rz-search-button")]
        public IWebElement SearchBtn;

        [FindsBy(How = How.CssSelector, Using = "div.g-i-tile-i-box-desc > div.g-i-tile-i-title > a")]
        public IList<IWebElement> Products;

        [FindsBy(How = How.CssSelector, Using = "span.g-i-more-link-text")]
        public IWebElement BtnNext32;

    }
}
