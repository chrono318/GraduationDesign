using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaStar : MonoBehaviour
{

    private Vector3 centerPos;    //你围绕那个点 就用谁的角度
    private float radius = 3;     //物理离 centerPos的距离
    private float angle = 0;      //偏移角度  
    public GameObject bullet;

    void Start()
    {
        CreateCubeAngle30();
    }

    /// <summary>
    /// 生成子弹
    /// </summary>
    public void CreateCubeAngle30()
    {
        centerPos = transform.position;
        //20度生成一个圆
        for (angle = 0; angle < 360; angle += 36)
        {
            //先解决你物体的位置的问题
            // x = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = cos(弧度) , 弧度 = 角度 *3.14f / 180f;   
            float x = centerPos.x + radius * Mathf.Cos(angle * 3.14f / 180f);
            float y = centerPos.y + radius * Mathf.Sin(angle * 3.14f / 180f);
            // 生成一个圆
            GameObject obj1 = Instantiate(bullet, new Vector3(x, y, 0), Quaternion.Euler(0, 0, angle), transform);
        }
    }
}
