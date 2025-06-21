using UnityEngine;
using UnityEditor;

public class GSDRoadHeightMatcher : EditorWindow
{
    [MenuItem("Tools/Road Architect/Set Match Heights Distance")]
    public static void ShowWindow()
    {
        GetWindow<GSDRoadHeightMatcher>("Road Height Matcher");
    }

    private float newDistance = 12f;

    void OnGUI()
    {
        GUILayout.Label("GSD Road Height Distance Setter", EditorStyles.boldLabel);
        
        newDistance = EditorGUILayout.FloatField("New Distance:", newDistance);
        
        if (GUILayout.Button("Apply to Selected Objects"))
        {
            ApplyToSelectedObjects();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("This will change opt_MatchHeightsDistance, set opt_TerrainSubtract_Match to 0.2, and disable opt_bGSDRoadRaise in all GSDRoad components found in selected objects and their children.", MessageType.Info);
    }

    private void ApplyToSelectedObjects()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("No object selected!");
            return;
        }

        int changedCount = 0;
        
        foreach (GameObject selectedObj in Selection.gameObjects)
        {
            // 선택된 오브젝트와 모든 하위 오브젝트에서 GSDRoad 컴포넌트 찾기
            GSDRoad[] roadComponents = selectedObj.GetComponentsInChildren<GSDRoad>();
            
            foreach (GSDRoad road in roadComponents)
            {
                Undo.RecordObject(road, "Change Match Heights Distance");
                road.opt_MatchHeightsDistance = newDistance;
                road.opt_TerrainSubtract_Match = 0.3f;
                road.opt_bGSDRoadRaise = true;
                EditorUtility.SetDirty(road);
                changedCount++;
            }
        }
        
        Debug.Log($"Changed opt_MatchHeightsDistance to {newDistance}, set opt_TerrainSubtract_Match to 0.2, and disabled opt_bGSDRoadRaise in {changedCount} GSDRoad components.");
    }
}