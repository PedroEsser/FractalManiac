using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Camera cam;
    public ComputeShader computeShader;
    public RawImage image;
    public RenderTexture target;
    private int kernel;
    public Vector4 DebugVector;
    public Light directionalLight;
    void Start()
    {
        kernel = computeShader.FindKernel("CSMain");
        target = new RenderTexture(Screen.width, Screen.height, 0);
        target.enableRandomWrite = true;
        target.Create();
        computeShader.SetTexture(kernel, "Result", target);
        image.texture = target;
    }

    void Update()
    {
        int threadGroupsX = Mathf.CeilToInt(target.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(target.height / 8.0f);
        computeShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
        computeShader.SetVector("Debug", DebugVector);
        computeShader.SetVector("_LightDirection", directionalLight.transform.forward);
    }
}
