1.Assets Unity项目的资源主目录，也是Unity编辑器Project的根目录，所有Unity资源（脚本、动画、音频等）都在里面

2.Assets/Editor 存储Unity编辑器脚本

3.Assets/Editor Default Resources Untity编辑器脚本中用到的资源（图片、动画等）目录,EditorGUIUtility.Load用来加载它

4.Assets/Gizmos Gizmos.DrawIcon用到的Icon资源目录

5.Assets/Plugins Unity用到的插件，是一个dll文件，一般用c++编写的，当然也可以放在Assets/Editor目录，只是会影响插件的编译顺序

6.Assets/Resources 游戏中用到的资源，发布时会自动把它打包，打包时资源会压缩跟加密，只能用Resources.load加载，只读权限

7.Assets/Standard Assets 导入的标准库用到的资源（图片、脚本等），当然也可以把它里面的资源移到Assets目录，但是会影响编译顺序
			它跟Assets有相同的目录结构，如Standard Assets/Editor Standard Assets/Scripts等

8.Assets/StreamingAssets 游戏中用到的资源，发布时会自动把它打包，打包时资源不会压缩跟加密，所以它里面只放二进制文件，否则内容
			会被直接查看，一般用于放音频、AssetsBundle等资源，只能用WWW加载，只读权限
			Application.streamingAssetsPath 为它在应用中的根目录，在Andriod里面它是一个jar文件，不用WWW读取的话，要
			自己用其它软件进行读取，详情 http://docs.unity3d.com/Manual/StreamingAssets.html




详情 http://docs.unity3d.com/Manual/SpecialFolders.html