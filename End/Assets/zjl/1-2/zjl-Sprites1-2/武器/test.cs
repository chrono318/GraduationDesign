using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        //transform.rotation = Quaternion.Euler(0f,0f,Vector2.Angle(Vector2.right, dir));
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
    private void OnDrawGizmos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        Debug.DrawLine(transform.position, (Vector2)transform.position + dir * 5f);
    }
}
