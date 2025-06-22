using UnityEngine;

public class AntiRollController : MonoBehaviour
{
    [Range(10f, 45f)]
    public float maxTiltAngle = 25f;
    
    [Range(500f, 3000f)]
    public float stabilizationForce = 1500f;
    
    [Range(-1f, 0f)]
    public float centerOfMassOffset = -0.5f;
    
    private Rigidbody rb;
    private CarAIController carAI;
    private bool isStabilizing = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        carAI = GetComponent<CarAIController>();
        
        // 무게중심 낮추기
        Vector3 centerOfMass = rb.centerOfMass;
        centerOfMass.y += centerOfMassOffset;
        rb.centerOfMass = centerOfMass;
    }
    
    void FixedUpdate()
    {
        // 기울기 각도 계산
        float zRotation = transform.rotation.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;
        float tiltAngle = Mathf.Abs(zRotation);
        
        // 전복 방지
        if (tiltAngle > maxTiltAngle)
        {
            isStabilizing = true;
            Vector3 torque = Vector3.forward * (-Mathf.Sign(zRotation) * stabilizationForce);
            rb.AddTorque(torque, ForceMode.Force);
        }
        else
        {
            isStabilizing = false;
        }
    }
    
    public bool IsStabilizing() => isStabilizing;
}
