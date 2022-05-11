using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullsCollision : MonoBehaviour
{
    public MoveObject MO;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MO.CollideWhenRush(collision.gameObject.tag);
    }
}
