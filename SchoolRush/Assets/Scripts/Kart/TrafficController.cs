using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class TrafficController : MonoBehaviour
{
    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    public GameObject wayPointObject;
    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;

    private Transform[] wayPointObjects;
    private int nextWayPointIndex = 0;

    float speed, currentSpeed;
    float rotate, currentRotate;
    Color c;

    PlayerData playerData;

    [Header("Bools")]
    public bool drifting;

    [Header("Parameters")]

    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;

    [Header("Model Parts")]

    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;

    void Start()
    {
        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;
        wayPointObjects = System.Array.FindAll(wayPointObject.GetComponentsInChildren<Transform>(), t => t != wayPointObject.transform);
    }

    void Update()
    {
        //Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        //Accelerate
        speed = acceleration;

        Transform nextWayPoint = wayPointObjects[nextWayPointIndex];

        //Steer
        int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        float amount = Mathf.Abs(Input.GetAxis("Horizontal"));

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;

        //a) Kart
        kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (Input.GetAxis("Horizontal") * 15), kartModel.localEulerAngles.z), .2f);

        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);
        backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((Input.GetAxis("Horizontal") * 45)));

    }

    private void FixedUpdate()
    {
        // Forward Movement towards nextWayPoint
        Transform nextWayPoint = wayPointObjects[nextWayPointIndex];
        Vector3 directionToNextWayPoint = (nextWayPoint.position - transform.position).normalized;
        sphere.velocity = directionToNextWayPoint * currentSpeed;

        // Gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        // Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);

        // Update nextWayPointIndex when reached
        if (Vector3.Distance(transform.position, nextWayPoint.position) < 1.0f)
        {
            nextWayPointIndex = (nextWayPointIndex + 1) % wayPointObjects.Length;
        }
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    private void Speed(float x)
    {
        currentSpeed = x;
    }
}
