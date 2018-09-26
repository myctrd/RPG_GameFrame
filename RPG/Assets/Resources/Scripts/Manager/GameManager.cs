using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager m_instance;

    private int [] roleAvailable = new int[4];

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

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadGameScene()
    {
        LoadScene("Game");
        int mapID = 1;
        if (PlayerPrefs.HasKey(GetPlayerData().m_Name + "_CurrentMap"))
        {
            mapID = PlayerPrefs.GetInt(GetPlayerData().m_Name + "_CurrentMap");
        }
        LoadMap(mapID);
    }

    void Start()
    {
        SetRoleAvailable(0, 1);
        SetRoleAvailable(1, 0);
        SetRoleAvailable(2, 0);
        SetRoleAvailable(3, 0);
    }

    public void SetRoleAvailable(int i, int v)
    {
        roleAvailable[i] = v;
    }

    public int[] GetRoleAvailable()
    {
        return roleAvailable;
    }

    private RolePlayer m_Player;
    private RoleEnemy m_Enemy;

    public int CheckEventState(string eventID)  //0.未触发 1.成功 2.失败
    {
        if(PlayerPrefs.HasKey(GetPlayerData().m_Name + "_Event_" + eventID))
        {
            return PlayerPrefs.GetInt(GetPlayerData().m_Name + "_Event_" + eventID);
        }
        else
        {
            return 0;
        }
    }

    public void AddGold(int value)
    {
        m_Player.AddGold(value);
    }

    public int GetPlayerGold()
    {
        return m_Player.GetGold();
    }

    public RolePlayer GetPlayerData()
    {
        return m_Player;
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

    public void SetRolePlayer(int role)
    {
        if(m_Player == null)
        {
            m_Player = gameObject.AddComponent<RolePlayer>();
        }
        m_Player.SetRolePlayer(role);
    }

    public void SetPlayerEquip(int slot, int id)
    {
        m_Player.SetPlayerEquip(slot, id);
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

