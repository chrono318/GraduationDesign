using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCamera : MonoBehaviour
{
    public Camera MainCamera;
    public Shader shader;
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        if(!material)
            material = new Material(shader);
        if (!shader.isSupported)
        {
            print("shader is not supported!");
        }
        print("start");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MainCamera.transform.position + new Vector3(0, 0, 1);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!material) return;
        Graphics.Blit(source, destination, material);
    }
}
