using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*
--------------------------------------------------------------
I    Naming Convention = DEF_MESH_NameOfParent               I
I                                                            I
I    Interactivity Options = DEF/COL                         I
I                                                            I
I    Collider Options = BOX/SPHERE/CAPSULE/MESH              I
--------------------------------------------------------------
*/

public class GenerateCollidersPostProcessor : AssetPostprocessor
{

    [MenuItem("Custom Tools/VR Collider Generation")]
    static void ToggleColliderGeneration()
    {
        var betterColliderGenerationEnabled = EditorPrefs.GetBool("VRColliderGeneration", false);
        EditorPrefs.SetBool("VRColliderGeneration", !betterColliderGenerationEnabled);
    }

    [MenuItem("Custom Tools/VR Collider Generation", true)]
    static bool ValidateToggleColliderGeneration()
    {
        var betterColliderGenerationEnabled = EditorPrefs.GetBool("VRColliderGeneration", false);
        Menu.SetChecked("Custom Tools/VR Collider Generation", betterColliderGenerationEnabled);
        return true;
    }

    void OnPostprocessModel(GameObject g)
    {
        if (!EditorPrefs.GetBool("VRColliderGeneration", false))
            return;

        //Skip the root
        foreach (Transform child in g.transform)
        {
            GenerateCollider(child);
        }
    }

    bool DetectNamingConvention(Transform t, string convention)
    {
        bool result = false;
        if (t.gameObject.TryGetComponent(out MeshFilter meshFilter))
        {
            var lowercaseMeshName = meshFilter.sharedMesh.name.ToLower();
            result = lowercaseMeshName.Contains($"_{convention}_");
        }

        if (!result)
        {
            var lowercaseName = t.name.ToLower();
            result = lowercaseName.Contains($"_{convention}_");
        }

        return result;
    }

    void GenerateCollider(Transform t)
    {
        foreach (Transform child in t.transform)
        {
            GenerateCollider(child);
        }

        if (DetectNamingConvention(t, "box"))
        {
            AddCollider<BoxCollider>(t);
            DeleteMeshComponents(t);
        }
        else if (DetectNamingConvention(t, "capsule"))
        {
            AddCollider<CapsuleCollider>(t);
            DeleteMeshComponents(t);
        }
        else if (DetectNamingConvention(t, "sphere"))
        {
            AddCollider<SphereCollider>(t);
            DeleteMeshComponents(t);
        }
        else if (DetectNamingConvention(t, "mesh"))
        {
            TransformSharedMesh(t.GetComponent<MeshFilter>());
            var collider = AddCollider<MeshCollider>(t);
            collider.convex = true;
            DeleteMeshComponents(t);
        }
    }

    void TransformSharedMesh(MeshFilter meshFilter)
    {
        if (meshFilter == null)
            return;

        var transform = meshFilter.transform;
        var mesh = meshFilter.sharedMesh;
        var vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
            vertices[i] = transform.parent.InverseTransformPoint(vertices[i]);
        }

        mesh.SetVertices(vertices);
    }

    T AddCollider<T>(Transform t) where T : Collider
    {
        T collider = t.gameObject.AddComponent<T>();

        return collider;
    }

    void DeleteMeshComponents(Transform t)
    {
        MeshFilter filter = t.GetComponent<MeshFilter>();
        MeshRenderer rend = t.GetComponent<MeshRenderer>();

        GameObject.DestroyImmediate(filter);
        GameObject.DestroyImmediate(rend);
    }
}
