using System;

namespace Framework
{
	//所有非MonoBehaviour单例的基类
	public abstract class Singleton<T> where T:new(){
		private static Object instance;
		private static readonly Object lockObject = new Object();
		
		protected Singleton(){
			if(instance == null){
				instance = this;
			}else{
				throw new SingletonException();
			}
		}
		
		public static T Instance{
			get{
				if(instance == null){
					lock(lockObject){
						if(instance == null){
							new T();
						}
					}
				}
				return (T)instance;
			}
		}
	}
}
