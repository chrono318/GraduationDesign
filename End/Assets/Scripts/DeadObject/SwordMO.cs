using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMO : MoveObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out MoveObject mo))
        {
            mo.GetHurt(50f, Vector2.down);
        }
    }
}
