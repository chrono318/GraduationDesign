using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMO : MoveObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out MoveObject mo))
        {
            if (!mo.isPlayer)
            {
                mo.GetHurt(51f, Vector2.down);
            }
        }
    }
}
