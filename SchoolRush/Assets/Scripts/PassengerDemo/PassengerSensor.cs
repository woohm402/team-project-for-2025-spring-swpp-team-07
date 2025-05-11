using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSensor : MonoBehaviour
{
    private Passenger passenger;

    private int trafficCnt = 0;

    private void Start()
    {
        passenger = GetComponentInParent<Passenger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Traffic") || other.CompareTag("Player"))
        {
            trafficCnt++;
            passenger.WaitForTraffic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Traffic") || other.CompareTag("Player"))
        {
            trafficCnt--;
            if (trafficCnt == 0)
            {
                passenger.ResumeForTraffic();
            }
        }

    }
}
