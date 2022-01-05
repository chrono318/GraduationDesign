using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldBtn : MonoBehaviour
{
    public UnityEvent Event;
    private Vector3 oriScale;
    // Start is called before the first frame update
    void Start()
    {
        oriScale = transform.localScale;
    }
    private void OnMouseEnter()
    {
        transform.localScale = oriScale * 1.2f;
    }
    private void OnMouseExit()
    {
        transform.localScale = oriScale;
    }
    private void OnMouseUpAsButton()
    {
        Event.Invoke();
    }
    private void Reset()
    {
        gameObject.AddComponent<BoxCollider2D>();
    }
}
