using System;
using UnityEngine;

namespace Framework
{
	//所有MonoBehaviour单例的基类
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T:new()
	{
		private static object instance;
		
		protected void Awake(){
			instance = this;
		}
		
		public static T Instance{
			get{
				return (T)instance;
			}
		}
	}
}

