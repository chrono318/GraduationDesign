using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aipath;
    private new SpriteRenderer renderer;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (aipath.desiredVelocity.x >= 0.01)
        {
            renderer.flipX = true;
        }
        else if(aipath.desiredVelocity.x <= -0.01)
        {
            renderer.flipX = false;
        }
    }
}
