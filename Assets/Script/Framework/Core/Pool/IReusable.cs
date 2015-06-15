using System;
namespace Framework
{
	/// <summary>
	/// 当一个对象要放对象池中，要实现这个接口，表示它可以被重用
	/// </summary>
	public interface IReusable
	{
		void Reset();
	}
}