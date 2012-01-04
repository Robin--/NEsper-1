using System;
using com.espertech.esper.client;
using com.espertech.esper.client.time;

namespace NEsper.Core
{
    public class NesperAdapter
    {
        public MatchEventSubscrtiber Subscriber { get; set; }
        internal EPServiceProvider Engine { get; private set; }

        public NesperAdapter()
        {
            //This call internally depend on log4net, 
            //will throw an error if log4net cannot be loaded 
            EPServiceProviderManager.PurgeDefaultProvider();

            //config
            var configuration = new Configuration();
            configuration.AddEventType("TestDummy", typeof(TestDummy).FullName);
            configuration.EngineDefaults.Threading.IsInternalTimerEnabled = false;
            configuration.EngineDefaults.Logging.IsEnableExecutionDebug = false;
            configuration.EngineDefaults.Logging.IsEnableTimerDebug = false;

            //engine
            Engine = EPServiceProviderManager.GetDefaultProvider(configuration);
            Engine.EPRuntime.SendEvent(new TimerControlEvent(TimerControlEvent.ClockTypeEnum.CLOCK_EXTERNAL));
            Engine.Initialize();
            Engine.EPRuntime.UnmatchedEvent += OnUnmatchedEvent;
        }

        public void AddStatementFromRegExp(string regExp)
        {
            const string pattern = "every (Id123=TestDummy(Value regexp '{0}'))";
            string formattedPattern = String.Format(pattern, regExp);
            EPStatement statement = Engine.EPAdministrator.CreatePattern(formattedPattern);
            
            //this is subscription
            Subscriber = new MatchEventSubscrtiber();
            statement.Subscriber = Subscriber;
        }
        
        internal void OnUnmatchedEvent(object sender, UnmatchedEventArgs e)
        {
            Console.WriteLine(@"Unmatched event");
            Console.WriteLine(e.Event);
        }

        public void SendEvent(object someEvent)
        {
            Engine.EPRuntime.SendEvent(someEvent);
        }
    }
}
