using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    [Header("ID 순서대로 0~6 체크포인트 오브젝트를 넣어주세요")]
    public GameObject[] checkpoints;           // 0→ID=0(시작지점), … , 6→ID=6(도착지점)

    private int nextCheckpointID = 1;          // 다음 목표 checkpoint id

    private void Start() {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].SetActive(false);

        checkpoints[nextCheckpointID].SetActive(true);
    }

    public int GetNextCheckpointID() {
        return nextCheckpointID;
    }

    public int GetPreviousCheckpointID() {
        return nextCheckpointID - 1;
    }

    public void OnEnterCheckpoint(int checkpointID) {
        if (checkpointID != nextCheckpointID) return;

        Debug.Log($"체크포인트 {checkpointID} 도착!");
        checkpoints[checkpointID].SetActive(false);
        nextCheckpointID++;

        if (nextCheckpointID < checkpoints.Length) checkpoints[nextCheckpointID].SetActive(true);
    }
}
