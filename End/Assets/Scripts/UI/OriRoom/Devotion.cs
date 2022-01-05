using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devotion : MonoBehaviour
{
    public Material material;
    private float height;
    public void OnClick()
    {
        height += 0.1f;
        material.SetFloat("_Height", height);
    }
}
