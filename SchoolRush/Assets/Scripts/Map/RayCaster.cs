using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.Jobs;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter))]
public class RayCaster : MonoBehaviour
{
    public float rayHeight = 10f;        
    public LayerMask layerMask = ~0;     
    public float thickness = 0.2f;       

    [ContextMenu("Bake Road Mesh")] 
    public void BakeMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh src = mf.sharedMesh;
        Vector3[] oldV = src.vertices;
        int[] oldT = src.triangles;
        Vector2[] oldUV = src.uv;

        var newV = new System.Collections.Generic.List<Vector3>(oldV);
        var newT = new System.Collections.Generic.List<int>(oldT);
        var newUV = new System.Collections.Generic.List<Vector2>(oldUV);

        Mesh baked = new Mesh();
        baked.vertices = newV.ToArray();
        baked.triangles = newT.ToArray();
        baked.uv = newUV.ToArray();
        
        int vertCount = baked.vertexCount;
        var vertsArray = new NativeArray<Vector3>(baked.vertices, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(vertCount, Allocator.TempJob);
        var results = new NativeArray<RaycastHit>(vertCount, Allocator.TempJob);
        Transform tr = transform;
        for (int i = 0; i < vertCount; i++) {
            Vector3 p = vertsArray[i];
            Vector3 worldP = tr.TransformPoint(p + Vector3.up * rayHeight);
            commands[i] = new RaycastCommand(worldP, Vector3.down, rayHeight * 2, layerMask);
        }
        var handle = RaycastCommand.ScheduleBatch(commands, results, 64);
        handle.Complete();
        for (int i = 0; i < vertCount; i++) {
            if (results[i].collider != null) {
                Vector3 local = tr.InverseTransformPoint(results[i].point);
                var p = vertsArray[i];
                p.y = local.y;
                vertsArray[i] = p;
            }
            vertsArray[i] += Vector3.up * thickness;
        }
        baked.vertices = vertsArray.ToArray();
        baked.RecalculateNormals();
        commands.Dispose();
        results.Dispose();
        vertsArray.Dispose();

        mf.sharedMesh = baked;

        #if UNITY_EDITOR
        const string folderPath = "Assets/BakedMeshes";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets", "BakedMeshes");
        string meshAssetPath = $"{folderPath}/{gameObject.name}_BakedMesh.asset";
        AssetDatabase.CreateAsset(baked, meshAssetPath);
        AssetDatabase.SaveAssets();
        Debug.Log("구운 메시 에셋 저장 경로: " + meshAssetPath);
        #endif

        #if UNITY_EDITOR
        string path = "Assets/RoadBakedMesh.asset";
        AssetDatabase.CreateAsset(baked, path);
        AssetDatabase.SaveAssets();
        Debug.Log("구운 메시 저장 경로: " + path);
        var prefabGO = new GameObject(gameObject.name + "_Baked");
        var mf2 = prefabGO.AddComponent<MeshFilter>();
        mf2.sharedMesh = baked;
        var origRenderer = GetComponent<MeshRenderer>();
        if (origRenderer != null) {
            var mr2 = prefabGO.AddComponent<MeshRenderer>();
            mr2.sharedMaterials = origRenderer.sharedMaterials;
        }
        string prefabPath = "Assets/" + prefabGO.name + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(prefabGO, prefabPath);
        AssetDatabase.SaveAssets();
        Debug.Log("구운 프리팹 저장 경로: " + prefabPath);
        Object.DestroyImmediate(prefabGO);
        #endif
    }
}
