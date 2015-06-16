using System;

namespace Framework
{
	public delegate void TimerHandler(ITimer timer,bool isFinish);
	
	public interface ITimer : IEquatable<ITimer>
	{
		ulong GetId();
		bool IsStop();
		bool IsPause();
		void Startup();
		void Stop();
		void Pause();
		void Resume();
		void AddTickListener(TimerHandler handler);
		void RemoveTickListener(TimerHandler handler);
		void RemoveAllListener();
		void Dispose();
		
		void Tick(float deltaTime);
	}
}