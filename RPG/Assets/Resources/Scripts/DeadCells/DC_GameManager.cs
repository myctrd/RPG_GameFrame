using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Collections.Generic;

public class DC_GameManager : MonoBehaviour {

    public static DC_GameManager m_instance;

    private Transform tileRoot, roleRoot;
    Transform t_camera;
    private DC_RolePlayer rolePlayer;
    private Transform t_rolePlayer;

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        tileRoot = transform.Find("SceneCanvas/MapRoot/TileRoot");
        roleRoot = transform.Find("SceneCanvas/MapRoot/RoleRoot");
        t_camera = transform.Find("SceneCamera");
    }
    
    void Start ()
    {
        LoadMap();
    }

    private float speedy = 0;
    private float speedx = 0;
    private float g = 1500;

    void Update()
    {
        if (t_rolePlayer && t_camera)
        {
            t_camera.localPosition = Vector3.Lerp(t_camera.localPosition, t_rolePlayer.localPosition, 0.1f);
            t_rolePlayer.Translate(Vector3.up * speedy * Time.deltaTime);
            t_rolePlayer.Translate(Vector3.right * speedx * Time.deltaTime);

            if (rolePlayer.GetRoleTilePosX() - t_rolePlayer.localPosition.x > tileWidth / 2)
            {
                rolePlayer.WalkOneTile(0, -1);
            }

            if (t_rolePlayer.localPosition.x - rolePlayer.GetRoleTilePosX() > tileWidth / 2)
            {
                rolePlayer.WalkOneTile(0, 1);
            }

            if (t_rolePlayer.localPosition.y - rolePlayer.GetRoleTilePosY() > tileWidth / 2)
            {
                rolePlayer.WalkOneTile(-1, 0);
            }

            if (rolePlayer.GetRoleTilePosY() - t_rolePlayer.localPosition.y > tileWidth / 2)
            {
                rolePlayer.WalkOneTile(1, 0);
            }

            switch(rolePlayer.GetRoleState())
            {
                case RoleState.Jumping:  //跳跃时
                    if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.A))  //左走
                    {
                        speedx = (rolePlayer.CanWalk(Direction.Left) || rolePlayer.CanClimb(Direction.Left)) ? -300 : 0;
                    }
                    if (Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.D))  //右走
                    {
                        speedx = (rolePlayer.CanWalk(Direction.Right) || rolePlayer.CanClimb(Direction.Right)) ? 300 : 0;
                    }
                    speedy -= g * Time.deltaTime;  //重力加速度
                    if (speedy > 0)  //向上跳时检查碰顶
                    {
                        if (!rolePlayer.CanWalk(Direction.Up) && !rolePlayer.CanClimb(Direction.Up))
                            speedy = 0;
                    }
                    if (speedy < 0)  //向下跳时检查落地
                    {
                        if (!rolePlayer.CanWalk(Direction.Down) && GetTile(rolePlayer.line, rolePlayer.col).id == 1)
                        {
                            rolePlayer.SetRoleState(RoleState.None);
                            speedx = 0;
                            speedy = 0;
                            rolePlayer.StopMove();
                        }
                    }
                    if (speedx > 0)  //向右跳时检查碰撞
                    {
                        if (!rolePlayer.CanWalk(Direction.Right) && !rolePlayer.CanClimb(Direction.Right))
                            speedx = 0;
                    }
                    if (speedx < 0)  //向左跳时检查碰撞
                    {
                        if (!rolePlayer.CanWalk(Direction.Left) && !rolePlayer.CanClimb(Direction.Left))
                            speedx = 0;
                    }
                    break;
                case RoleState.Climbing:  //爬行时
                    if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.W))  //左上
                    {
                        speedy = (rolePlayer.CanClimb(Direction.Up) || rolePlayer.CanWalk(Direction.Up)) ? 300 : 0;
                        speedx = rolePlayer.CanClimb(Direction.Up) ? 0 : speedx;
                    }
                    if (Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.S))  //右下
                    {
                        speedy = (rolePlayer.CanClimb(Direction.Down) || rolePlayer.CanWalk(Direction.Down)) ? -300 : 0;
                        speedx = rolePlayer.CanClimb(Direction.Up) ? 0 : speedx;
                    }
                    if (GetTile(rolePlayer.line, rolePlayer.col).id == 1)
                        rolePlayer.StopMove();
                    if (Input.GetAxis("Vertical") == 0 || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))  //停止
                    {
                        speedy = 0;
                    }
                    break;
                default:  //走路或idle时
                    if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.A))  //左走
                    {
                        speedx = rolePlayer.CanWalk(Direction.Left) ? -300 : 0;
                    }
                    if (Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.D))  //右走
                    {
                        speedx = rolePlayer.CanWalk(Direction.Right) ? 300 : 0;
                    }
                    if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.W))  //左上
                    {
                        speedy = rolePlayer.CanClimb(Direction.Up) ? 300 : 0;
                        speedx = rolePlayer.CanClimb(Direction.Up) ? 0 : speedx;
                    }
                    if (Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.S))  //右下
                    {
                        speedy = rolePlayer.CanClimb(Direction.Down) ? -300 : 0;
                        speedx = rolePlayer.CanClimb(Direction.Up) ? 0 : speedx;
                    }
                    if (rolePlayer.CanWalk(Direction.Down) && !rolePlayer.CanClimb(Direction.Down))
                    {
                        rolePlayer.SetRoleState(RoleState.Jumping);
                        speedx *= 0.5f;
                        speedy = 0;
                        return;
                    }
                    if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))  //停止
                    {
                        speedx = 0;
                        rolePlayer.StopMove();
                    }
                    if (Input.GetKeyDown(KeyCode.Space))  //跳跃
                    {
                        speedy = 750;
                        rolePlayer.SetRoleState(RoleState.Jumping);
                    }
                    break;
            }
        }
    }

    public MapTile GetTile(int line, int col)
    {
        Transform t = tileRoot.Find(line + "_" + col);
        if (t != null)
        {
            return t.GetComponent<MapTile>();
        }
        return null;
    }

    Dictionary<string, string> tileInfo = new Dictionary<string, string>();
    private int mapCol, mapLine;
    public int tileWidth = 80;
    
    void LoadMap()
    {
        if (tileRoot != null)
        {
            if (File.Exists(Application.dataPath + "/Resources/Xmls/MapData/map_999.XML"))
            {
                string filepath = Application.dataPath + @"/Resources/Xmls/MapData/map_999.XML";
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
                        tile.name = tileLine + "_" + tileCol;
                    }
                }
                LoadRolePlayer(defaultLine, defaultCol);

            }
        }
    }

    void LoadRolePlayer(int line, int col)
    {
        if (rolePlayer == null)
        {
            GameObject prefab = UIResourceLoader.m_instance.Load<GameObject>("UIPrefabs/DeadCells/UIRolePlayer.prefab");
            if (prefab != null)
            {
                GameObject role = Instantiate<GameObject>(prefab, roleRoot, false);
                rolePlayer = role.GetComponent<DC_RolePlayer>();
                t_rolePlayer = rolePlayer.transform;
                rolePlayer.SetUIRole(1, line, col, (col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth);
                t_rolePlayer.localPosition = new Vector3((col - ((float)mapCol - 1) / 2) * tileWidth, (((float)mapLine - 1) / 2 - line) * tileWidth, 0);
                t_camera.localPosition = t_rolePlayer.localPosition;
            }
        }
    }
}
