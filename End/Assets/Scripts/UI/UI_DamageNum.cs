using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum DamageType
{
    Physical,
    Magic,
    True
}
public class UI_DamageNum : MonoBehaviour
{
    public float Height = 20;
    public float fadetime = 1f;
    private Text text;
    public Vector3 worldPos;
    private float m_time = 0f;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPos);
        m_time = 0f;
        StartCoroutine(nameof(Fade));
        //transform.DOMoveY(transform.position.y + Height, fadetime);
        transform.DOMoveY(transform.localPosition.y + Height, fadetime);
        transform.DOScale(0.5f, fadetime);
        GetComponent<Text>().DOColor(Color.clear, fadetime);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.one;
        GetComponent<Text>().color = Color.red;
    }
    public void SetDamageNum(int damage,DamageType damageType)
    {
        text = GetComponent<Text>();

        text.text = damage.ToString();
        text.color = damageType == DamageType.Magic ? Color.blue : Color.red;
    }
    private IEnumerator Fade()
    {
        while (m_time <= fadetime)
        {
            GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPos) + Vector3.up * Height * m_time / fadetime;
            m_time += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
