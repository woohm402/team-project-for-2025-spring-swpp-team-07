using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public KartController kartController;

    [Header("ID 순서대로 1~6 체크포인트 오브젝트를 넣어주세요")]

    public GameObject checkpointPosition;
    public GameObject[] checkpoints;  // 0→ID=1, … , 5→ID=6(도착지점)

    private int _current = 0;          // 지금까지 통과한 마지막 cp ID (0=start)
    public int CurrentCheckpoint => _current;


    void Start()
    {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].SetActive(false);

        if (checkpoints.Length > 0)
            checkpoints[0].SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Checkpoint")) return;

        int cpId = other.GetComponent<CheckpointIdentifier>().ID;
        if (cpId == _current + 1)
        {
            checkpoints[_current].SetActive(false);
            _current++;
            Debug.Log($"체크포인트 {_current} 통과!");
            checkpointPosition = checkpoints[_current - 1];

            if (_current < checkpoints.Length)
            {
                checkpoints[_current].SetActive(true);
            }

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Passenger")) {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            GoToCheckPoint();
        }
    }

    private void GoToCheckPoint()
    {
        if (checkpointPosition == null) return;
        transform.position = checkpointPosition.transform.position;
    }
}
