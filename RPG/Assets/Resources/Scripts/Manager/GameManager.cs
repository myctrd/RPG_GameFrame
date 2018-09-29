using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager m_instance;

    void Awake()
	{
        //PlayerPrefs.DeleteAll();
		if(m_instance == null)
		{
			m_instance = this;
		}
        DontDestroyOnLoad(gameObject);
        LoadScene("Void");
    }

    void Start()
    {
        roleNum = 1;
        LoadRole();
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    public void LoadGameScene()
    {
        LoadScene("Game");
        int mapID = 1;
        if (PlayerPrefs.HasKey("RPGGame_CurrentMap"))
        {
            mapID = PlayerPrefs.GetInt("RPGGame_CurrentMap");
        }
        LoadMap(mapID);
        LoadGold();
    }

    private RolePlayer [] m_PlayerList;

    public RolePlayer[] GetPlayerList()
    {
        return m_PlayerList;
    }

    int roleNum;

    void LoadRole()
    {
        m_PlayerList = new RolePlayer[4]; 
        m_PlayerList[0] = new RolePlayer();
        m_PlayerList[0].SetRolePlayer(10001);
        
        for(int i = 1; i < 4; i ++)
        {
            if(PlayerPrefs.HasKey("RPGGame_Role_" + (10001 + i)))
            {
                m_PlayerList[roleNum] = new RolePlayer();
                m_PlayerList[roleNum].SetRolePlayer(10001 + i);
                roleNum += 1;
            }
        }
    }

    public void GetNewRole(string ID)
    {
        m_PlayerList[roleNum] = new RolePlayer();
        m_PlayerList[roleNum].SetRolePlayer(int.Parse(ID));
        roleNum += 1;
        EventManager.Broadcast("UI.UpdateRoleInfo");
    }

    private int m_gold;

    void LoadGold()
    {
        m_gold = PlayerPrefs.GetInt("RPGGame_Gold");
    }
    
    private RoleEnemy m_Enemy;

    public int CheckEventState(string eventID)  //0.未触发 1.成功 2.失败
    {
        if(PlayerPrefs.HasKey("RPGGame_Event_" + eventID))
        {
            return PlayerPrefs.GetInt("RPGGame_Event_" + eventID);
        }
        else
        {
            return 0;
        }
    }

    public void AddGold(int value)
    {
        m_gold += value;
        PlayerPrefs.SetInt("RPGGame_Gold", m_gold);
        EventManager.Broadcast("Common.UpdateGold");
    }
    
    public int GetPlayerGold()
    {
        m_gold = PlayerPrefs.GetInt("RPGGame_Gold");
        return m_gold;
    }

    public void RemoveEnemyData()
    {
        if(gameObject.GetComponent<RoleEnemy>())
        {
            RoleEnemy role = gameObject.GetComponent<RoleEnemy>();
            Destroy(role);
        }
    }

    public RoleEnemy GetEnemyData(int id)
    {
        if(m_Enemy == null)
        {
            m_Enemy = gameObject.AddComponent<RoleEnemy>();
            m_Enemy.SetRoleEnemy(id);
        }
        return m_Enemy;
    }

    public void SetRolePlayer(string ID)
    {
    }
    

    private int mapID = 0;

    public void LoadMap(int id)
    {
        mapID = id;
        TryLoadMap();
    }

    void TryLoadMap()
    {
        if (GameSceneManager.m_instance != null)
        {
            GameSceneManager.m_instance.LoadMap(mapID);
        }
        else
        {
            Invoke("TryLoadMap", 0.5f);
        }
    }

}

