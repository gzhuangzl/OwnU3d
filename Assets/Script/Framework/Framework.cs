using UnityEngine;

namespace Framework
{
	public sealed class Framework
	{
		private Framework ()
		{
		}
		
		/// <summary>
		/// 启动整个框架，程序入口调用，使用框架去初始化它相关联的一些数据
		/// </summary>
		public static void Startup(GameObject mainGameObject = null){
			if(mainGameObject == null){
				mainGameObject = new GameObject();
				mainGameObject.transform.SetParent(null);
			}
			//挂靠一些依赖GameObject的框架类
			mainGameObject.AddComponent<TimerManager>();
		}
	}
}