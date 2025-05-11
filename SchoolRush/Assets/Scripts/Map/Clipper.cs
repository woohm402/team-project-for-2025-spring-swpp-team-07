using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Clipper : MonoBehaviour
{
    public float clipHeight = 100f; 

    [ContextMenu("Clip Mesh and Save Asset")] 
    public void ClipAndInstantiate()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null)
        {
            Debug.LogError("메시 필터 또는 메시를 찾을 수 없습니다!");
            return;
        }

        Mesh originalMesh = mf.sharedMesh; 
        Vector3[] vertices = originalMesh.vertices; 
        int[] triangles = originalMesh.triangles; 
        Vector2[] uvs = originalMesh.uv; 

        List<Vector3> newVertices = new List<Vector3>(); 
        List<int> newTriangles = new List<int>(); 
        List<Vector2> newUVs = new List<Vector2>(); 
        Dictionary<int, int> indexMap = new Dictionary<int, int>(); 

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i0 = triangles[i];
            int i1 = triangles[i + 1];
            int i2 = triangles[i + 2];

            if (vertices[i0].y > clipHeight && vertices[i1].y > clipHeight && vertices[i2].y > clipHeight)
            {
                int[] idx = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    int vi = triangles[i + j];
                    if (!indexMap.TryGetValue(vi, out int newIndex))
                    {
                        newIndex = newVertices.Count;
                        indexMap.Add(vi, newIndex);
                        newVertices.Add(vertices[vi]);
                        newUVs.Add(uvs[vi]);
                    }
                    idx[j] = newIndex;
                }
                newTriangles.Add(idx[0]);
                newTriangles.Add(idx[1]);
                newTriangles.Add(idx[2]);
            }
        }

        Mesh newMesh = new Mesh(); 
        newMesh.vertices = newVertices.ToArray();
        newMesh.triangles = newTriangles.ToArray();
        newMesh.uv = newUVs.ToArray();
        newMesh.RecalculateNormals(); 
        newMesh.RecalculateBounds(); 

#if UNITY_EDITOR
        const string dir = "Assets/ClippedMeshes";
        if (!AssetDatabase.IsValidFolder(dir))
            AssetDatabase.CreateFolder("Assets", "ClippedMeshes");
        string assetPath = $"{dir}/{gameObject.name}_clipped.asset";
        AssetDatabase.DeleteAsset(assetPath); 
        AssetDatabase.CreateAsset(newMesh, assetPath); 
        AssetDatabase.SaveAssets(); 
        Debug.Log($"잘린 메시 저장 경로: {assetPath}");
#endif
    }
}

