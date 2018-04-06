using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace BddTests
{
    [Binding]
    public class CalculatorSteps
    {
        Func<int, int, int> operation;
        Queue<int> entered = new Queue<int>();

        [Given(@"I have entered (.*) into the calculator")]
        [Given(@"I've entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            entered.Enqueue(p0);
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            operation = (acc, n) => acc + n;
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int expected)
        {
            int res = 0;
            res = entered.Aggregate(operation);
            Assert.AreEqual(expected, res);
            entered.Clear();
        }

        [Given(@"I enter ""(.*)"" as my mood")]
        public void GivenIEnterAsMyMood(string p0)
        {
            Assert.AreEqual("Feel great", p0);
        }
    }
}
