using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public KartController kartController;

    [Header("ID 순서대로 0~6 체크포인트 오브젝트를 넣어주세요")]

    public GameObject[] checkpoints;           // 0→ID=0(시작지점), … , 6→ID=6(도착지점)
    private int nextCheckpointID = 0;          // 지금까지 통과한 마지막 cp ID (0=start)

    void Start() {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].SetActive(false);

        OnEnterCheckpoint(0);
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Checkpoint")) return;
        int checkpointID = other.GetComponent<CheckpointIdentifier>().ID;
        if (checkpointID != nextCheckpointID) return;

        OnEnterCheckpoint(checkpointID);
    }

    void OnEnterCheckpoint(int checkpointID) {
        Debug.Log($"체크포인트 {checkpointID} 통과!");
        checkpoints[checkpointID].SetActive(false);
        nextCheckpointID++;

        if (nextCheckpointID < checkpoints.Length) checkpoints[nextCheckpointID].SetActive(true);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Passenger")) {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            GoToCheckPoint(nextCheckpointID - 1);
        }
    }

    private void GoToCheckPoint(int id) {
        transform.position = checkpoints[id].transform.position;
    }
}
