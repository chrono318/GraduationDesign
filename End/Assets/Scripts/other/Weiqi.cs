using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weiqi : MonoBehaviour
{
    Animator animator;
    bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SetWeiqiPosition(Vector2 pos,bool left)
    {
        if (isPlaying) return;
        isPlaying = true;
        transform.position = pos;
        transform.localScale = new Vector3(left ? 1 : -1, 1, 1);
        animator.gameObject.SetActive(true);
        animator.Play("weiqi");
        Invoke(nameof(Disappaer), 0.25f);
    }
    void Disappaer()
    {
        animator.gameObject.SetActive(false);
        isPlaying = false;
    }
}
