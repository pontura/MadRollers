using System.Collections;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private Material[] mats;
    [SerializeField] private MeshRenderer meshRenderer;
    string matName;

    public void Init(string _matName)
    {
        if (_matName == matName)
            return;
        matName = _matName;
        Material mat = GetMat(matName);
        meshRenderer.material = mat;
    }
    public Material GetMat(string matName)
    {
        foreach (Material mat in mats)
        {
            if (mat.name == matName)
                return mat;
        }
        Debug.Log("Falta el color " + matName + " en MaterialSwapper");
        return mats[0];
    }
}
