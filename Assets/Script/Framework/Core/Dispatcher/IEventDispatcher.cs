using System;
namespace Framework
{
	public enum EventListenerTimes{
		Once,
		Forever
	}
	
	public enum EventListenerPriority{
		Low,
		Normal,
		High,
		Top
	}
	
	public interface IEventDispatcher
	{
		void AddListener(int eventType,EventListenerFunction function,EventListenerPriority priority = EventListenerPriority.Normal,EventListenerTimes times = EventListenerTimes.Forever);
		
		void RemoveListener(int eventType,EventListenerFunction funtion);
		
		void RemoveAllListener(int eventType);
		
		void Dispatcher(int eventType,object eventData = null);
		
		bool Has(int eventType,EventListenerFunction funtion);
		
		void Clear();
	}
}