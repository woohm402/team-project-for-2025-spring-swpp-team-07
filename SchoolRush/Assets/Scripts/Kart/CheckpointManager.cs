using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CheckpointManager : MonoBehaviour
{
    public KartController kartController;

    [Header("ID 순서대로 0~6 체크포인트 오브젝트를 넣어주세요")]

    public GameObject[] checkpoints;           // 0→ID=0(시작지점), … , 6→ID=6(도착지점)

    void Start() {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].SetActive(false);

        checkpoints[kartController.GetNextCheckpointID()].SetActive(true);
    }

    void Update() {
        // 개발용 치트키: checkpoint 로 순간이동
        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            for (int i = 0; i <= 6; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i)) {
                    Debug.Log($"Cheat: Teleporting to checkpoint {i}");
                    GoToCheckPoint(i);
                    break;
                }
            }
        }
        #endif
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Checkpoint")) return;
        int checkpointID = other.GetComponent<CheckpointIdentifier>().ID;
        if (checkpointID != kartController.GetNextCheckpointID()) return;

        OnEnterCheckpoint(checkpointID);
    }

    void OnEnterCheckpoint(int checkpointID) {
        Debug.Log($"체크포인트 {checkpointID} 도착!");
        checkpoints[checkpointID].SetActive(false);
        kartController.IncrementNextCheckpointID();

        if (kartController.GetNextCheckpointID() < checkpoints.Length) checkpoints[kartController.GetNextCheckpointID()].SetActive(true);
    }

    public void GoToCheckPoint(int id) {
        transform.position = checkpoints[id].transform.position;
    }
}
