using System;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
	
	public sealed class ObjectPool
	{
		public static readonly int CAPACITY = 20;
		private static Dictionary<Type,Stack<IReusable>> pools = new Dictionary<Type, Stack<IReusable>>();
		
		private ObjectPool ()
		{
		}
		/// <summary>
		/// 从池中获取一个对象，如池中没有，则创建一个
		/// </summary>
		/// <returns>T</returns>
		/// <param name="constructorParams">创建对象时，构造函数的参数列表</param>
		/// <typeparam name="T">T必须实现了IReusable接口.</typeparam>
		public static T GetObject<T>(params object[] constructorParams)where T:IReusable{
			Stack<IReusable> stack = GetTypePools(typeof(T));
			if(stack != null && stack.Count > 0){
				return (T)stack.Pop();
			}else{
				return (T)Activator.CreateInstance(typeof(T),constructorParams);
			}
		}
		/// <summary>
		/// 把一个对象放回池中，如池已满，则丢弃
		/// </summary>
		/// <param name="obj">IReusable.</param>
		public static void DisposeObject(IReusable obj){
			if(obj != null){
				obj.Reset();
				Stack<IReusable> statck = GetTypePools(obj.GetType(),true);
				if(statck.Count < CAPACITY){
					statck.Push(obj);
				}
			}
		}
		/// <summary>
		/// 清理某个类型T的对象池
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ClearObject<T>(){
			Stack<IReusable> stack = GetTypePools(typeof(T));
			if(stack.Count > 0){
				stack.Clear();
				pools.Remove(typeof(T));
			}
		}
		/// <summary>
		/// 清空对象池
		/// </summary>
		public static void ClearAll(){
			foreach(KeyValuePair<Type,Stack<IReusable>> pair in pools){
				pair.Value.Clear();
			}
			pools.Clear();
		}
		
		private static Stack<IReusable> GetTypePools(Type type,bool isAutoCreate = false){
			Stack<IReusable> stack;
			pools.TryGetValue(type,out stack);
			if(isAutoCreate && stack == null){
				stack = new Stack<IReusable>(CAPACITY);
				pools.Add(type,stack);
			}
			return stack;
		}
	}
}