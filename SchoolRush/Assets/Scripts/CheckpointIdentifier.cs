using UnityEngine;

public class CheckpointIdentifier : MonoBehaviour
{
    [Tooltip("1~5는 체크포인트, 6은 도착 지점")]
    public int ID;

    [SerializeField]
    private UpgradeManager upgradeManager;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            upgradeManager.PickUpgrade(ID);
        }
    }
}