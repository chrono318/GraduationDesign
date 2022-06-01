using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraControl : MonoBehaviour
{
    [Header("爆炸震动")]
    public float shakeDuration = 0.25f;
    public float ShotShakeIntensity = 1f;
    public float InjureShakeIntensity = 1f;
    public static CameraControl instance;
    private float shakeTime = 0.25f;
    //private bool shaking = false;
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

    private Vector2 PosOffset = Vector2.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);
    }
    void Start()
    {
        playerTrans = player.transform;
        if (!gameObject.CompareTag("MainCamera")) return;
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out vignette);
        CloseInjureVolume();
        CloseChromaticEffect();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 rota = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        //rota /= new Vector2(Screen.width / 2, Screen.height / 2);
        //rota = new Vector2(rota.x * Mathf.Abs(rota.x), rota.y * Mathf.Abs(rota.y));

        //rota *= CameraOffsetCoe;

        Vector2 pos = playerTrans.position;
        //锁镜头
        //pos = new Vector2(Mathf.Clamp(pos.x, Game.instance.curRoomCameraArea.x, Game.instance.curRoomCameraArea.y), Mathf.Clamp(pos.y, Game.instance.curRoomCameraArea.z, Game.instance.curRoomCameraArea.w));
        pos += PosOffset;
        //transform.position = new Vector3(pos.x + rota.x , pos.y + rota.y , -10);
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(pos.x + rota.x, pos.y + rota.y, -10),ref currenV, 0.2f);
        Vector3 p = new Vector3(pos.x, pos.y, -10);
        transform.position = p;// Vector3.Lerp(transform.position, p, 0.5f);
    }
    //计算阻尼(系数)
    float Damping(float dep)
    {
        float oriLenght = 0.2f;
        float detalX = Mathf.Clamp(dep - oriLenght, 0, 1);
        float force = 1 * detalX;
        return dep;
    }
    public void CameraShakeShot(Vector2 dir,float shakeTime)
    {
        StartCoroutine(ShotShake(dir*ShotShakeIntensity, shakeTime));
    }
    public void CameraInjure(Vector2 dir)
    {
        StartCoroutine(Shake(dir.normalized*InjureShakeIntensity));
        OpenInjureVolume();
        Invoke(nameof(CloseInjureVolume), shakeDuration);
    }
    IEnumerator Shake(Vector2 dirMforce)
    {
        //Vector3 oriPos = transform.position;
        float m_shakeIntensity = 1;
        shakeTime = shakeDuration;

        while (shakeTime > 0)
        {
            float p = shakeTime / shakeDuration;
            p = 1 - p;

            m_shakeIntensity = Mathf.Sin(p * 28.5f) * Mathf.Exp(-2f * p);

            //Vector3 v3 = dirMforce * m_shakeIntensity ;

            //transform.position = oriPos + v3;
            PosOffset = dirMforce * m_shakeIntensity;

            shakeTime -= Time.deltaTime;
            yield return 0;
        }
        PosOffset = Vector2.zero;
        //transform.position = oriPos;
    }
    IEnumerator ShotShake(Vector2 dirMforce, float duration)
    {
        PosOffset = dirMforce;
        float t = 0;
        while (t < duration)
        {
            PosOffset = Vector2.Lerp(dirMforce, Vector2.zero, t / duration * t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        //transform.position = oriPos;
    }

    public void OpenChromaticEffect()
    {
        if (chromaticAberration)
        {
            chromaticAberration.active = true;
        }
    }
    public void CloseChromaticEffect()
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

    public void OpenBloomVolume()
    {
        if (bloom)
        {
            bloom.active = true;
        }
    }
    public void CloseBloomVolume()
    {
        if (bloom)
        {
            bloom.active = false;
        }
    }

    //fade
    public void Fade()
    {
        DeliveryFade.SetTrigger("fade");
    }
}
