using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstRole : Player
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("roll");
        }
        Move();
    }
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float t = Time.deltaTime;
        Vector3 move = new Vector3(h * speed * t, v * speed * t, 0);
        transform.position += move;

        float dir = h == 0 ? (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).x : h;
        transform.localScale = new Vector3((dir >= 0 ? -1 : 1), 1, 1);
        //animator.SetBool("Move", Vector3.Magnitude(move) > 0.01 ? true : false);
    }
}
