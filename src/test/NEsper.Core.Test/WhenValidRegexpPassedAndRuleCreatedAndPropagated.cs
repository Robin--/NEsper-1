using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace NEsper.Core.Test
{
    //Match any IP address
    [TestFixture(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", "127.0.0.5")] 
    public class WhenValidRegexpPassedAndRuleCreatedAndPropagated
    {
        private NesperAdapter nesper;

        //Setup
        public WhenValidRegexpPassedAndRuleCreatedAndPropagated(string regexp, string input)
        {
            //check it is valid regexp in .NET
            var r = new Regex(regexp);
            var match = r.Match(input);
            Assert.IsTrue(match.Success, "Regexp validation failed in .NET");

            //create and start engine
            nesper = new NesperAdapter();

            //Add a rule, this fails with a correct regexp and a matching input
            //PROBLEM IS HERE 
            nesper.AddStatementFromRegExp(regexp);
            //PROBLEM IS HERE 

            //This works, but it is just input self-matching
            //nesper.AddStatementFromRegExp(input);

            var oneEvent = new TestDummy
            {
                Value = input
            };

            nesper.SendEvent(oneEvent);
        }

        [Test]
        public void ThenNesperFiresMatchEvent()
        {
            //wait till nesper process the event
            Thread.Sleep(100);

            //Check if subscriber has received the event
            Assert.IsTrue(nesper.Subscriber.HasEventFired);
        }
    }
}
