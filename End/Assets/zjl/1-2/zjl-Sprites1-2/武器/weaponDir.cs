using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class weaponDir : MonoBehaviour
{
    public Quaternion quaternion;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = quaternion;
    }
}
