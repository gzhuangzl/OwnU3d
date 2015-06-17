using System;
using UnityEngine;
namespace Framework
{
	public class AssetBundleResource : AbstractResource<AssetBundle>
	{
		protected AssetBundleResource (String fullPath):base(fullPath)
		{
			
		}
		
		public static AssetBundleResource Create(String fullPath)
		{
			return new AssetBundleResource(fullPath);
		}
	}
}