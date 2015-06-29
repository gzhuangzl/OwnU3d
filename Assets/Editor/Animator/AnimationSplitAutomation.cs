using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;

public class AnimatorSplitAutomation
{
	private static bool isSpliting = false;		//自动与手动同时只能工作一个，要不会死循环
	
	private static readonly string ANIMATION_CONFIG_POSTFIX = "_AnimationConfig.xml";
	private static readonly string ANIMATION_FILE_POSTFIX = ".fbx";
	
	//在fbx导入的过程中分隔动画
	class SplitAnimationOnImport : AssetPostprocessor
	{
		void OnPreprocessModel()
		{
			if(isSpliting)
			{
				return;
			}
			isSpliting = true;
			if(assetPath.EndsWith(ANIMATION_FILE_POSTFIX))
			{
				string configPath = assetPath.Replace(ANIMATION_FILE_POSTFIX,ANIMATION_CONFIG_POSTFIX);
				if(File.Exists(configPath))
				{
					ModelImporter model = assetImporter as ModelImporter;
					if(model != null)
					{
						HandlerModelImporter(model,configPath,false);
					}
				}
			}
			isSpliting = false;
		}
	}
	
	
	
	private static Dictionary<string,bool> splitStatus;
	
	/**手动选择包含动画的文件或目录，自动切隔动画
	*  1.选择目录，自动递归遍历并分隔所有动画
	*  2.选择配置文件或.fbx文件，只分隔此一个动画
	*  3.选择其它类型文件，不处理
	*/
	[MenuItem("Tools/Animation/SplitSelectAnimation")]
	public static void SplitSelectAnimation()
	{
		if(isSpliting)
		{
			return;
		}
		
		string[] selectGuids = Selection.assetGUIDs;
		if(selectGuids.Length == 0)
		{
			Debug.Log("请选择Animation目录或文件!");
			return;
		}
		isSpliting = true;
		splitStatus = new Dictionary<string,bool>();
		foreach(string guid in selectGuids)
		{
			string filePath = AssetDatabase.GUIDToAssetPath(guid);
			if(Directory.Exists(filePath))
			{
				HandlerDirectory(filePath);
			}
			else if(File.Exists(filePath))
			{
				HandlerFile(filePath);
			}
		}
		splitStatus.Clear();
		splitStatus = null;
		isSpliting = false;
	}
	
	private static void HandlerDirectory(string directoryPath)
	{
		DirectoryInfo info = new DirectoryInfo(directoryPath);
		foreach(FileInfo fi in info.GetFiles())
		{
			HandlerFile(directoryPath + "/" + fi.Name);
		}
		foreach(DirectoryInfo di in info.GetDirectories())
		{
			HandlerDirectory(directoryPath + "/" + di.Name);
		}
	}
	
	private static void HandlerFile(string filePath)
	{
		if(filePath.EndsWith(ANIMATION_CONFIG_POSTFIX))
		{
			string animatorPath = filePath.Replace(ANIMATION_CONFIG_POSTFIX,ANIMATION_FILE_POSTFIX);
			if(File.Exists(animatorPath))
			{
				SplitAnimator(animatorPath,filePath);
			}
		}
		else if(filePath.EndsWith(ANIMATION_FILE_POSTFIX))
		{
			string configPath = filePath.Replace(ANIMATION_FILE_POSTFIX,ANIMATION_CONFIG_POSTFIX);
			if(File.Exists(configPath))
			{
				SplitAnimator(filePath,configPath);
			}
		}
	}
	
	private static void SplitAnimator(string animatorPath,string configPath)
	{
		bool status;
		splitStatus.TryGetValue(animatorPath,out status);
		if(status == false)
		{
			splitStatus.Add(animatorPath,true);
			
			ModelImporter model = ModelImporter.GetAtPath(animatorPath) as ModelImporter;
			if(model != null)
			{
//				model.animationType = ModelImporterAnimationType.Legacy;
//				model.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
				
				HandlerModelImporter(model,configPath);
			}
		}
	}
	
	private static void HandlerModelImporter(ModelImporter model,string configPath,bool isReimport = true)
	{
		XmlDocument document = new XmlDocument();
		document.Load(configPath);
		if(document.FirstChild.HasChildNodes)
		{
			List<ModelImporterClipAnimation> clips = new List<ModelImporterClipAnimation>();
			foreach(XmlElement node in document.FirstChild.ChildNodes)
			{
				clips.Add(GenerateAnimationClip(node));
			}
			model.clipAnimations = clips.ToArray();
			if(isReimport)
			{
				model.SaveAndReimport();
			}
			Debug.Log("成功切隔动画：" + model.assetPath);
		}
	}
	
	private static ModelImporterClipAnimation GenerateAnimationClip(XmlElement node)
	{
		ModelImporterClipAnimation clip = new ModelImporterClipAnimation();
		clip.name = node.GetAttribute("name");
		clip.firstFrame = int.Parse(node.GetAttribute("startFrame"));
		clip.lastFrame = int.Parse(node.GetAttribute("endFrame"));
		if(node.HasAttribute("isLoop"))
		{
			clip.loop = bool.Parse(node.GetAttribute("isLoop"));
			clip.loopTime = clip.loop;
		}
		else
		{
			clip.loop = false;
			clip.loopTime = clip.loop;
		}
		
		if(node.HasChildNodes)
		{
			List<AnimationEvent> events = new List<AnimationEvent>();
			foreach(XmlElement child in node.ChildNodes)
			{
				AnimationEvent animationEvent = new AnimationEvent();
				animationEvent.functionName = "OnAnimationEvent";
				animationEvent.stringParameter = child.GetAttribute("eventName");
				int startFrame = int.Parse(child.GetAttribute("startFrame"));
				animationEvent.time = (startFrame - clip.firstFrame) / (clip.lastFrame - clip.firstFrame);
				
				if(child.HasAttribute("endFrame"))
				{
					int endFrame = int.Parse(child.GetAttribute("endFrame"));
					//对MoveTo Comeback事件是移动到目标的时间
					animationEvent.floatParameter = endFrame - startFrame;
				}
				events.Add(animationEvent);
			}
			clip.events = events.ToArray();
		}
		return clip;
	}
}