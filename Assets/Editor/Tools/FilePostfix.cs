using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class FilePostfix : AssetPostprocessor {

	//最高优先级
//	public override int GetPostprocessOrder ()
//	{
//		return -99999;
//	}

	void OnPreprocessModel()
	{
		Debug.Log(1);
	}
	
	//把新导入的文件名后缀变小字
	static void OnPostprocessAllAssets(String[] importedAssets ,
	                            String[] deletedAssets,
	                            String[] movedAssets,
	                            String[] movedFromAssetPaths)
	{
		foreach(string assetPath in importedAssets)
		{
			if(File.Exists(assetPath))
			{
				int index = assetPath.LastIndexOf(".");
				if(index != -1)
				{
					string postfix = assetPath.Substring(index);
					if(postfix != postfix.ToLower())
					{
						string newPath = assetPath.Substring(0,index) + postfix.ToLower();
						FileUtil.MoveFileOrDirectory(assetPath,newPath);
					}
				}
			}
		}
	}
}