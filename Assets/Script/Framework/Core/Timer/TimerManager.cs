using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace Framework
{
	public sealed class TimerManager : MonoBehaviourSingleton<TimerManager>
	{
		private Dictionary<ulong,ITimer> timerMaps;
		private List<ulong> sortTimerList;
		
		protected override void Awake (){
			base.Awake ();
			
			timerMaps = new Dictionary<ulong, ITimer>();
			sortTimerList = new List<ulong>();
		}
		
		void Update(){
			ITimer timer;
			foreach(ulong id in sortTimerList){
				timerMaps.TryGetValue(id,out timer);
				if(timer != null && !timer.IsStop() && !timer.IsPause()){
					timer.Tick(Time.deltaTime);
				}
			}
		}
		
		public void AddTimer(ITimer timer){
			if(timer != null){
				ITimer temp;
				timerMaps.TryGetValue(timer.GetId(),out temp);
				if(temp == null){
					timerMaps.Add(timer.GetId(),timer);
					sortTimerList.Add(timer.GetId());
				}
			}
		}
		
		public void RemoveTimer(ITimer timer){
			if(timer != null){
				RemoveTimer(timer.GetId());
			}
		}
		
		public void RemoveTimer(ulong timerId){
			ITimer timer;
			timerMaps.TryGetValue(timerId,out timer);
			if(timer != null){
				timerMaps.Remove(timerId);
				sortTimerList.Remove(timerId);
				timer.Dispose();
			}
		}
		
		public void RemoveAllTimer(){
			foreach(KeyValuePair<ulong,ITimer> pair in timerMaps){
				pair.Value.Dispose();
			}
			timerMaps.Clear();
			sortTimerList.Clear();
		}
		
		public void Clear(){
			RemoveAllTimer();
		}
	}
}