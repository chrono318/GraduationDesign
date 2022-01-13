using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_table : MonoBehaviour
{
    public Sprite sprite;
    public GameObject wall;

    private bool canPush = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPush = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canPush = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPush = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canPush = false;
        }
    }
    private void Update()
    {
        if (canPush)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //
                GetComponent<SpriteRenderer>().sprite = sprite;

                //wall.SetActive(true);
                gameObject.layer = LayerMask.NameToLayer("Table");
            }
        }
    }
}
