using System;
using System.Collections.Generic;
namespace Framework
{
	public sealed class EventDispatcher : Singleton<EventDispatcher>,IEventDispatcher
	{
		private Dictionary<int,EventListener> eventListenerList;
		
		public EventDispatcher(){
			eventListenerList = new Dictionary<int, EventListener>();
		}
		
		public void AddListener(int eventType,EventListenerFunction function,EventListenerPriority priority = EventListenerPriority.Normal,EventListenerTimes times = EventListenerTimes.Forever){
			EventListener listener = GetEventListener(eventType,true);
			listener.AddListener(function,priority,times);
		}
		
		public void RemoveListener(int eventType,EventListenerFunction funtion){
			EventListener listener = GetEventListener(eventType);
			if(listener != null){
				listener.RemoveListener(funtion);
				if(listener.IsEmpty()){
					listener.Clear();
					eventListenerList.Remove(eventType);
				}
			}
		}
		
		public void RemoveAllListener(int eventType){
			EventListener listener = GetEventListener(eventType);
			if(listener != null){
				listener.Clear();
				eventListenerList.Remove(eventType);
			}
		}
		
		public void Dispatcher(int eventType,object eventData = null){
			EventListener listener = GetEventListener(eventType);
			if(listener != null){
				listener.Dispatcher(eventData);
			}
		}

		public bool Has (int eventType, EventListenerFunction funtion)
		{
			EventListener listener = GetEventListener(eventType);
			if(listener != null){
				return listener.Has(funtion);
			}
			return false;
		}
		
		public void Clear ()
		{
			foreach(KeyValuePair<int,EventListener> pair in eventListenerList){
				pair.Value.Clear();
			}
			eventListenerList.Clear();
		}
		
		private EventListener GetEventListener(int eventType,bool isAutoCreate = false){
			EventListener info;
			eventListenerList.TryGetValue(eventType,out info);
			if(info == null && isAutoCreate){
				info = new EventListener(eventType);
				eventListenerList.Add(eventType,info);
			}
			return info;
		}
		/*以下为具体了逻辑实现---------------------------------------------------------------*/
		//单个类型的所有侦听器的封装类
		private class EventListener{
			private int eventType;
			private int count = 0;
			private Dictionary<EventListenerPriority,List<EventListenerInfo>> listenerList;
			
			public EventListener(int eventType){
				this.eventType = eventType;
				listenerList = new Dictionary<EventListenerPriority, List<EventListenerInfo>>();
			}
			
			public void AddListener(EventListenerFunction funtion,EventListenerPriority priortity,EventListenerTimes times){
				List<EventListenerInfo> list =  GetPriorityList(priortity,true);
				EventListenerInfo info = new EventListenerInfo(funtion,times);
				if(list.IndexOf(info) < 0){
					list.Add(info);
					count++;
				}
			}
			
			public void RemoveListener(EventListenerFunction funtion){
				EventListenerInfo info = new EventListenerInfo(funtion,EventListenerTimes.Once);
				foreach(KeyValuePair<EventListenerPriority,List<EventListenerInfo>> pair in listenerList){
					int index = pair.Value.IndexOf(info);
					if(index >= 0){
						pair.Value.RemoveAt(index);
						count--;
						break;
					}
				}
			}
			
			public bool Has(EventListenerFunction function){
				EventListenerInfo info = new EventListenerInfo(function,EventListenerTimes.Once);
				foreach(KeyValuePair<EventListenerPriority,List<EventListenerInfo>> pair in listenerList){
					int index = pair.Value.IndexOf(info);
					if(index >= 0){
						return true;
					}
				}
				return false;
			}
			
			public void Dispatcher(object eventData){
				Dispatcher(EventListenerPriority.Top,eventData);
				Dispatcher(EventListenerPriority.High,eventData);
				Dispatcher(EventListenerPriority.Normal,eventData);
				Dispatcher(EventListenerPriority.Low,eventData);
			}
			
			private void Dispatcher(EventListenerPriority priority,object eventData){
				List<EventListenerInfo> list = GetPriorityList(priority);
				if(list != null){
					List<int> finishedList = new List<int>();
					for(int i = 0;i<list.Count;i++){
						EventListenerInfo info = list[i];
						info.Dispatcher(eventType,eventData);
						if(info.IsFinished()){
							finishedList.Add(i);
						}
					}
					//移除完成的
					for(int i = finishedList.Count - 1;i>=0;i--){
						int index = finishedList[i];
						list[index].Clear();
						list.RemoveAt(index);
						count--;
					}
					finishedList.Clear();
				}
			}
			
			public bool IsEmpty(){
				return count == 0;
			}
			
			public void Clear(){
				foreach(KeyValuePair<EventListenerPriority,List<EventListenerInfo>> pair in listenerList){
					foreach(EventListenerInfo info in pair.Value){
						info.Clear();
					}
					pair.Value.Clear();
				}
				listenerList.Clear();
				count = 0;
			}
			
			private List<EventListenerInfo> GetPriorityList(EventListenerPriority priority,bool isAutoCreate = false){
				List<EventListenerInfo> list;
				listenerList.TryGetValue(priority,out list);
				if(list == null && isAutoCreate){
					list = new List<EventListenerInfo>();
					listenerList.Add(priority,list);
				}
				return list;
			}
		}
		
		//单个侦听器的数据封装
		private class EventListenerInfo : IEquatable<EventListenerInfo>{
			private EventListenerFunction function;
			private EventListenerTimes times;
			private int currentTimes = 0;
			
			public EventListenerInfo(EventListenerFunction function,EventListenerTimes times){
				this.function = function;
				this.times = times;
			}
			
			public void Dispatcher(int eventType,object eventData){
				if(!IsFinished()){
					if(!(times == EventListenerTimes.Forever)){
						currentTimes ++;
					}
					if(function != null){
						function(eventType,eventData);
					}
				}
			}
			
			public bool IsFinished(){
				if(times == EventListenerTimes.Forever){
					return false;
				}else if(times == EventListenerTimes.Once){
					return this.currentTimes >= 1;
				}else{
					return false;
				}
			}
			
			public bool Equals (EventListenerInfo other)
			{
				if(other != null){
					return other.function == this.function;
				}
				return false;
			}
			
			public void Clear(){
				function = null;
			}
		}
	}
}

