using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Core
{
    public class MyDriver
    {
        private MyDriver() { }

        private static IWebDriver _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                _driver = DriverFactory.GetDriver("chrome"); // TODO test with firefox
            }
            return _driver;
        }
    }
}