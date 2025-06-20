using UnityEngine;

public class CheckpointIdentifier : MonoBehaviour {
    [Tooltip("0은 시작 지점, 1~5는 체크포인트, 6은 도착 지점")]
    public int ID;
    public MMIndicator MMIndicator;

    [SerializeField]
    private UpgradeManager upgradeManager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && ID >= 1 && ID <= 4) {
            upgradeManager.PickUpgrade(ID);
        }
    }

    void OnEnable(){
        if (ID == 0) return;

        MMIndicator.SetCheckpoint(gameObject);
    }
}
