using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CurvatureEffect : MonoBehaviour
{
    public Material effectMaterial;

    [Range(0f, 1f)]
    public float curvature = 0.6f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (effectMaterial != null)
        {
            effectMaterial.SetFloat("_Curvature", curvature);
            Graphics.Blit(src, dest, effectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest); // sin efecto
        }
    }
}