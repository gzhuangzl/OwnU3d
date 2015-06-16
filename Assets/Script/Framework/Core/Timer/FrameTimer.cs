using System;
namespace Framework
{
	public class FrameTimer : BaseTimer
	{
		protected int frameInterval = 1;
		protected int frameCount = 0;
		protected ulong totalTimes = 0;
		protected ulong passTimes = 0;
		
		public FrameTimer (int frameInterval,ulong totalTimes):base(){
			this.frameInterval = frameInterval;
			this.totalTimes = totalTimes;
		}
		
		public static FrameTimer Create(int frameInterval = 1,ulong totalTimes = ulong.MaxValue,bool isAutoStartup = true){
			FrameTimer timer = ObjectPool.GetObject<FrameTimer>(frameInterval,totalTimes);
			timer.frameInterval = frameInterval;
			timer.totalTimes = totalTimes;
			
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
			frameInterval = 1;
			frameCount = 0;
			totalTimes = 0;
			passTimes = 0;
		}
		
		public override void Tick (float deltaTime)
		{
			base.Tick (deltaTime);
			frameCount++;
			if(frameCount / frameInterval > 0){
				frameCount -= frameInterval;
				if(++passTimes >= totalTimes){
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
			frameInterval = 1;
			frameCount = 0;
			totalTimes = 0;
			passTimes = 0;
			
			ObjectPool.DisposeObject(this);
		}
	}
}