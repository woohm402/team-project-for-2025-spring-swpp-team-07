using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using GSD.Roads;

public class CheckpointGenerator : EditorWindow
{
    float interval = 10f; // 체크포인트 간격 (m)
    float offsetDist = 2.5f; // 차선 오프셋 (m)

    [MenuItem("Tools/Generate Checkpoints")]
    static void OpenWindow()
    {
        GetWindow<CheckpointGenerator>("Checkpoint Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Checkpoint Settings", EditorStyles.boldLabel);
        interval = EditorGUILayout.FloatField("Interval (m)", interval);
        offsetDist = EditorGUILayout.FloatField("Lane Offset (m)", offsetDist);

        if (GUILayout.Button("Generate for Selected Road"))
        {
            GenerateForSelected();
        }
    }

    void GenerateForSelected()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a GSDRoad object first.", "OK");
            return;
        }

        var road = go.GetComponent<GSDRoad>();
        if (road == null)
        {
            EditorUtility.DisplayDialog("Error", "The selected object does not have a GSDRoad component.", "OK");
            return;
        }

        GenerateForRoad(road);
        EditorUtility.DisplayDialog("Done", $"Checkpoints have been generated for the selected road '{road.name}'.", "OK");
    }

    void GenerateForRoad(GSDRoad road)
    {
        // 부모 오브젝트 생성
        var parent = new GameObject($"Checkpoints_{road.name}");
        Undo.RegisterCreatedObjectUndo(parent, "Create Checkpoint Parent");
        parent.transform.SetParent(road.transform, false);

        var spline = road.GSDSpline;
        float length = spline.distance;
        int count = Mathf.FloorToInt(length / interval);

        var forward = new List<CheckpointScript>();
        var reverse = new List<CheckpointScript>();

        // 정방향 (t=0→1), 좌측 오프셋 (우측통행)
        for (int i = 0; i <= count; i++)
        {
            float t = (i * interval) / length;
            Vector3 pos = spline.GetSplineValue(t);
            Vector3 dir = spline.GetSplineValue(t, true).normalized;
            Vector3 normal = Vector3.Cross(dir, Vector3.up).normalized;
            Vector3 world = pos - normal * offsetDist;

            var cp = SpawnCheckpoint(world, parent.transform);
            forward.Add(cp.GetComponent<CheckpointScript>());
        }

        // 역방향 (t=1→0), 우측 오프셋 (우측통행)
        for (int i = 0; i <= count; i++)
        {
            float t = 1f - (i * interval) / length;
            Vector3 pos = spline.GetSplineValue(t);
            Vector3 dir = spline.GetSplineValue(t, true).normalized;
            Vector3 normal = Vector3.Cross(dir, Vector3.up).normalized;
            Vector3 world = pos + normal * offsetDist;

            var cp = SpawnCheckpoint(world, parent.transform);
            reverse.Add(cp.GetComponent<CheckpointScript>());
        }

        // 체인 연결
        for (int i = 0; i < forward.Count - 1; i++)
            forward[i].nextCheckpoints.Add(forward[i + 1].transform);

        for (int i = 0; i < reverse.Count - 1; i++)
            reverse[i].nextCheckpoints.Add(reverse[i + 1].transform);
    }

    GameObject SpawnCheckpoint(Vector3 pos, Transform parent)
    {
        var cp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(cp, "Spawn Checkpoint");
        cp.transform.position = pos;
        cp.transform.localScale = Vector3.one;
        var col = cp.GetComponent<BoxCollider>();
        col.isTrigger = true;
        cp.transform.SetParent(parent, false);

        var script = cp.AddComponent<CheckpointScript>();
        script.speedLimit = -1;
        return cp;
    }
}