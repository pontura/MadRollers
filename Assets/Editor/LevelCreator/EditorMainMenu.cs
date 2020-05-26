using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorMainMenu : MonoBehaviour
{

    [MenuItem("Level Editor/Add Empty Area")]
    static void AddArea()
    {
        GameObject target = GameObject.Find("LevelCreator");
        //Area area = Instantiate(emptyArea);
        Debug.Log("Add area " + target.name);
    }
    [MenuItem("Level Editor/Add Object/Tree_Tree", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        Debug.Log("CreateCustomGameObject " + menuCommand.context);
        ObjectPool pool = GameObject.Find("sceneObjectsPool").GetComponent<ObjectPool>();
        GameObject target = GameObject.Find("LevelCreator");
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        go.name = "Area" + Random.Range(0, 10000000).ToString() + "_" + Random.Range(0, 10000000).ToString();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, target as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    //[MenuItem("Level Editor/Add Object/Tree")]
    //static void AddTree()
    //{
    //    Debug.Log("Add Tree");
    //}
}
