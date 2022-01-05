using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetting : MonoBehaviour
{
    public enum AttackMode
    {
        近战,
        远程
    }
    private AttackMode mode = AttackMode.近战;
    public Texture2D[] textures;

    public AttackMode Mode { get => mode; set => mode = value; }


    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Game>().cursorSetting = this;
        Cursor.SetCursor(textures[mode.GetHashCode() * 2], Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(textures[mode.GetHashCode()*2+1], Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(textures[mode.GetHashCode()*2], Vector2.zero, CursorMode.Auto);
        }
    }
}
