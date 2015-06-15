using System;
using UnityEngine;
namespace Framework
{
	public sealed class Log
	{
		private Log ()
		{
		}
		
		public static void WriteLog(object log){
			WriteLog(log,"black");
		}
		
		public static void WriteLog(object log,String color){
			LogRecorder.Instance.Log(log);
			Debug.Log(GetColorString(log,color));
		}
		
		public static void WriteWarning(object warning){
			WriteWarning(warning,"yellow");
		}
		
		public static void WriteWarning(object warning,String color){
			LogRecorder.Instance.Warning(warning);
			Debug.LogWarning(GetColorString(warning,color));
		}
		
		public static void WriteError(object error){
			WriteError(error,"red");
		}
		
		public static void WriteError(object error,String color){
			LogRecorder.Instance.Error(error);
			Debug.LogError(GetColorString(error,color));
		}
		
		private static String GetColorString(object obj,String color){
			if(color != null){
				return String.Format("<color={0}>{1}</color>",color,obj);
			}
			return obj.ToString();
		}
	}
}