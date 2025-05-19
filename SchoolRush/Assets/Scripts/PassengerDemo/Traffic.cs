using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward); 
    }

    public void WaitForPassenger()
    {
        moveSpeed = 0;
    }

    public void ResumeForPassenger()
    {
        moveSpeed = 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndWall"))
        {
            Destroy(gameObject);
        }
    }
}
