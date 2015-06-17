using System;
namespace Framework
{
	public class FrameTimer : AbstractTimer
	{
		protected int frameInterval = 1;
		protected int frameCounter = 0;
		protected ulong totalCounter = 0;
		protected ulong passCounter = 0;
		
		public FrameTimer ():base(){
			
		}
		/// <summary>
		/// 创建一个帧定时器
		/// </summary>
		/// <param name="frameInterval">Frame interval.</param>
		/// <param name="totalTimes">Total times.</param>
		/// <param name="isAutoStartup">If set to <c>true</c> is auto startup.</param>
		public static FrameTimer Create(int frameInterval = 1,ulong totalCounter = ulong.MaxValue,bool isAutoStartup = true)
		{
			FrameTimer timer = ObjectPool.GetObject<FrameTimer>();
			timer.frameInterval = frameInterval;
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
			frameInterval = 1;
			frameCounter = 0;
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
			frameCounter++;
			if(frameCounter / frameInterval > 0){
				frameCounter -= frameInterval;
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
			frameInterval = 1;
			frameCounter = 0;
			totalCounter = 0;
			passCounter = 0;
			
			ObjectPool.DisposeObject(this);
		}
	}
}