using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{

    public int col;
    public int line;
    public int id = 0;
    public int canWalk = 0;
    public int npc = 0;
    private Image img;

    public MapTile(int line, int col)
    {
        this.line = line;
        this.col = col;
    }

    public override string ToString()
    {
        return string.Format("LINE:{0},COL:{1},ID:{2},NPC:{3}",
            this.line,
            this.col,
            this.id,
            this.npc
            );
    }

    void Start()
    {
        img = transform.GetComponent<Image>();
        img.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Tile/tile_" + id.ToString() + ".png");
        if(CSCallLua.m_instance != null)
        {
            Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("tile", id.ToString());
            canWalk = int.Parse((string)data["CANWALK"]);
        }
    }

    public void DisableRayCast()
    {
        if(img == null)
        {
            img = transform.GetComponent<Image>();
        }
        img.raycastTarget = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BrushTile();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BrushTile();
    }

    void BrushTile()
    {
        if (Input.GetMouseButton(0) && MapBrushManager.m_instance.current_id != -1)
        {
            id = MapBrushManager.m_instance.current_id;
            img.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Tile/tile_" + id.ToString() + ".png");
        }
    }
    
}

