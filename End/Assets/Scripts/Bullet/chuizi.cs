using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chuizi : Attack_Item
{
    public delegate void finish();
    public finish Finish;
    public override void Destroy()
    {
        gameObject.SetActive(false);
        Finish();
    }
}
