using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace Framework
{
	public sealed class TimerManager : MonoBehaviourSingleton<TimerManager>
	{
		private Dictionary<ulong,ITimer> timerMaps;
		private List<ulong> timerNaturalOrderList;
		private List<ulong> nextFrameRemoveTimerList;
		
		protected override void Awake (){
			base.Awake ();
			
			timerMaps = new Dictionary<ulong, ITimer>();
			timerNaturalOrderList = new List<ulong>();
			nextFrameRemoveTimerList = new List<ulong>();
		}
		
		void Update(){
			ITimer timer;
			foreach(ulong id in timerNaturalOrderList){
				timerMaps.TryGetValue(id,out timer);
				if(timer != null && !timer.IsStop() && !timer.IsPause()){
					timer.Tick(Time.deltaTime);
				}
			}
		}
		
		public void AddTimer(ITimer timer){
			if(timer != null){
				//在移除列表则从中移出
				int index = nextFrameRemoveTimerList.IndexOf(timer.GetId());
				if(index >= 0){
					nextFrameRemoveTimerList.RemoveAt(index);
				}
				ITimer temp;
				timerMaps.TryGetValue(timer.GetId(),out temp);
				if(temp == null){
					timerMaps.Add(timer.GetId(),timer);
					timerNaturalOrderList.Add(timer.GetId());
				}
			}
		}
		
		public void RemoveTimer(ITimer timer){
			if(timer != null){
				nextFrameRemoveTimerList.Add(timer.GetId());
			}
		}
		
		public void RemoveTimer(ulong timerId){
			nextFrameRemoveTimerList.Add(timerId);
		}
		
		private void RemoveTimer(){
			ITimer timer;
			foreach(ulong timerId in nextFrameRemoveTimerList){
				timerMaps.TryGetValue(timerId,out timer);
				if(timer != null){
					timerMaps.Remove(timerId);
					timerNaturalOrderList.Remove(timerId);
					timer.Dispose();
				}
			}
			
			nextFrameRemoveTimerList.Clear();
		}
		
		public void RemoveAllTimer(){
			foreach(KeyValuePair<ulong,ITimer> pair in timerMaps){
				pair.Value.Dispose();
			}
			timerMaps.Clear();
			timerNaturalOrderList.Clear();
			nextFrameRemoveTimerList.Clear();
		}
		
		public void Clear(){
			RemoveAllTimer();
		}
	}
}