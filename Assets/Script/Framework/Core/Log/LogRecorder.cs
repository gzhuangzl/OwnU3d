using System;
using System.IO;
using UnityEngine;
using System.Text;

namespace Framework
{
	public class LogRecorder : Singleton<LogRecorder>
	{
		private static readonly String LogType = "[ L O G ]";
		private static readonly String WarningType = "[WARNING]";
		private static readonly String ErrorType = "[ ERROR ]";
		
		public readonly String LogFilePath;
		private StreamWriter writer;
		private StringBuilder buffer;
		
		public LogRecorder ():base()
		{
			String fileName = DateTime.Now.ToString("yyyy-MM-dd") + "----Log.log";
			if(Application.platform == RuntimePlatform.OSXEditor ||
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.WindowsEditor ||
				Application.platform == RuntimePlatform.WindowsPlayer){
				LogFilePath = Application.dataPath + "/../" + fileName;
			}else{
				LogFilePath = Application.persistentDataPath + "/" + fileName;
			}
		}
		
		public void Log(object log){
			Writer(log,LogType);
		}
		
		public void Warning(object warning){
			Writer(warning,WarningType);
		}
		
		public void Error(object error){
			Writer(error,ErrorType);
		}
		
		private StreamWriter GetWriter(){
			if(writer == null){
				writer = new StreamWriter(LogFilePath,true);
			}
			return writer;
		}
		
		private StringBuilder GetBuffer(){
			if(buffer == null){
				buffer = new StringBuilder();
			}
			return buffer;
		}
		
		private void Writer(object log,String type){
			GetBuffer().Append(type);
			GetBuffer().Append("[");
			GetBuffer().Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			GetBuffer().Append("]");
			GetBuffer().Append(log);
			GetBuffer().Append("\n");
			
			GetWriter().WriteLine(GetBuffer());
			GetWriter().Flush();
			
			GetBuffer().Length = 0;
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