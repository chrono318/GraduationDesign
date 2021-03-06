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

    bool isLoading = false;
    int loading = 3;
    float loadingTime = 0f;
    public GameObject loadingUI;
    public Text loadingTex;
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        if(isSelect && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadSceneAsync(scenceName);
            isLoading = true;
            loadingUI.SetActive(true);
            SoundManager.instance.PlaySoundClip(SoundManager.instance.UISound[0]);
        }
        if (isLoading)
        {
            loadingTime += Time.deltaTime;
            loading = (int)loadingTime % 4;
            string tex = "Loading";
            for(int i = 0; i < loading; i++)
            {
                tex += ".";
            }
            loadingTex.text = tex;
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
            anim.Play("not");
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
