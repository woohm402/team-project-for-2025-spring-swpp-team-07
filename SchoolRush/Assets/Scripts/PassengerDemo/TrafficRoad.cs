using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger"))
        {
            other.GetComponent<Passenger>().KnowOnTrafficRoad();
        }
    }
}
