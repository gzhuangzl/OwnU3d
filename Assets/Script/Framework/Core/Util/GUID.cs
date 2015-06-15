using System;
namespace Framework
{
	public sealed class GUID
	{
		private static ulong IdCount = 0;
		
		private GUID ()
		{
		}
		
		public static ulong GetNumber(){
			if(IdCount == ulong.MaxValue){
				throw new GUIDException("数字型UID已生成到unlong的表示极限，无法生成");
			}
			return IdCount++;
		}
		
		public static String GetString(){
			return Guid.NewGuid().ToString();
		}
		
		public static byte[] GetByteArray(){
			return Guid.NewGuid().ToByteArray();
		}
	}
}