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
	
	[MenuItem("Tools/AssetBundle/AutoSetResourceAssetBundleName")]
	public static void AutoSetResourceAssetBundleName()
	{
		String filePath = Application.dataPath + "/a.txt.meta";
		//UI下面的每个目录都分别对应一个bundle
		
		//Config打成一个
		
		//Audio下面每个文件打成一个
		
		//Animator 都应该是prefab 每个打成一个
		
		//Animation 每个打成一个
		
		SetFileAssetBundleName(filePath,"textbubu");
	}
	
	private static void SetDirectoryAssetBundleName(string dirPath,string bundleName)
	{
	
	}
	
	private static void SetFileAssetBundleName(String filePath,String bundleName)
	{
		StreamReader  reader = new StreamReader(filePath);
		String content = reader.ReadToEnd();
		reader.Close();
		
		content = content.Replace("assetBundleName: \n","assetBundleName: " + bundleName + "\n");
		
		File.Delete(filePath);
		StreamWriter writer = new StreamWriter(filePath,false);
		writer.Write(content);
		writer.Flush();
		writer.Close();
		
		Debug.Log("生成成功：" + filePath);
	}
}