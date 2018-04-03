using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRozetka.pages;
using static TestRozetka.MyDriver;

namespace TestRozetka.steps
{
    class RozetkaSteps
    {
        private RozetkaPage page;

        public RozetkaSteps(RozetkaPage page) {
            this.page = page;
        }

        public void Open() {
            GetDriver().Navigate().GoToUrl(page.Url);
        }

    }
}
