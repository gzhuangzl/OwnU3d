using UnityEngine;

namespace Framework
{
	public class SecondTimer : AbstractTimer {
		protected float timeInterval = 0;
		protected float totalCounter = 0;
		protected float passCounter = 0;
		
		public SecondTimer():base(){
		
		}
		
		public static SecondTimer Create(float secondInterval = 1f,ulong totalCounter = ulong.MaxValue,bool isAutoStartup = true)
		{
			SecondTimer timer = ObjectPool.GetObject<SecondTimer>();
			timer.timeInterval = secondInterval;
			timer.totalCounter = totalCounter;
			
			if(isAutoStartup){
				timer.Startup();
			}
			return timer;
		}
		
		public override void Startup ()
		{
			base.Startup ();
			
			TimerManager.Instance.AddTimer(this);
		}
		
		public override void Stop ()
		{
			base.Stop ();
			
			TimerManager.Instance.RemoveTimer(this.GetId());
		}
		
		public override void Reset ()
		{
			base.Reset ();
			timeInterval = 1;
			totalCounter = 0;
			passCounter = 0;
		}
		
		public override void Tick (float deltaTime)
		{
			if(totalCounter == 0){
				Dispose();
				return;
			}
			base.Tick (deltaTime);
			if(passTime / timeInterval > 0){
				passTime -= timeInterval;
				if(++passCounter >= totalCounter){
					DispatcherTick(true);
					Dispose();
				}else{
					DispatcherTick(false);
				}
			}
		}
		
		public override void Dispose ()
		{
			base.Dispose ();
			timeInterval = 1;
			totalCounter = 0;
			passCounter = 0;
			
			ObjectPool.DisposeObject(this);
		}
	}
}