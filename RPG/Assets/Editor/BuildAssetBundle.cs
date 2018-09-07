
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using UnityEditor;

public class EditorBuild : MonoBehaviour
{
    [MenuItem("AssetBundle/Build AssetBundle From Selection")]  
	static void ExportResourceRGB2()  
	{  
		UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);  
		foreach(UnityEngine.Object obj in selection)
		{
			string path = "Assets/StreamingAssets/" + obj.name + ".assetbundle"; 
			BuildPipeline.BuildAssetBundle(obj, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);  
		}
	}  
}