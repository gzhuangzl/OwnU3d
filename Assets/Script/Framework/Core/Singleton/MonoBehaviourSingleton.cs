using System;
using UnityEngine;

namespace Framework
{
	//所有MonoBehaviour单例的基类
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T:new()
	{
		private static object instance;
		
		protected virtual void Awake(){
			if(instance == null){
				instance = this;
			}else{
				throw new SingletonException("重复创建了"+typeof(T)+"的对象");
			}
		}
		
		public static T Instance{
			get{
				return (T)instance;
			}
		}
		
		protected virtual void OnDestroy(){
			instance = null;
		}
	}
}

