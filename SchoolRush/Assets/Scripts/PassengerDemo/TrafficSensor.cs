using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSensor : MonoBehaviour
{
    private Traffic traffic;

    private int passengerCnt = 0;


    private void Start()
    {
        traffic = GetComponentInParent<Traffic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger"))
        {
            passengerCnt++;
            traffic.WaitForPassenger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Passenger"))
        {
            passengerCnt--;
            if (passengerCnt == 0)
            {
                traffic.ResumeForPassenger();
            }
        }

    }


}
