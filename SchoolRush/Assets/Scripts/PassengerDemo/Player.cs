using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float rotateSpeed = 90f;


    private void Update()
    {
        float inputHor = Input.GetAxis("Horizontal");
        float inputVer = Input.GetAxis("Vertical");

        transform.Translate(moveSpeed * Time.deltaTime * inputVer * Vector3.forward);
        transform.Rotate(rotateSpeed * Time.deltaTime * inputHor * Vector3.up);
    }
}
