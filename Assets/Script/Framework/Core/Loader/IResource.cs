using UnityEngine;
using System;

namespace Framework
{
	public interface IResource<T>:IEquatable<IResource<T>> where T:UnityEngine.Object
	{
		ulong GetId();
		String GetName();
		String GetFullPath();
		T GetAsset();
		void Retain();
		void Release();
		void Dispose();
	}
}