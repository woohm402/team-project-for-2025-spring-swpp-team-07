using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class FindBadColliders : EditorWindow
{
    [MenuItem("Tools/Find Bad MeshColliders And Disable Cooking")]
    static void Init()
    {
        var window = GetWindow<FindBadColliders>();
        window.titleContent = new GUIContent("Bad Colliders Fixer");
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Scan & Disable Cooking"))
        {
            ScanAndDisableCooking();
        }
    }

    public static void ScanAndDisableCooking()
    {
        int count = 0;
        foreach (var mc in GameObject.FindObjectsOfType<MeshCollider>())
        {
            if (mc.sharedMesh == null) continue;
            if (HasDegenerate(mc.sharedMesh))
            {
                mc.cookingOptions = MeshColliderCookingOptions.None;
                EditorUtility.SetDirty(mc);
                count++;
                Debug.LogWarning($"Disabled cooking on '{mc.gameObject.name}'", mc.gameObject);
            }
        }
        Debug.Log($"Completed: Disabled cooking on {count} MeshColliders with degenerate triangles.");
    }

    public static bool HasDegenerate(Mesh mesh)
    {
        var verts = mesh.vertices;
        var tris = mesh.triangles;
        for (int i = 0; i < tris.Length; i += 3)
        {
            var v0 = verts[tris[i]];
            var v1 = verts[tris[i+1]];
            var v2 = verts[tris[i+2]];
            if (Vector3.Cross(v1 - v0, v2 - v0).sqrMagnitude == 0f)
                return true;
        }
        return false;
    }
}

// Build-time processor to remove problematic MeshColliders before building
class MeshColliderBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        int removedCount = 0;
        // Temporarily open and process each build scene without saving changes
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            var loadedScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
            foreach (var mc in GameObject.FindObjectsOfType<MeshCollider>())
            {
                if (mc.sharedMesh != null && FindBadColliders.HasDegenerate(mc.sharedMesh))
                {
                    Debug.LogWarning($"Removing MeshCollider from '{mc.gameObject.name}' for build.", mc.gameObject);
                    Object.DestroyImmediate(mc, true);
                    removedCount++;
                }
            }
            // Unload the scene to restore editor state
            EditorSceneManager.CloseScene(loadedScene, true);
        }
        if (removedCount > 0)
            Debug.Log($"Build Cleanup: Removed {removedCount} bad MeshColliders before build.");
    }
}
