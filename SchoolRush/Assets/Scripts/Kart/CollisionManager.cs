using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private Rigidbody rb;
    private float bumpCoef = 3000f;

    [SerializeField]
    private KartController kartController;
    private CheckpointManager checkpointManager;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager audioManager = AudioManager.Instance;
        GameObject gameObject = collision.gameObject;

        if (gameObject.CompareTag("Passenger")) {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            checkpointManager.GoToCheckPoint(kartController.GetNextCheckpointID() - 1);
            audioManager.PlayOneShot(audioManager.hitPassengerAudio);
        } else if (gameObject.CompareTag("Traffic")) {
            ShieldResult shieldResult = kartController.UseShield();
            if (shieldResult == ShieldResult.Succeed) return;
            audioManager.PlayOneShot(audioManager.hitTrafficAudio);
            NotifyDizziness();
            Vector3 v = transform.position - gameObject.transform.position + Vector3.up;
            rb.AddForce(bumpCoef * v, ForceMode.Impulse);
        } else if (gameObject.CompareTag("Building")) {
            audioManager.PlayOneShot(audioManager.hitWallAudio);
        } else if (gameObject.CompareTag("Terrain")) {
            kartController.SetAsOnGround();
        }
    }

    private void NotifyDizziness() {
        kartController.GetDizzy();
    }
}
