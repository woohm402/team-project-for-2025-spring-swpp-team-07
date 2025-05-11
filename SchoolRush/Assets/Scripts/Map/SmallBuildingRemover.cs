using UnityEngine;
using System.Collections.Generic; 
#if UNITY_EDITOR
using UnityEditor; 
#endif

public class SmallBuildingRemover : MonoBehaviour
{
    [Tooltip("자식들을 확인할 부모 GameObject입니다.")]
    public GameObject parentObject;

    [Tooltip("메시 경계 크기(magnitude)의 임계값입니다. 이보다 작은 메시를 가진 객체는 제거됩니다.")]
    public float sizeThreshold = 130.0f;

    [Tooltip("true이면 Start 시 제거 프로세스가 자동으로 실행됩니다.")]
    public bool runOnStart = true;

    [Tooltip("true이고 에디터에서 실행 중(플레이 모드가 아님)이면 Destroy 대신 DestroyImmediate를 사용합니다. 주의해서 사용하세요.")]
    public bool useDestroyImmediateInEditor = false;

    void Start()
    {
        if (runOnStart)
        {
            RemoveSmallObjects();
        }
    }

    [ContextMenu("작은 객체 지금 제거 (수동)")]
    public void RemoveSmallObjects()
    {
        if (parentObject == null)
        {
            Debug.LogError("SmallBuildingRemover에 ParentObject가 할당되지 않았습니다.", this);
            return;
        }

        bool isManualEditorRun = Application.isEditor && !Application.isPlaying;

        Debug.Log($"'{parentObject.name}'의 손자 객체 제거 프로세스를 시작합니다. 수동 에디터 실행: {isManualEditorRun}");

        List<GameObject> objectsToDestroy = new List<GameObject>();

        for (int i = parentObject.transform.childCount - 1; i >= 0; i--)
        {
            Transform directChild = parentObject.transform.GetChild(i);

            for (int j = directChild.childCount - 1; j >= 0; j--)
            {
                Transform grandchild = directChild.GetChild(j);
                if (grandchild == null) continue;

                MeshFilter meshFilter = grandchild.GetComponent<MeshFilter>();

                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    float meshSize = meshFilter.sharedMesh.bounds.size.magnitude;

                    if (meshSize < sizeThreshold)
                    {
                        Debug.Log($"손자 객체 '{grandchild.name}' ('{directChild.name}'의 자식)를 제거 대상으로 표시합니다. 메시 크기 ({meshSize}) < 임계값 ({sizeThreshold}).");

                        if (isManualEditorRun && useDestroyImmediateInEditor)
                        {
                            objectsToDestroy.Add(grandchild.gameObject);
                        }
                        else if (!isManualEditorRun) 
                        {
                            Destroy(grandchild.gameObject);
                        }
                        else
                        {
                             Debug.Log($"손자 객체 '{grandchild.name}'은(는) 영구적으로 제거되지만, 'useDestroyImmediateInEditor'가 false입니다.");
                        }
                    }
                }
            }
        }

        if (objectsToDestroy.Count > 0)
        {
            Debug.Log($"DestroyImmediate를 사용하여 {objectsToDestroy.Count}개의 객체를 영구적으로 제거합니다.");
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
            #if UNITY_EDITOR
            if (!Application.isPlaying) 
            {
                UnityEditor.EditorUtility.SetDirty(parentObject); 
                // UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(parentObject.scene);
            }
            #endif
        }

        Debug.Log("작은 손자 객체 제거 프로세스가 완료되었습니다.");
    }
}
