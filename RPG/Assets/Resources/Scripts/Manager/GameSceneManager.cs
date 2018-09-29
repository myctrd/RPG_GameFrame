using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour {

    public static GameSceneManager m_instance;

    private Transform tileRoot, roleRoot;
    Transform t_camera;

    void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
        tileRoot = transform.Find("SceneCanvas/MapRoot/TileRoot");
        roleRoot = transform.Find("SceneCanvas/MapRoot/RoleRoot");
        t_camera = transform.Find("SceneCamera");
    }

    Dictionary<string, string> tileInfo = new Dictionary<string, string>();
    private int mapCol, mapLine;
    public int tileWidth = 80;
    private float speed = 300;

    private UIRolePlayer rolePlayer;
    private Transform t_rolePlayer;

    public int GetInteractiveNPC()
    {
        return rolePlayer.GetInteractiveNPC();
    }

    public MapTile GetTile(int line, int col)
    {
        Transform t = tileRoot.Find(line + "_" + col);
        if(t != null)
        {
            return t.GetComponent<MapTile>();
        }
        return null;
    }

    private int interactiveNPC = 0;
    Dictionary<string, object> data_interactiveNPC;
    Dictionary<string, object> NPC_params = new Dictionary<string, object>();

    private bool canInteraction = true;

    public void SetInteraction(bool state)
    {
        canInteraction = state;
    }

    void Update()
    {
        if(t_rolePlayer && t_camera)
        {
            t_camera.localPosition = Vector3.Lerp(t_camera.localPosition, t_rolePlayer.localPosition, 0.1f);

            //以下为玩家操作，行走、NPC交互等
            if (canInteraction == false)
                return;

            if (Input.GetKeyDown(KeyCode.F) && rolePlayer.GetRoleState() == RoleState.None)  //尝试触发NPC事件
            {
                if (rolePlayer.GetInteractiveNPC() != 0)
                {
                    interactiveNPC = rolePlayer.GetInteractiveNPC();
                    data_interactiveNPC = CSCallLua.m_instance.GetDBData("npc", interactiveNPC.ToString());
                    if((string)data_interactiveNPC["EVENT"] == "0")
                    {
                        NPC_params.Clear();
                        NPC_params.Add("name", (string)data_interactiveNPC["ROLENAME"]);
                        NPC_params.Add("txt", (string)data_interactiveNPC["DIALOG"]);
                        EventManager.Broadcast("Dialog.CommonDialog", NPC_params);
                    }
                    else
                    {
                        if (GameManager.m_instance.CheckEventState((string)data_interactiveNPC["EVENT"]) == 0 || (string)data_interactiveNPC["CANREUSE"] == "1")  //NPC事件未触发过或事件可重复触发
                        {
                            if((string)data_interactiveNPC["PREEVENT"] != "0" && PlayerPrefs.HasKey("RPGGame_Event_" + (string)data_interactiveNPC["PREEVENT"]) == false)  //前置事件未触发
                            {
                                if ((string)data_interactiveNPC["DIALOG"] != "")
                                {
                                    NPC_params.Clear();
                                    NPC_params.Add("npcID", interactiveNPC);
                                    EventManager.Broadcast("Dialog.NPCDialog", NPC_params);
                                }
                                return;
                            }
                            GameEventManager.m_instance.ActivateEvent((string)data_interactiveNPC["EVENT"]);
                            if ((string)data_interactiveNPC["MARKNOW"] == "1")  //立刻标记触发过的事件（有的事件可以回头继续触发）
                            {
                                PlayerPrefs.SetInt("RPGGame_Event_" + (string)data_interactiveNPC["EVENT"], 1);
                            }
                        }
                        else  //NPC事件已触发过且不可重复触发弹出固定的对话
                        {
                            if((string)data_interactiveNPC["DIALOG"] != "")
                            {
                                NPC_params.Clear();
                                NPC_params.Add("npcID", interactiveNPC);
                                EventManager.Broadcast("Dialog.NPCDialog", NPC_params);
                            }
                            
                        }
                    }
                    
                }
            }

            if (Input.GetKey(KeyCode.W) && rolePlayer.CanWalk(Direction.Up))
            {
                t_rolePlayer.Translate(Vector3.up * speed * Time.deltaTime);
                if(t_rolePlayer.localPosition.y - rolePlayer.GetRoleTilePosY() > tileWidth / 2)
                {
                    rolePlayer.WalkOneTile(-1, 0);
                }
            }
            if (Input.GetKey(KeyCode.S) && rolePlayer.CanWalk(Direction.Down))
            {
                t_rolePlayer.Translate(Vector3.down * speed * Time.deltaTime);
                if (rolePlayer.GetRoleTilePosY() - t_rolePlayer.localPosition.y > tileWidth / 2)
                {
                    rolePlayer.WalkOneTile(1, 0);
                }
            }
            if (Input.GetKey(KeyCode.A) && rolePlayer.CanWalk(Direction.Left))
            {
                t_rolePlayer.Translate(Vector3.left * speed * Time.deltaTime);
                if (rolePlayer.GetRoleTilePosX() - t_rolePlayer.localPosition.x > tileWidth / 2)
                {
                    rolePlayer.WalkOneTile(0, -1);
                }
            }
            if (Input.GetKey(KeyCode.D) && rolePlayer.CanWalk(Direction.Right))
            {
                t_rolePlayer.Translate(Vector3.right * speed * Time.deltaTime);
                if (t_rolePlayer.localPosition.x - rolePlayer.GetRoleTilePosX() > tileWidth / 2)
                {
                    rolePlayer.WalkOneTile(0, 1);
                }
            }

            if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                rolePlayer.StopMove();
            }
        }
    }

    void LoadRolePlayer(int line, int col)
    {
        if(rolePlayer == null)
        {
            GameObject prefab = UIResourceLoader.m_instance.Load<GameObject>("UIPrefabs/UIRoles/UIRolePlayer.prefab");
            if (prefab != null)
            {
                GameObject role = Instantiate<GameObject>(prefab, roleRoot, false);
                rolePlayer = role.GetComponent<UIRolePlayer>();
                t_rolePlayer = rolePlayer.transform;
                rolePlayer.SetUIRole(10001, line, col, (col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth);
                t_rolePlayer.localPosition = new Vector3((col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth, 0);
                t_camera.localPosition = t_rolePlayer.localPosition;
            }
        }
    }

    void LoadRoleNPC(int id, int line, int col)
    {
        GameObject prefab = UIResourceLoader.m_instance.Load<GameObject>("UIPrefabs/UIRoles/UIRoleNPC.prefab");
        if (prefab != null)
        {
            GameObject role = Instantiate<GameObject>(prefab, roleRoot, false);
            UIRoleBase roleBase = role.GetComponent<UIRoleBase>();
            Transform t_roleBase = roleBase.transform;
            roleBase.SetUIRole(id, line, col, (col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth);
            t_roleBase.localPosition = new Vector3((col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth, 0);
        }
    }

    void ClearMap()
    {
        if (tileRoot != null)
        {
            int childCount = tileRoot.childCount;
            for (int i = childCount; i > 0; i--)
            {
                DestroyImmediate(tileRoot.GetChild(i - 1).gameObject, true);
            }
        }
        if (roleRoot != null)
        {
            int childCount = roleRoot.childCount;
            for (int i = childCount; i > 0; i--)
            {
                DestroyImmediate(roleRoot.GetChild(i - 1).gameObject, true);
            }
        }
    }

    public void LoadMap(int id)
    {
        ClearMap();
        if (tileRoot != null)
        {
            PlayerPrefs.SetInt("RPGGame_CurrentMap", id);
            if (File.Exists(Application.dataPath + "/Resources/Xmls/MapData/map_" + id.ToString() + ".XML"))
            {
                string filepath = Application.dataPath + @"/Resources/Xmls/MapData/map_" + id.ToString() + ".XML";
                XmlDocument xml = new XmlDocument();
                xml.Load(filepath);

                XmlNode root = xml.SelectSingleNode("Root");
                int defaultLine = 0;
                int defaultCol = 0;
                tileInfo.Clear();
                foreach (XmlElement node in root)
                {
                    if (node.ChildNodes[0].InnerText == "mapCol")
                    {
                        mapCol = int.Parse(node.ChildNodes[1].InnerText);
                    }
                    else if (node.ChildNodes[0].InnerText == "mapLine")
                    {
                        mapLine = int.Parse(node.ChildNodes[1].InnerText);
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

                GridLayoutGroup grid = tileRoot.GetComponent<GridLayoutGroup>();
                grid.constraintCount = mapCol;
                for (int i = 0; i < mapCol * mapLine; i++)
                {
                    string str = tileInfo[i.ToString()];
                    string[] info = str.Split(new char[1] { ',' });
                    int tileLine = int.Parse(info[0].Split(new char[1] { ':' })[1]);
                    int tileCol = int.Parse(info[1].Split(new char[1] { ':' })[1]);
                    int tileID = int.Parse(info[2].Split(new char[1] { ':' })[1]);
                    int npcID = int.Parse(info[3].Split(new char[1] { ':' })[1]);
                    GameObject obj = UIResourceLoader.m_instance.Load<GameObject>("UIPrefabs/UIScenes/UISceneMapTile.prefab");
                    if (obj != null)
                    {
                        GameObject tile = Instantiate<GameObject>(obj, tileRoot.transform, false);
                        MapTile mapTile = tile.GetComponent<MapTile>();
                        mapTile.DisableRayCast();
                        mapTile.line = tileLine;
                        mapTile.col = tileCol;
                        mapTile.id = tileID;
                        mapTile.npc = npcID;
                        if(npcID != 0)
                        {
                            LoadRoleNPC(npcID, tileLine, tileCol);
                        }
                        tile.name = tileLine + "_" + tileCol;
                    }
                }
                if(PlayerPrefs.HasKey("RPGGame_CurrentCol"))
                {
                    int currentCol = PlayerPrefs.GetInt("RPGGame_CurrentCol");
                    int currentLine = PlayerPrefs.GetInt("RPGGame_CurrentLine");
                    LoadRolePlayer(currentLine, currentCol);
                }
                else
                {
                    LoadRolePlayer(defaultLine, defaultCol);
                }
                
            }
        }
    }

}

public enum Direction
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}
