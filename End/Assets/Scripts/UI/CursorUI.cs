using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    public enum AttackMode
    {
        近战,
        远程
    }
    private AttackMode mode = AttackMode.近战;
    public Sprite[] textures;

    public AttackMode Mode { get => mode; set => mode = value; }

    private Image image;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Game>().cursorSetting = this;
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            image.sprite = textures[mode.GetHashCode() * 2 + 1];
        }
        else
        {
            image.sprite = textures[mode.GetHashCode() * 2];
        }
        Vector2 mousePos = Input.mousePosition;
        rectTransform.anchoredPosition = mousePos;
    }
}
