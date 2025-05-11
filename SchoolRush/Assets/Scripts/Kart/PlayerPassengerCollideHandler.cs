using UnityEngine;

public class PlayerPassengerCollideHandler : MonoBehaviour
{
    public GameObject checkpointPosition;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Passenger"))
        {
            GoToCheckPoint();
        }
    }

    private void GoToCheckPoint()
    {
        if (checkpointPosition == null) return;
        transform.position = checkpointPosition.transform.position;
    }
}
