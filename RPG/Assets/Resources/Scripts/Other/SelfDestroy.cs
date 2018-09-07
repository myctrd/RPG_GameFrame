using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

    public float lifeTime;

	// Use this for initialization
	void Start () {
        Invoke("Destroy", lifeTime);	
	}
	
	// Update is called once per frame
	void Destroy () {
        transform.GetComponent<UIBase>().Close();
	}
}
