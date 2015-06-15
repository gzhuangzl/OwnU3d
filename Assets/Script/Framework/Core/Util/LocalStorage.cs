using System;
using UnityEngine;

namespace Framework
{
	public sealed class LocalStorage
	{
		private static String rootId = "ROOT_ID";
		//WebPlayer 存储限制是1M，其它平台无限制
		private LocalStorage ()
		{
		}
		/// <summary>
		/// 本地存储的根的ID，不同的根下面可以拥有相同的KEY，可用于解决不同用户的偏好设置冲突问题等
		/// 可自行根据情况扩展多个不同的RootId避免相同KEY冲突，一般在游戏进入时初始化RootId,推荐使用
		/// 登录用户的ID进行初始化
		/// </summary>
		/// <value>The root identifier.</value>
		public static String RootId{
			get{return rootId;}
			set{
				if(value != null){
					rootId = value;
				}
			}
		}
		
		public static void SetInt(String key,int value){
			PlayerPrefs.SetInt(GetKey(key),value);
		}
		
		public static int GetInt(String key){
			return PlayerPrefs.GetInt(GetKey(key));
		}
		
		public static void SetFloat(String key,float value){
			PlayerPrefs.SetFloat(GetKey(key),value);
		}
		
		public static float GetFloat(String key){
			return PlayerPrefs.GetFloat(GetKey(key));
		}
		
		public static void SetString(String key,String value){
			PlayerPrefs.SetString(GetKey(key),value);
		}
		
		public static String GetString(String key){
			return PlayerPrefs.GetString(GetKey(key));
		}
		
		//obj必须为可序列化的对象
		public static void SetObject(String key,object obj){
			SetString(key,Serializetion.ToBase64String(obj));
		}
		
		public static T GetObject<T>(String key){
			return Serializetion.FromBase64String<T>(GetString(key));
		}
		
		public static bool HasKey(String key){
			return PlayerPrefs.HasKey(GetKey(key));
		}
		//暂不支持删除一个RootId下面所有的Key，如果需要要扩展，记录一个RootId下面有哪些key
		public static void DeleteKey(String key){
			PlayerPrefs.DeleteKey(GetKey(key));
		}
		/// <summary>
		/// 默认Unity在游戏退出时自动调用，如果崩溃或异常退出，则需要手工调用，在正常运行期间不要调用，会造成卡顿
		/// </summary>
		public static void Save(){
			PlayerPrefs.Save();
		}
		
		private static String GetKey(String key){
			return rootId + key;
		}
	}
}