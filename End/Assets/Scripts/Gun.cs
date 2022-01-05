using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform back;
    public Transform forward;
    private Vector3 posOffest;
    private float lastAngle = 0f;

    private bool inSpace = false;
    private float myTimeSpace = 0f;

    [HideInInspector]
    public int surplusB;
    private bool reloading = false;
    [Header("武器数据")]
    public int capacity;
    public float timeOfReload;
    public float spaceOfFire;
    //删除
    public int sum = 1000;

    private new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        posOffest = transform.position - back.position;

        //
        surplusB = capacity;
        renderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (inSpace)
        {
            myTimeSpace += Time.deltaTime;
            if (myTimeSpace >= spaceOfFire)
            {
                myTimeSpace = 0;
                inSpace = false;
            }
        }
        float col = (float)surplusB / capacity;
        renderer.color = new Color(col, col, col, 1);
        UIManager.instance.UI_Bullet.UpdateNum(surplusB, sum);
    }
    public void SetPos(Vector3 rightHandPos,float angle,Vector3 axis)
    {
        transform.RotateAround(rightHandPos, axis, angle-lastAngle);
        //transform.position = rightHandPos + posOffest;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        lastAngle = angle;
    }
    public void Fire()
    {
        if (inSpace || reloading) return;
        GameObject bull = Instantiate(bullet, forward.position, transform.rotation) as GameObject;
        surplusB--;

        if (surplusB == 0)
        {
            reloading = true;
            Reload();
        }
        inSpace = true;
    }
    public void Reload()
    {
        StartCoroutine(nameof(IEReload));
    }
    IEnumerator IEReload()
    {
        yield return new WaitForSeconds(timeOfReload);
        reloading = false;
        surplusB = capacity;
        sum -= capacity;
    }
}
