using System;
using UnityEngine;
namespace Framework
{
	public abstract class AbstractResource<T> : IResource<T> where T:UnityEngine.Object
	{
		private readonly ulong id;
		private uint referenceCounter = 0;
		protected String name;
		protected String fullPath;
		
		public AbstractResource (String fullPath)
		{
			id = GUID.GetNumber();
			//在此在判断文件的类型，这些应该在外面自动生成全局的文件信息，自动决定是本地加载，还是网络加载，及加载完成后的资源类型等
			fullPath = fullPath.Replace("\\","/");
			int index = fullPath.LastIndexOf("/");
			name =  index >= 0 ? fullPath.Substring(index + 1) : fullPath;
		}

		public ulong GetId ()
		{
			return id;
		}

		public String GetName ()
		{
			return name;
		}

		public String GetFullPath ()
		{
			return fullPath;
		}

		public T GetAsset ()
		{
			throw new NotImplementedException ();
		}

		public void Retain ()
		{
			referenceCounter++;
		}

		public void Release ()
		{
			if(referenceCounter <= 0){
				throw new ResourceReleaseException("资源" + GetFullPath() + "已被释放!");
			}
			referenceCounter--;
		}

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		public bool Equals (IResource<T> other)
		{
			if(other != null){
				return other.GetFullPath() == this.GetFullPath();
			}
			return false;
		}
	}
}

