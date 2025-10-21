using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public FractalMaterial fractalMaterial;
    public Color AmbientLight = new Color(1, 1, 1);
    public ComputeShader shader;

    void Update()
    {
        shader.SetVector("FractalColor", fractalMaterial.FractalColor);
        shader.SetVector("AmbientLight", AmbientLight);
        shader.SetFloat("DiffuseComponent", fractalMaterial.DiffuseComponent);
        shader.SetFloat("SpecularComponent", fractalMaterial.SpecularComponent);
        shader.SetFloat("Shininess", fractalMaterial.Shininess);
    }
}
