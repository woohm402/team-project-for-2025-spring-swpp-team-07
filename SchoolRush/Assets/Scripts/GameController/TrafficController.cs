using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour {
    public GameObject trafficsObject;
    public List<GameObject> trafficList;

    public void Start() {
        trafficList = new List<GameObject>();
        foreach (Transform traffic in trafficsObject.transform)
            trafficList.Add(traffic.gameObject);
    }

    public void DestroyHalf() {
        var trafficsToDelete = new RandomPicker<GameObject>(trafficList).pick(trafficList.Count / 2);
        foreach (GameObject traffic in trafficsToDelete) {
            Destroy(traffic);
            trafficList.Remove(traffic);
        }
    }

    public void DestroyAll() {
        foreach (GameObject traffic in trafficList)
            Destroy(traffic);
        trafficList.Clear();
    }
}
