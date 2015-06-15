using System;
namespace Framework
{
	public class GUIDException : Exception
	{
		public GUIDException ():base(){
			
		}
		
		public GUIDException(String message):base(message){
			
		}
		
		public GUIDException(String message,Exception innerException):base(message,innerException){
			
		}
	}
}

