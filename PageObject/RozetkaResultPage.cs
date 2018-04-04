using OpenQA.Selenium;
using System.Collections.Generic;
using SeleniumExtras.PageObjects;

namespace PageObject
{
    public class RozetkaResultPage : RozetkaPage
    {

        public RozetkaResultPage(IWebDriver driver): base(driver) {
            base.url = "http://rozetka.com.ua/mobile-phones/c80003/preset=smartfon/";
        }

        public string XPathFilterTemplate = "//div[@class='filter-parametrs-i' and @param='producer']//i[text()='{0}']";

        public string XPathAppliedFilterTemplate = "//div[@class='filter-parametrs-i' and @param='producer']//i[text()='{0}']";

        [FindsBy(How = How.CssSelector, Using = "div.sort-view-container > a")]
        public IWebElement SortingDropdown;

        [FindsBy(How = How.CssSelector, Using = "div.sort-view-container li#filter_sortexpensive")]
        public IWebElement ExpensiveFilter;

        [FindsBy(How = How.XPath, Using = "//div[@class='sort-view-container']/a[contains(text(),'от дорогих к дешевым')]")]
        public IWebElement AppliedSortingLabel;

        [FindsBy(How = How.CssSelector, Using = "div.g-i-tile-i-box-desc div.g-price-uah")]
        public IList<IWebElement> ProductsPrices;

    }
}
