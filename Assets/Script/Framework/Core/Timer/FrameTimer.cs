using System;
namespace Framework
{
	public class FrameTimer : BaseTimer
	{
		protected int frameInterval = 1;
		protected int frameCount = 0;
		
		protected FrameTimer (int frameInterval):base(){
			this.frameInterval = frameInterval;
		}
		
		public static FrameTimer Create(int frameInterval = 1,bool isAutoStartup = true){
			FrameTimer timer = ObjectPool.GetObject<FrameTimer>(frameInterval);
			timer.frameInterval = frameInterval;
			
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
		}
		
		public override void Tick (float deltaTime)
		{
			base.Tick (deltaTime);
			frameCount++;
			if(frameCount / frameInterval > 0){
				frameCount -= frameInterval;
				if(handlers != null){
					handlers.Invoke(this,false);
				}
			}
		}
		
		public override void Dispose ()
		{
			base.Dispose ();
			frameInterval = 1;
			frameCount = 0;
			
			ObjectPool.DisposeObject(this);
		}
	}
}