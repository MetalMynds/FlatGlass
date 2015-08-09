using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace MetalMynds.Utilities
{
    public class AutoResetEvents
    {
        public event EventHandler<EventSetEventArgs> EventSet;

        protected Dictionary<String, AutoResetEvent> BaseEvents = new Dictionary<string, AutoResetEvent>();

        public AutoResetEvents()
        {
        }

        public virtual void SignalEvent(String Identifier)
        {
            BaseEvents[Identifier].Set();
        }

        public virtual AutoResetEvent AddEvent(String Identifier, Boolean InitialStateSignaled)
        {
            AutoResetEvent newAutoReset = new AutoResetEvent(InitialStateSignaled);

            BaseEvents.Add(Identifier, newAutoReset);

            return BaseEvents[Identifier];
        }

        public virtual void Clear()
        {
            foreach (AutoResetEvent autoEvent in BaseEvents.Values)
            {
                autoEvent.Close();
            }
        }

        public Dictionary<String, AutoResetEvent> Events { get { return BaseEvents;  } } 

        protected virtual void OnEventSet(String Identifier) 
        {
            if (EventSet != null)
            {
                EventSet.Invoke(this,new EventSetEventArgs(Identifier));
            }
        }

    }

    public class EventSetEventArgs : EventArgs 
    {

        protected String BaseIdentifier;

        public EventSetEventArgs(String Identifier) 
        {
            BaseIdentifier = Identifier;
        }
        
        public String Identifier { get { return BaseIdentifier; } } 

    }



}
