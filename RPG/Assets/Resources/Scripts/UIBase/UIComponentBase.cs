using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponentBase : UIBase
{

    public override void Close()
    {
        base.Close();
        GameObject.Destroy(this.gameObject);
    }
}
