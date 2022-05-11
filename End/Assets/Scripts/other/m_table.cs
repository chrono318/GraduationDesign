using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_table : MonoBehaviour
{
    public Animator anim;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
                anim.SetTrigger("fan");
                anim.gameObject.tag = "Wall";
                anim.gameObject.layer = LayerMask.NameToLayer("Table");
                //wall.SetActive(true);
                //gameObject.layer = LayerMask.NameToLayer("Table");
            }
        }
    }
}
