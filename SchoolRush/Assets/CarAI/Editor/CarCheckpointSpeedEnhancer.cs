using UnityEditor;
using UnityEngine;

public class CarCheckpointSpeedEnhancer : Editor
{
    [MenuItem("Tools/Set All Checkpoint Speed Limits to -1")]
    public static void SetAllCheckpointSpeedLimitsToMinusOne()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("오류", "GameObject를 선택해주세요.", "확인");
            return;
        }

        CheckpointScript[] checkpointScripts = selectedObject.GetComponentsInChildren<CheckpointScript>();
        
        if (checkpointScripts.Length == 0)
        {
            return;
        }

        Undo.RecordObjects(checkpointScripts, "Set Checkpoint Speed Limits to -1");

        int modifiedCount = 0;
        
        foreach (CheckpointScript checkpoint in checkpointScripts)
        {
            checkpoint.speedLimit = -1;
            modifiedCount++;
            
            EditorUtility.SetDirty(checkpoint);
        }

        EditorUtility.DisplayDialog("완료", 
            $"{modifiedCount}개의 CheckpointScript에서 speedLimit을 -1로 설정했습니다.", 
            "확인");
    }

    [MenuItem("Tools/Set All Checkpoint Speed Limits to -1", true)]
    public static bool ValidateSetAllCheckpointSpeedLimitsToMinusOne()
    {
        // GameObject가 선택되었을 때만 메뉴 활성화
        return Selection.activeGameObject != null;
    }
}