using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnSeaPlayer : MonoBehaviour
{
    public float speed = 10f;
    //public Text aimText;
    //public Text tipsText;
    bool isSelect;
    string scenceName;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        if(isSelect && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(scenceName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            collision.gameObject.transform.Find("Frame").gameObject.SetActive(true);
            //aimText.gameObject.transform.parent.gameObject.SetActive(true);
            //this.aimText.text = collision.gameObject.GetComponent<ChangeText>().aimText;
            //this.tipsText.text = collision.gameObject.GetComponent<ChangeText>().tipsText;
            this.scenceName = collision.gameObject.GetComponent<ChangeText>().scenceName;
            isSelect = true;
            anim.Play("select");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            collision.gameObject.transform.Find("Frame").gameObject.SetActive(false);
            //aimText.gameObject.transform.parent.gameObject.SetActive(false);
            isSelect = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "SavePoint")
    //    {
    //        //SceneManager.LoadScene("OriRoom");
    //    }
    //}
}
