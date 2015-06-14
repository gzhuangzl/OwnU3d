using System;
namespace Framework
{
	public class SingletonException : Exception
	{
		public SingletonException ():base(){
			
		}
		
		public SingletonException(String message):base(message){
		
		}
		
		public SingletonException(String message,Exception innerException):base(message,innerException){
		
		}
	}
}

