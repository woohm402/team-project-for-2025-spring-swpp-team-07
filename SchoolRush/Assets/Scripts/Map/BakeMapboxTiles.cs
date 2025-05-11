#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif
using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AbstractMap))]
public class BakeMapboxTiles : MonoBehaviour
{
    AbstractMap _map;
    Vector3 _rootPosition;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(14f);

        List<GameObject> originalTiles = new List<GameObject>();
        foreach (Transform child in transform)
        {
            originalTiles.Add(child.gameObject);
        }

        foreach (GameObject go in originalTiles)
        {
#if UNITY_EDITOR
            SaveTileAsPrefab(go);
#endif
        }

        Debug.Log("BakeMapboxTiles completed saving prefabs after 14s delay.");
    }


#if UNITY_EDITOR
    void SaveTileAsPrefab(GameObject root)
    {
        GameObject clone = Instantiate(root);
        clone.name = root.name; 

        clone.hideFlags = HideFlags.None;
        clone.isStatic = true;

        string sanitizedName = clone.name.Replace('/', '_').Replace('(', '_').Replace(')', '_'); 
        string prefabBaseDir = "Assets/MapboxBaked";
        string tileAssetDir = $"{prefabBaseDir}/{sanitizedName}"; 
        string prefabPath = $"{tileAssetDir}/{sanitizedName}.prefab";

        if (!Directory.Exists(tileAssetDir))
        {
            Directory.CreateDirectory(tileAssetDir);
        }

        ProcessTileObject(clone, tileAssetDir, sanitizedName);
        ProcessChildObjects(clone, tileAssetDir, sanitizedName);

        Debug.Log($"Attempting to save prefab for {clone.name} from clone.");

        GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(clone, prefabPath, out bool success);

        DestroyImmediate(clone); 

        if (success)
        {
            Debug.Log($"Prefab and assets saved at: {tileAssetDir}");
        }
        else
        {
            Debug.LogError($"Failed to save prefab at: {prefabPath}");
        }
    }

    void ProcessTileObject(GameObject tileObject, string assetDir, string baseName)
    {
        UnityTile unityTile = tileObject.GetComponent<UnityTile>();
        Texture2D savedTexture = null; 

        if (unityTile != null) 
        {
            Texture2D originalTexture = unityTile.GetRasterData(); 

            if (originalTexture != null)
            {
                try
                {
                    TextureFormat format = originalTexture.format;
                    if (!SystemInfo.SupportsTextureFormat(format)) {
                        format = TextureFormat.ARGB32; 
                    }

                    savedTexture = new Texture2D(originalTexture.width, originalTexture.height, format, originalTexture.mipmapCount > 1);
                    Graphics.CopyTexture(originalTexture, savedTexture);

                    string texturePath = $"{assetDir}/{baseName}_Raster.asset";

                    AssetDatabase.CreateAsset(savedTexture, texturePath);
                    AssetDatabase.Refresh(); 

                    savedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

                    if (savedTexture != null)
                    {
                        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                        if (importer != null)
                        {
                            importer.isReadable = false; 
                            importer.SaveAndReimport();
                            Debug.Log($"Saved Raster Texture Asset: {texturePath}");
                        }
                        else
                        {
                            Debug.LogError($"Failed to get TextureImporter for: {texturePath}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to load saved texture asset at: {texturePath}");
                    }
                }
                catch (System.Exception ex)
                {
                     Debug.LogError($"Error processing texture for {tileObject.name}: {ex.Message}");
                     if (savedTexture != null) {
                         string failedPath = AssetDatabase.GetAssetPath(savedTexture);
                         if (!string.IsNullOrEmpty(failedPath)) {
                             AssetDatabase.DeleteAsset(failedPath);
                         }
                         Object.DestroyImmediate(savedTexture); 
                         savedTexture = null;
                     }
                }
            }
            else {
                 Debug.LogWarning($"Original texture is null for {tileObject.name}. Skipping texture save.");
            }
        }
        else {
             Debug.LogWarning($"UnityTile component not found on {tileObject.name}. Skipping texture save.");
        }

        MeshRenderer meshRenderer = tileObject.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = tileObject.GetComponent<MeshFilter>();
        MeshCollider meshCollider = tileObject.GetComponent<MeshCollider>();

        Mesh savedMesh = null;
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            savedMesh = SaveMeshAsset(meshFilter.sharedMesh, assetDir, $"{baseName}_Mesh");
            if (savedMesh != null)
            {
                meshFilter.sharedMesh = savedMesh; 
                EditorUtility.SetDirty(meshFilter);
            }
        }

        if (meshRenderer != null && meshRenderer.sharedMaterial != null)
        {
            Material originalMaterial = meshRenderer.sharedMaterial;

            if (savedTexture != null && originalMaterial.mainTexture != savedTexture) 
            {
                 originalMaterial.mainTexture = savedTexture;
                 EditorUtility.SetDirty(originalMaterial); 
            }

            Material savedMaterial = SaveMaterialAsset(originalMaterial, assetDir, $"{baseName}_Material");
            if (savedMaterial != null)
            {
                meshRenderer.sharedMaterial = savedMaterial;
                EditorUtility.SetDirty(meshRenderer);
            }
        }

        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            if (savedMesh != null && meshCollider.sharedMesh == meshFilter?.sharedMesh)
            {
                meshCollider.sharedMesh = savedMesh; 
            }
            else
            {
                Mesh colliderMesh = SaveMeshAsset(meshCollider.sharedMesh, assetDir, $"{baseName}_ColliderMesh");
                 if (colliderMesh != null)
                 {
                    meshCollider.sharedMesh = colliderMesh; 
                 }
            }
            EditorUtility.SetDirty(meshCollider);
        }
    }
     void ProcessChildObjects(GameObject tileObject, string assetDir, string baseName)
    {
        foreach (Transform child in tileObject.transform)
        {
            GameObject childGo = child.gameObject;
            MeshRenderer childRenderer = childGo.GetComponent<MeshRenderer>();
            MeshFilter childFilter = childGo.GetComponent<MeshFilter>();
            MeshCollider childCollider = childGo.GetComponent<MeshCollider>();

            string childAssetName = $"{baseName}_{childGo.name}"; 

            Mesh savedChildMesh = null;
            if (childFilter != null && childFilter.sharedMesh != null)
            {
                savedChildMesh = SaveMeshAsset(childFilter.sharedMesh, assetDir, $"{childAssetName}_Mesh");
                if (savedChildMesh != null)
                {
                    childFilter.sharedMesh = savedChildMesh; 
                    EditorUtility.SetDirty(childFilter);
                }
            }

            if (childRenderer != null && childRenderer.sharedMaterials != null && childRenderer.sharedMaterials.Length > 0)
            {
                Material[] originalMaterials = childRenderer.sharedMaterials;
                Material[] savedMaterials = new Material[originalMaterials.Length];
                bool materialsChanged = false;

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    if (originalMaterials[i] != null)
                    {
                        Material savedMat = SaveMaterialAsset(originalMaterials[i], assetDir, $"{childAssetName}_Material_{i}");
                        if (savedMat != null)
                        {
                            savedMaterials[i] = savedMat;
                            materialsChanged = true;
                        }
                        else
                        {
                            savedMaterials[i] = originalMaterials[i]; 
                        }
                    }
                }

                if (materialsChanged)
                {
                    childRenderer.sharedMaterials = savedMaterials; 
                    EditorUtility.SetDirty(childRenderer);
                }
            }

            if (childCollider != null && childCollider.sharedMesh != null)
            {
                if (savedChildMesh != null && childCollider.sharedMesh == childFilter?.sharedMesh)
                {
                    childCollider.sharedMesh = savedChildMesh; 
                }
                else
                {
                    Mesh colliderMesh = SaveMeshAsset(childCollider.sharedMesh, assetDir, $"{childAssetName}_ColliderMesh");
                    if (colliderMesh != null)
                    {
                        childCollider.sharedMesh = colliderMesh; 
                    }
                }
                 EditorUtility.SetDirty(childCollider);
            }
        }
    }


    Mesh SaveMeshAsset(Mesh meshToSave, string folderPath, string assetName)
    {
        if (meshToSave == null) return null;

        string validAssetName = MakeValidFileName(assetName);
        string meshPath = $"{folderPath}/{validAssetName}.asset";

        Mesh meshCopy = Instantiate(meshToSave); 

        AssetDatabase.CreateAsset(meshCopy, meshPath);
        Debug.Log($"Saved Mesh: {meshPath}");
        return AssetDatabase.LoadAssetAtPath<Mesh>(meshPath); 
    }

    Material SaveMaterialAsset(Material materialToSave, string folderPath, string assetName)
    {
        if (materialToSave == null) return null;

        string validAssetName = MakeValidFileName(assetName);
        string materialPath = $"{folderPath}/{validAssetName}.mat";

        Material materialCopy = new Material(materialToSave); 

        AssetDatabase.CreateAsset(materialCopy, materialPath);
        Debug.Log($"Saved Material: {materialPath}");
        return AssetDatabase.LoadAssetAtPath<Material>(materialPath); 
    }

    string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }

    Texture2D DuplicateTexture(Texture2D source)
    {
        if (source == null) return null;

        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear); 

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return readableText;
    }

#endif
}