using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBrush : MonoBehaviour {

    public int id;

    private Image img;

	// Use this for initialization
	void Start () {
        img = transform.GetComponent<Image>();
        img.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Tile/tile_" + id.ToString() + ".png");
    }

    public void OnClickBrush()
    {
        MapBrushManager.m_instance.SetID(id);
    }
	
}
