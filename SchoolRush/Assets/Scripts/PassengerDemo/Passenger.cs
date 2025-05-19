using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private bool isOnTrafficRoad = false;

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndWall"))
        {
            Destroy(gameObject);
        }
    }

    public void KnowOnTrafficRoad()
    {
        isOnTrafficRoad = true;
        ResumeForTraffic();
    }

    public void WaitForTraffic()
    {
        if (isOnTrafficRoad) return;

        moveSpeed = 0;
    }

    public void ResumeForTraffic()
    {
        moveSpeed = 5f;
    }
}
