using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Collections.Generic;

public class MapEditor : EditorWindow
{
    private static MapEditor window;

    [MenuItem("地图编辑器/编辑地图(第一步)")]
    static void Init()
    {
        window = GetWindow<MapEditor>(false, "编辑地图", true);
        window.Show();
    }
    
    private static int mapCol = 10;
    private static int mapLine = 10;

    private static Rect loadBtnShowRect;
    private static Rect genBtnShowRect;
    private static GUIContent btnContent;
    private static GUIContent autoGenBtnCon;
    private static GUIContent loadMapBtnCon;
    private static float btnWidth = 200.0f;

    GameObject mapRoot;

    Dictionary<string, string> tileInfo = new Dictionary<string, string>();

    void OnGUI()
    {
        mapCol = EditorGUILayout.IntField("地图列数", mapCol);
        mapLine = EditorGUILayout.IntField("地图行数", mapLine);

        autoGenBtnCon = new GUIContent("生成地砖");
        loadMapBtnCon = new GUIContent("加载已有地图");
        EditorGUILayout.Space();

        loadBtnShowRect = GUILayoutUtility.GetRect(btnContent, GUI.skin.button);
        loadBtnShowRect.x = loadBtnShowRect.width / 4;
        loadBtnShowRect.width = 3 * loadBtnShowRect.x / 4;
        if (GUI.Button(loadBtnShowRect, loadMapBtnCon))
        {
            if (GameObject.Find("MapRoot") == null || GameObject.Find("MapRoot").GetComponent<Map>() == null)
            {
                EditorUtility.DisplayDialog("提示", "该场景没有地图信息节点", "确认");
                return;
            }
            mapRoot = GameObject.Find("MapRoot");
            int mapID = mapRoot.GetComponent<Map>().ID;
            if (File.Exists(Application.dataPath + "/Resources/Xmls/MapData/map_" + mapID.ToString() + ".XML"))  //读取到xml数据并加载地图
            {
                int childCount = mapRoot.transform.childCount;
                for (int i = childCount; i > 0; i--)
                {
                    DestroyImmediate(mapRoot.transform.GetChild(i - 1).gameObject, true);
                }
                string filepath = Application.dataPath + @"/Resources/Xmls/MapData/map_" + mapID.ToString() + ".XML";
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);

                XmlNode root = xml.SelectSingleNode("Root");
                int col = 0;
                int line = 0;
                int defaultLine = 0;
                int defaultCol = 0;
                tileInfo.Clear();
                foreach (XmlElement node in root)
                {
                    if (node.ChildNodes[0].InnerText == "mapCol")
                    {
                        col = int.Parse(node.ChildNodes[1].InnerText);
                    }
                    else if (node.ChildNodes[0].InnerText == "mapLine")
                    {
                        line = int.Parse(node.ChildNodes[1].InnerText);
                    }
                    else if (node.ChildNodes[0].InnerText == "defaultLine")
                    {
                        defaultLine = int.Parse(node.ChildNodes[1].InnerText);
                    }
                    else if (node.ChildNodes[0].InnerText == "defaultCol")
                    {
                        defaultCol = int.Parse(node.ChildNodes[1].InnerText);
                    }
                    else
                    {
                        tileInfo[node.ChildNodes[0].InnerText] = node.ChildNodes[1].InnerText;
                    }
                }

                GridLayoutGroup grid = mapRoot.GetComponent<GridLayoutGroup>();
                grid.constraintCount = col;
                
                mapRoot.GetComponent<Map>().col = col;
                mapRoot.GetComponent<Map>().line = line;
                mapRoot.GetComponent<Map>().defaultLine = defaultLine;
                mapRoot.GetComponent<Map>().defaultCol = defaultCol;

                for (int i = 0; i < col * line; i++)
                {
                    string str = tileInfo[i.ToString()];
                    string[] info = str.Split(new char[1] { ',' });
                    int tileLine = int.Parse(info[0].Split(new char[1] { ':' })[1]);
                    int tileCol = int.Parse(info[1].Split(new char[1] { ':' })[1]);
                    int tileID = int.Parse(info[2].Split(new char[1] { ':' })[1]);
                    int npcID = int.Parse(info[3].Split(new char[1] { ':' })[1]);
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/UIPrefabs/UIScenes/UISceneMapTile.prefab");
                    if (obj != null)
                    {
                        GameObject tile = Instantiate<GameObject>(obj, mapRoot.transform, false);
                        MapTile mapTile = tile.GetComponent<MapTile>();
                        mapTile.line = tileLine;
                        mapTile.col = tileCol;
                        mapTile.id = tileID;
                        mapTile.npc = npcID;
                        tile.name = tileLine + "_" + tileCol;
                    }
                }
                EditorUtility.DisplayDialog("提示", "地图已加载", "确认");
                window.Close();
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "没有找到 map_" + mapID.ToString() + ".xml 文件", "确认");
            }
        }
        genBtnShowRect = GUILayoutUtility.GetRect(btnContent, GUI.skin.button);
        genBtnShowRect.x = genBtnShowRect.width / 4;
        genBtnShowRect.width = 3 * genBtnShowRect.x / 4;
        if (GUI.Button(genBtnShowRect, autoGenBtnCon))
        {
            window.Close();
            if (GameObject.Find("MapRoot") == null || GameObject.Find("MapRoot").GetComponent<Map>() == null)
            {
                EditorUtility.DisplayDialog("提示", "该场景没有地图信息节点", "确认");
            }
            else
            {
                if (mapCol < 1 || mapLine < 1)
                {
                    EditorUtility.DisplayDialog("提示", "地图行数和地图列数都要大于0", "确认");
                    return;
                }
                mapRoot = GameObject.Find("MapRoot");
                mapRoot.GetComponent<Map>().col = mapCol;
                mapRoot.GetComponent<Map>().line = mapLine;
                int childCount = mapRoot.transform.childCount;
                for (int i = childCount; i > 0; i--)
                {
                    DestroyImmediate(mapRoot.transform.GetChild(i - 1).gameObject, true);
                }
                GridLayoutGroup grid = mapRoot.GetComponent<GridLayoutGroup>();
                grid.constraintCount = mapCol;
                for (int i = 0; i < mapCol * mapLine; i++)
                {
                    int line = i / mapCol;
                    int col = i % mapCol;
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/UIPrefabs/UIScenes/UISceneMapTile.prefab");
                    if (obj != null)
                    {
                        GameObject tile = Instantiate<GameObject>(obj, mapRoot.transform, false);
                        MapTile mapTile = tile.GetComponent<MapTile>();
                        mapTile.line = line;
                        mapTile.col = col;
                        tile.name = line + "_" + col;
                    }
                }
                EditorUtility.DisplayDialog("提示", mapCol + "*" + mapLine + "地图已生成", "确认");
            }
        }
    }

}
