using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AssetBundleBuild{

	[MenuItem("Tools/AssetBundle/BuildAssetBundle")]
	public static void BuildAssetBundle()
	{
		//生成之前删除所有之前的文件
		String filePath = Application.streamingAssetsPath + "/AssetBundles";
		DirectoryInfo dir = Directory.CreateDirectory(filePath);
		foreach(FileInfo childFile in dir.GetFiles())
		{
			childFile.Delete();
		}
		foreach(DirectoryInfo childDir in dir.GetDirectories())
		{
			childDir.Delete(true);
		}
		//注：Assets/StreamingAssets目录的资源不会被打包成AssetBundle
		//BuildPipeline.BuildAssetBundle会忽略些目录
		//编译bundle
		BuildPipeline.BuildAssetBundles(filePath,BuildAssetBundleOptions.None);
	}
	
	[MenuItem("Tools/AssetBundle/AutomaticBinding")]
	public static void AutomaticBinding()
	{
		//UI下面的每个目录都分别对应一个bundle
		foreach(DirectoryInfo child in Directory.CreateDirectory(Application.dataPath + "/Resources/UI").GetDirectories())
		{
			AutomaticBindingDirectory(child.FullName,child.Name,false);
		}
		//Config打成一个
		AutomaticBindingDirectory(Application.dataPath + "/Resources/Config","config",false);
		//Audio下面每个文件打成一个
		AutomaticBindingDirectory(Application.dataPath + "/Resources/Audio","",true);
		//Animator 都应该是prefab 每个打成一个
		AutomaticBindingDirectory(Application.dataPath + "/Resources/Animator","",true,".prefab");
		//Animation 每个打成一个
		AutomaticBindingDirectory(Application.dataPath + "/Resources/Animation","",true,".prefab");
	}
	
	private static void AutomaticBindingDirectory(string directoryPath,string assetBundleName,bool isRespective,string suffix = "*")
	{
		DirectoryInfo dir = Directory.CreateDirectory(directoryPath);
		foreach(DirectoryInfo child in dir.GetDirectories())
		{
			AutomaticBindingDirectory(child.FullName,child.Name,isRespective,suffix);
		}
		foreach(FileInfo child in dir.GetFiles())
		{
			AutomaticBindingFile(child.FullName,isRespective ? child.Name.Substring(0,child.Name.IndexOf(".")) : assetBundleName,suffix);
		}
	}
	
	private static void AutomaticBindingFile(String filePath,String assetBundleName,String suffix)
	{
		if(filePath.LastIndexOf(".meta") >= 0 && (suffix == "*" || filePath.LastIndexOf(suffix) >= 0))
		{
			String content = File.ReadAllText(filePath);
			
			File.Delete(filePath);
			
			int startIndex = content.IndexOf("assetBundleName:");
			if(startIndex >= 0)
			{
				int endIndex = content.IndexOf("\n",startIndex);
				if(endIndex >= 0)
				{
					content = content.Replace(content.Substring(startIndex,endIndex - startIndex),"assetBundleName: " + assetBundleName.ToLower());
					File.WriteAllText(filePath,content);
					
					Debug.Log("生成成功：" + filePath);
				}
			}
		}
	}
}