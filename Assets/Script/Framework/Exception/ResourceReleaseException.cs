using System;

namespace Framework
{
	public class ResourceReleaseException : Exception
	{
		public ResourceReleaseException ()
		{
		}
		
		public ResourceReleaseException(String message):base(message){
			
		}
		
		public ResourceReleaseException(String message,Exception innerException):base(message,innerException){
			
		}
	}
}

