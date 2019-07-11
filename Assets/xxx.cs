using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xxx : MonoBehaviour {


    void Start()
    {
        createMeshBlock();
    }
    private GameObject createMeshBlock()
    {
        GameObject block = new GameObject("Block");
        Mesh newMesh = new Mesh();
        block.AddComponent<MeshFilter>();
        block.AddComponent<MeshRenderer>();
        List<Vector3> verticeList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();
        List<int> triList = new List<int>();
        int width = 20;
        int height = 20;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                verticeList.Add(new Vector3(x, 0f, y));
                uvList.Add(new Vector2(x, y));
                //Skip if a new square on the plane hasn't been formed
                if (x == 0 || y == 0)
                    continue;
                //Adds the index of the three vertices in order to make up each of the two tris
                triList.Add(width * x + y); //Top right
                triList.Add(width * x + y - 1); //Bottom right
                triList.Add(width * (x - 1) + y - 1); //Bottom left - First triangle
                triList.Add(width * (x - 1) + y - 1); //Bottom left 
                triList.Add(width * (x - 1) + y); //Top left
                triList.Add(width * x + y); //Top right - Second triangle
            }
        }
        newMesh.vertices = verticeList.ToArray();
        newMesh.uv = uvList.ToArray();
        newMesh.triangles = triList.ToArray();
        newMesh.RecalculateNormals();
        block.GetComponent<MeshFilter>().mesh = newMesh;
        return block;
    }
}
