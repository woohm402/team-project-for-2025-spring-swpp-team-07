using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMIndicator : MonoBehaviour
{
    private float length;
    public GameObject player;
    private GameObject checkpoint;

    private float amp = 2f;
    private float freq = 2f;

    private void Start() {
        length = 20f;
    }

    private void Update()
    {
        Vector3 dir = Vector3.Normalize(checkpoint.transform.position - player.transform.position);
        transform.position = player.transform.position + (length + amp * Mathf.Sin(Time.time * freq)) * dir;

        Quaternion rotation = Quaternion.LookRotation(dir, transform.up);
        transform.rotation = rotation * Quaternion.Euler(0, -90, 0);

        Vector3 pos = transform.position;
        transform.position = new(pos.x, player.transform.position.y + 30, pos.z);
    }

    public void SetCheckpoint(GameObject cp){
        checkpoint = cp;
    }
}
