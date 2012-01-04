using System;
using System.Collections.Generic;

namespace NEsper.Core
{
    public class MatchEventSubscrtiber
    {
        public bool HasEventFired { get; set; }

        public MatchEventSubscrtiber()
        {
            HasEventFired = false;
        }

        public void Update(IDictionary<string, object> rows)
        {
            Console.WriteLine("Match event fired");
            Console.WriteLine(rows);

            HasEventFired = true;
        }
    }
}
