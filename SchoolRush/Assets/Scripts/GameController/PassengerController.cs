using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public void StopSpawnAndDestroyAll()
    {
        var passengers = GameObject.FindGameObjectsWithTag("Passenger");

        foreach (GameObject a in passengers)
        {
            a.Destroy();
        }

        gameObject.Destroy();
    }
}
