using System;
using System.IO;
using UnityEngine;

namespace Framework
{
	public class LogRecorder : Singleton<LogRecorder>
	{
		public readonly String LogFilePath;
		private StreamWriter writer;
		
		public LogRecorder ():base()
		{
			if(Application.platform == RuntimePlatform.OSXEditor ||
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.WindowsEditor ||
				Application.platform == RuntimePlatform.WindowsPlayer){
				LogFilePath = Application.dataPath + "/../Log.log";
			}else{
				LogFilePath = Application.persistentDataPath + "/Log.log";
			}
			StreamWriter we;
		}
		
		private StreamWriter GetWriter(){
			if(writer == null){
				writer = new StreamWriter(LogFilePath,true);
			}
		}
		
		~LogRecorder(){
			if(writer != null){
				writer.Flush();
				writer.Close();
				writer.Dispose();
				writer = null;
			}
		}
	}
}