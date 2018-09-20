using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class SaveMapInfo : EditorWindow
{
    private static SaveMapInfo window;

    [MenuItem("地图编辑器/保存地图(第二步)")]
    static void Init()
    {
        if (GameObject.Find("MapRoot") == null || GameObject.Find("MapRoot").GetComponent<Map>() == null)
        {
            EditorUtility.DisplayDialog("提示", "该场景没有地图信息节点", "确认");
        }
        else
        {
            GameObject mapRoot = GameObject.Find("MapRoot");
            mapRoot.GetComponent<Map>().SaveMapInfo();
            EditorUtility.DisplayDialog("提示", "地图已保存", "确认");
        }
    }

    


}
