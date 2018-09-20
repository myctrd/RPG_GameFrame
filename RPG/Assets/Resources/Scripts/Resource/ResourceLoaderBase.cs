using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceLoaderBase : MonoBehaviour {

    public static string WriteablePath
    {
        get { return Application.persistentDataPath + "/"; }
    }
    // see https://docs.unity3d.com/Manual/StreamingAssets.html
    public static string StreamingAssetsPath
    {
        get
        {
            return
#if UNITY_EDITOR && FORCE_USE_AB

 Application.dataPath + "/../";
#elif UNITY_ANDROID && !UNITY_EDITOR
 "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
    Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN &&! UNITY_EDITOR
 Application.dataPath + "/StreamingAssets/";

#else
 "";
            //      Application.dataPath + "/StreamingAssets/";
#endif
        }
    }

	public const string AssetPreName = "Assets/Resources/";
    public const string AssetBundlePreName = "Assets/StreamingAssets/";
	
	protected Dictionary<string, AssetBundle> _cache = new Dictionary<string, AssetBundle>();
	protected Dictionary<string, bool> _cache_file_exist = new Dictionary<string, bool>();
    protected Dictionary<string, bool> _cache_check_deps = new Dictionary<string, bool>();
	
	protected static AssetBundle _dep_ab = null;
	public static string DEP_AB_STREAM_FULL_PATH = null;
    public static string DEP_AB_WEITEABLE_FULL_PATH = null;
	protected static Dictionary<string, AssetBundle> _all_deps = new Dictionary<string, AssetBundle>();

    public Sprite LoadItemSprite(int itemId)
    {
        return Load<Sprite>("Textures/Item/item_" + itemId.ToString() + ".png");
    }
	
	public T Load<T>(string _asset_name) where T : UnityEngine.Object
    {
        string asset = AssetPreName + _asset_name;
        var ret = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(asset);
        if (ret == null)
        {
            Debug.LogError("can not find asset " + asset);
            return null;
        }
        return ret;
    }

    public string[] LoadFile(string fileName)
    {
        var fileAddress = System.IO.Path.Combine(AssetPreName, fileName);

        string[] strs = File.ReadAllLines(fileAddress);
        return strs;
    }

    public void LoadAssetBundle(string _ab_name)
    {
        var ab = GetAssetBundle(GetRelativePath(_ab_name));
		var prefab = ab.LoadAsset<GameObject>(_ab_name);
        Instantiate(prefab);
    }
	
	private AssetBundle GetOrCreateAssetBundle(string relativePath, string fullPath)
    {
        AssetBundle ab = null;
        if (_cache.TryGetValue(relativePath, out ab))
        {
            return ab;
        }
        // will load check has in deps share cache?
#if ENABLE_SHARE_DEP_AB
        if (_all_deps.TryGetValue(fullPath, out ab))
        {
       //     Debug.LogError("has loaded dep " + relativePath);
#if UNITY_EDITOR 
            //这条日志是提醒一下  ab包有重叠依赖 ，尽可能减少重叠的依赖项 一旦出现 那么需要检查一下 因为设计的AB规则 不应该有 ab自动依赖(unity没识别出来)之外的依赖关系
            Debug.LogWarning("has loaded dep conflict " + relativePath);
#endif
            return ab;
        }
#endif
        ab = AssetBundle.LoadFromFile(fullPath);
        if (ab != null)
        {
            _cache.Add(relativePath, ab);
            _all_deps.Add(fullPath, ab);
            return ab;
        }
        return null;
    }
	
	void CheckDeps(string relativePath)
    {
#if UNITY_EDITOR && !FORCE_USE_AB
        return;
#else
#endif
        if (_cache_check_deps.ContainsKey(relativePath))
        {
            return;
        }
        else
        {
            _cache_check_deps.Add(relativePath, true);
        }
        //check deps
        AssetBundle ab = null;
        if (_dep_ab == null)
        {
            if (DEP_AB_STREAM_FULL_PATH == null || DEP_AB_WEITEABLE_FULL_PATH == null)
            {
                string dep_name = AssetBundlePreName + AssetBundlePreName.Substring(0, AssetBundlePreName.Length - 1);
                DEP_AB_WEITEABLE_FULL_PATH = WriteablePath + "Patch/" + dep_name;
                DEP_AB_STREAM_FULL_PATH = StreamingAssetsPath + dep_name;
                //  Debug.LogError(DEP_AB_WEITEABLE_FULL_PATH);
                //   Debug.LogError(DEP_AB_STREAM_FULL_PATH);
            }
            //check has patch
            if (FileExists(DEP_AB_WEITEABLE_FULL_PATH))
            {//in writeable path
                ab = AssetBundle.LoadFromFile(DEP_AB_WEITEABLE_FULL_PATH);
            }
            else
            {
                ab = AssetBundle.LoadFromFile(DEP_AB_STREAM_FULL_PATH);
            }
            //load in orgined app package streaming asset path
        }
        else
        {
            ab = _dep_ab;
        }
        if (ab != null)
        {
            _dep_ab = ab;
            // load dep ab
            var dep = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (dep != null)
            {
                //has dep
                Dictionary<string, string> _dep = new Dictionary<string, string>();

                string[] allABs = dep.GetAllAssetBundles();
                for (int i = 0; i < allABs.Length; ++i)
                {
                    if (!relativePath.Contains(allABs[i])) continue;
                    string[] __dep = dep.GetDirectDependencies(allABs[i]);
                    for (int j = 0; j < __dep.Length; ++j)
                    {
                        string ab_name = (AssetBundlePreName + __dep[j]);
                        _dep.Add(ab_name, "");
                    }
                }

                foreach (var a in _dep)
                {
                    var _ab = GetAssetBundle(a.Key);
                    if (_ab == null)
                    {
                        Debug.LogError("load dep error: can not find " + a.Key);
                    }
                    else
                    {
                        //  Debug.LogError("load dep " + a.Key + "   " + _dep.Count);
                    }
                }
            }
        }
    }
	
	protected bool FileExists(string fullPath)
    {
        bool exist = false;
        if (_cache_file_exist.TryGetValue(fullPath, out exist))
        {
            return exist;
        }
        // miss cache
        exist = File.Exists(fullPath);
        _cache_file_exist.Add(fullPath, exist);
        return exist;
    }
    protected AssetBundle GetAssetBundle(string relativePath)
    {
		print(relativePath);
#if UNITY_EDITOR && !FORCE_USE_AB
        // return null;
#else
#endif
        //check deps if-not  may be missing dep object
        CheckDeps(relativePath);
        string fullPath = WriteablePath + "Patch/" + relativePath;

        //check has patch
        if (FileExists(fullPath))
        {
            return GetOrCreateAssetBundle(relativePath, fullPath);
        }
        //load in orgined app package streaming asset path
        fullPath = StreamingAssetsPath + relativePath;

        return GetOrCreateAssetBundle(relativePath, fullPath);
    }
	
	protected string GetRelativePath(string ab_name)
    {
        return AssetBundlePreName + ab_name + ".assetbundle";
    }
    protected string GetRelativePathRaw(string ab_name)
    {
        return AssetBundlePreName + ab_name;
    }
	
	private void OnDestroy()
    {
        this.Destroy();
    }
    public virtual void Destroy()
    {
        if (_dep_ab != null)
        {
            _dep_ab.Unload(true);
            _dep_ab = null;
        }
        foreach (var kv in _cache)
        {
            if (kv.Value != null)
            {
                kv.Value.Unload(true);
                GameObject.DestroyImmediate(kv.Value);
            }
        }
        _cache.Clear();
        _cache_check_deps.Clear();
        _cache_file_exist.Clear();
        _all_deps.Clear();
    }
}
