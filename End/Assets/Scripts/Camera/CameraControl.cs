using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraControl : MonoBehaviour
{
    public float shakeDuration = 0.25f;
    public float shakeIntensity = 1f;
    private float shakeTime = 0.25f;
    private bool shaking = false;
    public GameObject player;
    private Transform playerTrans;
    public float CameraOffsetCoe = 1;
    //后效
    private PostProcessVolume volume;
    private Bloom bloom;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    //淡入淡出
    public Animator DeliveryFade;
    // Start is called before the first frame update
    void Start()
    {
        playerTrans = player.transform;
        if (!gameObject.CompareTag("MainCamera")) return;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out vignette);
        CloseInjureVolume();
        CloseProcessVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking) return;
        Vector2 rota = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        rota /= new Vector2(Screen.width / 2, Screen.height / 2);
        rota = new Vector2(rota.x * Mathf.Abs(rota.x), rota.y * Mathf.Abs(rota.y));

        rota *= CameraOffsetCoe;

        Vector2 pos = playerTrans.position;
        //transform.position = new Vector3(pos.x + rota.x , pos.y + rota.y , -10);
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(pos.x + rota.x, pos.y + rota.y, -10),ref currenV, 0.2f);
        transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Shake(Vector2.down));
        }
    }
    //计算阻尼(系数)
    float Damping(float dep)
    {
        float oriLenght = 0.2f;
        float detalX = Mathf.Clamp(dep - oriLenght, 0, 1);
        float force = 1 * detalX;
        return dep;
    }
    public void CameraShake(Vector2 dir)
    {
        shaking = true;
        StartCoroutine(Shake(dir));
    }
    public void CameraInjure(Vector2 dir)
    {
        shaking = true;
        StartCoroutine(Shake(dir));
        OpenInjureVolume();
        Invoke(nameof(CloseInjureVolume), shakeDuration);
    }
    IEnumerator Shake(Vector2 dir)
    {
        Vector3 oriPos = transform.position;
        float m_shakeIntensity = 1;
        shakeTime = shakeDuration;

        while (shakeTime > 0)
        {
            float p = shakeTime / shakeDuration;
            p = 1 - p;

            m_shakeIntensity = Mathf.Sin(p * 28.5f) * Mathf.Exp(-2f * p);

            Vector3 v3 = dir * m_shakeIntensity * shakeIntensity;

            transform.position = oriPos + v3;

            shakeTime -= Time.deltaTime;
            yield return 0;
        }

        //transform.position = oriPos;
        shaking = false;
    }

    public void OpenProcessVolume()
    {
        if (chromaticAberration)
        {
            chromaticAberration.active = true;
        }
    }
    public void CloseProcessVolume()
    {
        if (chromaticAberration)
        {
            chromaticAberration.active = false;
        }
    }

    public void OpenInjureVolume()
    {
        if (vignette)
        {
            vignette.active = true;
        }
    }
    public void CloseInjureVolume()
    {
        if (vignette)
        {
            vignette.active = false;
        }
    }

    //fade
    public void Fade()
    {
        DeliveryFade.SetTrigger("fade");
    }
}
