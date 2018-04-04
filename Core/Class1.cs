using System;


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
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--lang=ru");
                _driver = new ChromeDriver(options);
            }
            return _driver;
        }
    }
}