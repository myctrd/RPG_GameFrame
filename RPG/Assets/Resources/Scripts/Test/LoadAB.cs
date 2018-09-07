using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAB : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// var myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/StreamingAssets/UIPanelCardLibrary.assetbundle");
		
		UIResourceLoader.m_instance.LoadAssetBundle("UIPanelCardLibrary");
	}
	
}
