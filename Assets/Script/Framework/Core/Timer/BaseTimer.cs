
namespace Framework
{
	public abstract class BaseTimer : ITimer,IReusable
	{
		protected readonly ulong id;
		protected event TimerHandler handlers;
		protected bool isStop = true;
		protected bool isPause = false;
		protected float passTime = 0f;
		
		protected BaseTimer ()
		{
			id = GUID.GetNumber();
			
			if(handlers != null){
				handlers.Invoke(this,false);
			}
		}
		
		public ulong GetId()
		{
			return id;
		}

		public bool Equals (ITimer other)
		{
			if(other != null){
				return this.GetId() == other.GetId();
			}
			return false;
		}

		public virtual bool IsStop ()
		{
			return isStop;
		}

		public virtual bool IsPause ()
		{
			return isPause;
		}
		
		public virtual void Startup ()
		{
			isStop = false;
		}

		public virtual void Stop ()
		{
			isStop = true;
		}

		public virtual void Pause ()
		{
			isPause = true;
		}

		public virtual void Resume ()
		{
			isPause = false;
		}
		
		public virtual void Reset()
		{
			RemoveAllListener();
			Stop();
			isPause = false;
			passTime = 0;
		}

		public virtual void Tick (float deltaTime)
		{
			if(IsPause() || IsStop())
			{
				throw new TimerException(this.GetType().ToString() + "已经暂时或停止，不允许触发Tick");
			}
		}
		
		public virtual void AddTickListener(TimerHandler handler)
		{
			handlers += handler;
		}
		
		public virtual void RemoveTickListener(TimerHandler handler)
		{
			handlers -= handler;
		}
		
		public virtual void RemoveAllListener()
		{
			handlers = null;
		}
		
		protected void DispatcherTick(bool isFinish)
		{
			if(handlers != null){
				handlers.Invoke(this,isFinish);
			}
		}
		
		public virtual void Dispose()
		{
			RemoveAllListener();
			Stop();
			isPause = false;
			passTime = 0;
		}
	}
}