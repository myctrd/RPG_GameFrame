using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBrushManager : MonoBehaviour {

    public static MapBrushManager m_instance;

    public int current_id = -1;

    // Use this for initialization
    void Awake () {
		if(m_instance == null)
        {
            m_instance = this;
        }
        img = transform.GetComponent<Image>();
    }

    Image img;

	// Update is called once per frame
	public void SetID (int id) {
        current_id = id;
        img.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Tile/tile_" + current_id.ToString() + ".png");

    }
}
