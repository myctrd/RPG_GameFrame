using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISceneBase : MonoBehaviour {

    protected int step;

	protected void DestroySelf()
	{
		GameObject.Destroy(this.gameObject);
	}
	
}
