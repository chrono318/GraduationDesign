using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomWaring : MonoBehaviour
{
    public bool isReadyBoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isReadyBoom = true;
        }
    }
}
