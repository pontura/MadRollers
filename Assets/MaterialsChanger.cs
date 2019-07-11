using UnityEngine;
using System.Collections;

public class MaterialsChanger : MonoBehaviour {

    public string materialName;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public void changeMaterial(Material materialActive, Material materialNew)
    {
      //  skinnedMeshRenderer.material.SetTexture(0, materialNew.mainTexture);           
    }
    public void Init()
    {

    }
}
