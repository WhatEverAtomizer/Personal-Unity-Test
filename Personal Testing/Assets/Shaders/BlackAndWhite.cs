using UnityEngine;

[ExecuteInEditMode]
public class BlackAndWhiteEffect : MonoBehaviour
{
    public Shader shader;
    private Material material;
    [Range(0, 1)]
    public float intensity = 1.0f;

    void Start()
    {
        if (!shader)
        {
            Debug.LogError("Shader is missing!");
            enabled = false;
            return;
        }

        material = new Material(shader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        material.SetFloat("_Intensity", intensity);
        Graphics.Blit(src, dest, material);
    }

    void OnDisable()
    {
        if (material)
        {
            DestroyImmediate(material);
        }
    }
}
