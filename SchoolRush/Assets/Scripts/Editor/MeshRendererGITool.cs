using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class MeshRendererGITool : EditorWindow
{
    [MenuItem("Tools/Mesh Renderer GI Tool")]
    static void Init()
    {
        MeshRendererGITool window = (MeshRendererGITool)EditorWindow.GetWindow(typeof(MeshRendererGITool));
        window.titleContent = new GUIContent("Mesh Renderer GI Tool");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Mesh Renderer Global Illumination Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("This tool will modify all child Mesh Renderers:");
        GUILayout.Label("• Disable Contribute Global Illumination");
        GUILayout.Label("• Set Receive Global Illumination to Light Probes");
        GUILayout.Space(10);

        if (Selection.activeGameObject == null)
        {
            EditorGUILayout.HelpBox("Please select a GameObject.", MessageType.Warning);
            return;
        }

        GUILayout.Label("Selected Object: " + Selection.activeGameObject.name, EditorStyles.helpBox);
        GUILayout.Space(10);

        if (GUILayout.Button("Update All Child Mesh Renderers", GUILayout.Height(30)))
        {
            UpdateMeshRendererGISettings(Selection.activeGameObject);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Update Selected Object Only", GUILayout.Height(25)))
        {
            UpdateSingleMeshRenderer(Selection.activeGameObject);
        }
    }

    void UpdateMeshRendererGISettings(GameObject rootObject)
    {
        if (rootObject == null)
        {
            Debug.LogError("Root object is null!");
            return;
        }

        // Find MeshRenderer components in current object and all children
        MeshRenderer[] meshRenderers = rootObject.GetComponentsInChildren<MeshRenderer>(true);
        
        if (meshRenderers.Length == 0)
        {
            Debug.LogWarning($"No MeshRenderer found under '{rootObject.name}' object.");
            EditorUtility.DisplayDialog("Notice", "No child objects with MeshRenderer found.", "OK");
            return;
        }

        // Register for Undo
        Undo.RecordObjects(meshRenderers, "Update Mesh Renderer GI Settings");

        int updatedCount = 0;
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer != null)
            {
                // Disable Contribute Global Illumination
                GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, 
                    GameObjectUtility.GetStaticEditorFlags(meshRenderer.gameObject) & ~StaticEditorFlags.ContributeGI);

                // Set Receive Global Illumination to Light Probes
                meshRenderer.receiveGI = ReceiveGI.LightProbes;

                updatedCount++;
                Debug.Log($"Updated MeshRenderer on '{meshRenderer.gameObject.name}'");
            }
        }

        // Mark scene as dirty
        EditorUtility.SetDirty(rootObject);
        
        string message = $"Updated {updatedCount} MeshRenderer settings under '{rootObject.name}'.";
        Debug.Log(message);
        EditorUtility.DisplayDialog("Complete", message, "OK");
    }

    void UpdateSingleMeshRenderer(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is null!");
            return;
        }

        MeshRenderer meshRenderer = targetObject.GetComponent<MeshRenderer>();
        
        if (meshRenderer == null)
        {
            Debug.LogWarning($"No MeshRenderer found on '{targetObject.name}' object.");
            EditorUtility.DisplayDialog("Notice", "Selected object has no MeshRenderer.", "OK");
            return;
        }

        // Register for Undo
        Undo.RecordObject(meshRenderer, "Update Single Mesh Renderer GI Settings");

        // Disable Contribute Global Illumination
        GameObjectUtility.SetStaticEditorFlags(targetObject, 
            GameObjectUtility.GetStaticEditorFlags(targetObject) & ~StaticEditorFlags.ContributeGI);

        // Set Receive Global Illumination to Light Probes
        meshRenderer.receiveGI = ReceiveGI.LightProbes;

        // Mark scene as dirty
        EditorUtility.SetDirty(targetObject);
        
        string message = $"Updated MeshRenderer settings on '{targetObject.name}'.";
        Debug.Log(message);
        EditorUtility.DisplayDialog("Complete", message, "OK");
    }

    // Add to context menu for easier access
    [MenuItem("GameObject/Mesh Renderer GI/Update All Children", false, 0)]
    static void UpdateAllChildrenFromContextMenu()
    {
        if (Selection.activeGameObject != null)
        {
            MeshRendererGITool tool = new MeshRendererGITool();
            tool.UpdateMeshRendererGISettings(Selection.activeGameObject);
        }
    }

    [MenuItem("GameObject/Mesh Renderer GI/Update Selected Only", false, 1)]
    static void UpdateSelectedOnlyFromContextMenu()
    {
        if (Selection.activeGameObject != null)
        {
            MeshRendererGITool tool = new MeshRendererGITool();
            tool.UpdateSingleMeshRenderer(Selection.activeGameObject);
        }
    }

    // Menu validation
    [MenuItem("GameObject/Mesh Renderer GI/Update All Children", true)]
    [MenuItem("GameObject/Mesh Renderer GI/Update Selected Only", true)]
    static bool ValidateContextMenu()
    {
        return Selection.activeGameObject != null;
    }
}
