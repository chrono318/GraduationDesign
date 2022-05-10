using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullsCollision : MonoBehaviour
{
    public SeagullsMO seagullsMO;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        seagullsMO.CollideWhenRush(collision.gameObject.tag);
    }
}
