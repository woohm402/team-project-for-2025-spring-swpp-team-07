using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Passenger2 : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isPausedByTraffic = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Assume initial forward direction
        moveDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        if (isPausedByTraffic) return;

        Vector3 delta = moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyObj"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("TrafficSignal"))
        {
            PauseForTraffic();
        }
        else if (other.CompareTag("TrafficResume"))
        {
            ResumeFromTraffic();
        }
    }

    public void PauseForTraffic()
    {
        isPausedByTraffic = true;
    }

    public void ResumeFromTraffic()
    {
        isPausedByTraffic = false;
    }

    public void SetMoveDirection(Vector3 newDirection)
    {
        moveDirection = newDirection.normalized;
    }
}
