using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private Rigidbody rb;
    private const float bumpCoef = 3000f;

    [SerializeField]
    private KartController kartController;
    [SerializeField]
    private CheckpointManager checkpointManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 개발용 치트키: checkpoint 로 순간이동
        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            for (int i = 0; i <= 6; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    Debug.Log($"Cheat: Teleporting to checkpoint {i}");
                    GoToCheckPoint(i);
                    break;
                }
            }
        }
        #endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager audioManager = AudioManager.Instance;
        GameObject gameObject = collision.gameObject;

        if (gameObject.CompareTag("Passenger"))
        {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            GoToCheckPoint(checkpointManager.GetPreviousCheckpointID());
            audioManager.PlayOneShot(audioManager.hitPassengerAudio);
        }
        else if (gameObject.CompareTag("Traffic"))
        {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            audioManager.PlayOneShot(audioManager.hitTrafficAudio);
            NotifyDizziness();
            Vector3 v = transform.position - gameObject.transform.position + Vector3.up;
            rb.AddForce(bumpCoef * v, ForceMode.Impulse);
        }
        else if (gameObject.CompareTag("Building"))
        {
            audioManager.PlayOneShot(audioManager.hitWallAudio);
        }
        else if (gameObject.CompareTag("Terrain"))
        {
            kartController.SetAsOnGround();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Checkpoint")) return;
        checkpointManager.OnEnterCheckpoint(other.GetComponent<CheckpointIdentifier>().ID);
    }

    public void GoToCheckPoint(int id)
    {
        transform.position = checkpointManager.checkpoints[id].transform.Find("PortPos").position;
    }

    private void NotifyDizziness()
    {
        kartController.GetDizzy();
    }
}
