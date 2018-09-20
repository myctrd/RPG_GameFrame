using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponentBase : UIBase
{

    public void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void Close()
    {
        base.Close();
        GameObject.Destroy(this.gameObject);
    }
}
