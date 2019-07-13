using UnityEngine;
using System.Collections;

public class MaterialsChanger : MonoBehaviour {

    public Material[] materials;
    public MeshRenderer[] meshes;
    public GameObject meshContainer;

    private void Start()
    {
        if (meshContainer != null)
            meshes = meshContainer.GetComponentsInChildren<MeshRenderer>();
    }
    private void OnEnable()
    {
        int id = Random.Range(0, materials.Length);

        foreach (MeshRenderer mr in meshes)
            mr.material = materials[id];
    }
}
