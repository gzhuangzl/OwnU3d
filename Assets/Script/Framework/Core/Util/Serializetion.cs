using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace Framework
{
	/// <summary>
	/// 序列化工具，包含对象与字符串的序列与反序列
	/// </summary>
	public sealed class Serializetion
	{
		private Serializetion ()
		{
		}
		/// <summary>
		/// 把一个Object序列化成一个Base64String
		/// </summary>
		/// <returns>The to string.</returns>
		/// <param name="obj">Object.</param>
		public static String ToBase64String(Object source){
			if(source == null){
				return null;
			}
			try{
				MemoryStream stream = new MemoryStream();
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream,source);
				
				stream.Position = 0;
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer,0,buffer.Length);
				stream.Flush();
				stream.Close();
				
				String target = Convert.ToBase64String(buffer);
				//清理内存
				stream.Dispose();
				return target;
			}catch(Exception exception){
				throw new SerializationException(source.GetType().ToString() + "序列化成" + typeof(String) + "异常!",exception);
			}
		}
		/// <summary>
		/// 把一个Base64String转换成一个Object
		/// </summary>
		/// <returns>The base64 string.</returns>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T FromBase64String<T>(String source){
			if(source == null){
				return default(T);
			}
			try{
				byte[] buffer = Convert.FromBase64String(source);
				MemoryStream stream = new MemoryStream(buffer);
				
				IFormatter formatter = new BinaryFormatter();
				T target = (T)formatter.Deserialize(stream);
				
				stream.Flush();
				stream.Close();
				stream.Dispose();
				return target;
			}catch(Exception exception){
				throw new SerializationException(typeof(String) +":"+source+"序列化成"+typeof(T)+"异常",exception);
			}
		}
	}
}