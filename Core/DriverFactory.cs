using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Core
{
    public class DriverFactory
    {
        public static IWebDriver GetDriver(string driverName) {
            if ("chrome".Equals(driverName))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--lang=ru");
                return new ChromeDriver(options);
            }
            else if ("firefox".Equals(driverName))
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("--lang=ru");
                return new FirefoxDriver(options);
            }
            else
                return null;
        }
    }
}
