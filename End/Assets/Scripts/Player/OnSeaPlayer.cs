using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnSeaPlayer : MonoBehaviour
{
    public float speed = 10f;
    public Text aimText;
    public Text tipsText;
    bool isSelect;
    
    void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        //float t = Time.deltaTime;
        //transform.position += new Vector3(speed * h * t, speed * v * t, 0);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        if(isSelect && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("OriRoom");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            collision.gameObject.transform.Find("Frame").gameObject.SetActive(true);
            aimText.gameObject.transform.parent.gameObject.SetActive(true);
            this.aimText.text = collision.gameObject.GetComponent<ChangeText>().aimText;
            this.tipsText.text = collision.gameObject.GetComponent<ChangeText>().tipsText;
            isSelect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            collision.gameObject.transform.Find("Frame").gameObject.SetActive(false);
            aimText.gameObject.transform.parent.gameObject.SetActive(false);
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
