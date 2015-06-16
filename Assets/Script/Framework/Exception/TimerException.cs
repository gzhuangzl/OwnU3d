using System;
namespace Framework
{
	public class TimerException : Exception
	{
		public TimerException ():base(){
			
		}
		
		public TimerException(String message):base(message){
			
		}
		
		public TimerException(String message,Exception innerException):base(message,innerException){
			
		}
	}
}

