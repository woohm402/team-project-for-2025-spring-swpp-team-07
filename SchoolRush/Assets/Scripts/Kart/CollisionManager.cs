using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private Rigidbody rb;
    private float bumpCoef = 3000f;

    private KartController kc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager am = AudioManager.Instance;

        switch (collision.gameObject.tag)
        {
            case "Passenger":
                am.PlayOneShot(am.hitPassengerAudio);
                NotifyDizziness();
                break;
            case "Traffic":
                am.PlayOneShot(am.hitTrafficAudio);
                NotifyDizziness();
                break;
            case "Wall":
                am.PlayOneShot(am.hitWallAudio);
                break;
            case "Terrain":
                kc.SetAsOnGround();
                break;
        }

        GameObject go = collision.gameObject;
        if (go.CompareTag("Traffic"))
        {
            Vector3 v = transform.position - go.transform.position + Vector3.up;
            rb.AddForce(bumpCoef * v, ForceMode.Impulse);
        }
    }

    public void KCRegister(KartController kc)
    {
        this.kc = kc;
    }

    private void NotifyDizziness() {
        kc.GetDizzy();
    }
}
