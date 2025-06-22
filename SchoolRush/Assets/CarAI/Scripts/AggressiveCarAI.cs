using UnityEngine;

public class AggressiveCarAI : MonoBehaviour
{
    [Header("Target Settings")]
    private Transform target;
    
    [Header("Aggressive Behavior")]
    [Tooltip("Charge speed (km/h)")]
    public int chargeSpeed = 100;
    
    [Tooltip("Minimum distance to target (won't approach closer than this)")]
    public float minDistanceToTarget = 5f;
    
    [Tooltip("Maximum distance to continue chasing if target is lost")]
    public float maxChaseDistance = 100f;
    
    [Tooltip("Rotation speed toward target")]
    public float turnSpeed = 2f;
    
    [Header("Detection")]
    [Tooltip("Obstacle detection distance")]
    public float obstacleDetectionDistance = 15f;
    
    [Tooltip("Obstacle detection layer mask")]
    public LayerMask obstacleLayerMask = Physics.AllLayers;
    
    [Tooltip("Cooldown time after hitting target (seconds)")]
    public float cooldownTime = 5f;
    
    private CarAIController carController;
    private Rigidbody rb;
    private bool isCharging = true;
    private bool isOnCooldown = false;
    private float lastTargetDistance;
    
    void Start()
    {
        carController = GetComponent<CarAIController>();
        rb = GetComponent<Rigidbody>();
        
        if (carController != null)
        {
            // Modify existing AI controller settings
            carController.isCarControlledByAI = false; // Disable default AI
            carController.speedLimit = chargeSpeed;
            carController.recklessnessThreshold = 20; // More aggressive
            carController.distanceFromObjects = 1f; // Get closer to obstacles
        }
        
        // Ensure rigidbody uses gravity to land properly when spawned at height
        if (rb != null)
        {
            rb.useGravity = true;
        }
        
        lastTargetDistance = float.MaxValue;
    }
    
    void FixedUpdate()
    {
        if (target == null || (!isCharging && !isOnCooldown))
            return;
            
        // If on cooldown, just wait
        if (isOnCooldown)
        {
            carController?.Break(carController.breaking);
            return;
        }
            
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        // Stop chasing if the target is too far
        if (distanceToTarget > maxChaseDistance)
        {
            Debug.Log($"{gameObject.name} lost target - too far away");
            SelfDestruct();
            return;
        }
        
        // Stop if too close to the target
        if (distanceToTarget < minDistanceToTarget)
        {
            carController?.Break(carController.breaking);
            return;
        }
        
        // Move toward the target
        ChaseTarget();
        
        // Detect and avoid obstacles
        HandleObstacles();
        
        lastTargetDistance = distanceToTarget;
    }
    
    void ChaseTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        
        // Rotate toward the target direction
        Vector3 targetRotation = Vector3.RotateTowards(
            transform.forward, 
            directionToTarget, 
            turnSpeed * Time.fixedDeltaTime, 
            0.0f
        );
        
        transform.rotation = Quaternion.LookRotation(targetRotation);
        
        // Move forward
        if (carController != null)
        {
            carController.Accelerate(carController.acceleration);
            carController.Break(0);
            
            // Steering calculation
            Vector3 localTarget = transform.InverseTransformPoint(target.position);
            float steerAngle = Mathf.Clamp(localTarget.x / localTarget.magnitude * 30f, -30f, 30f);
            carController.Turn(steerAngle);
        }
    }
    
    void HandleObstacles()
    {
        // Detect obstacles ahead
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            // Avoid only if it's not the player
            if (hit.collider.transform != target)
            {
                AvoidObstacle(hit);
            }
        }
        
        #if UNITY_EDITOR
        Debug.DrawRay(rayOrigin, transform.forward * obstacleDetectionDistance, Color.red);
        #endif
    }
    
    void AvoidObstacle(RaycastHit obstacle)
    {
        // Simple obstacle avoidance - turn left or right
        Vector3 avoidDirection;
        
        // Avoid toward the more open space between left and right of the obstacle
        Vector3 leftDirection = transform.position + transform.right * -5f;
        Vector3 rightDirection = transform.position + transform.right * 5f;
        
        bool leftClear = !Physics.Raycast(transform.position, (leftDirection - transform.position).normalized, 10f, obstacleLayerMask);
        bool rightClear = !Physics.Raycast(transform.position, (rightDirection - transform.position).normalized, 10f, obstacleLayerMask);
        
        if (leftClear && !rightClear)
        {
            avoidDirection = -transform.right;
        }
        else if (!leftClear && rightClear)
        {
            avoidDirection = transform.right;
        }
        else
        {
            // If both blocked or both clear, choose randomly
            avoidDirection = Random.value > 0.5f ? transform.right : -transform.right;
        }
        
        // Apply avoidance steering
        float avoidSteerAngle = Vector3.Dot(avoidDirection, transform.right) * 45f;
        carController?.Turn(avoidSteerAngle);
        
        // Adjust speed
        carController?.SetSpeed(chargeSpeed / 2);
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isCharging = true;
        Debug.Log($"{gameObject.name} targeting {newTarget.name}");
    }
    
    public void StopCharging()
    {
        isCharging = false;
        carController?.Break(carController.breaking);
    }
    
    void SelfDestruct()
    {
        Debug.Log($"{gameObject.name} self-destructing");
        Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Stop when hitting the player
        if (collision.gameObject.transform == target)
        {
            Debug.Log($"{gameObject.name} hit target!");
            StartCooldown();
            
            // Here you can add damage to player or other effects
            // Example: PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            // if (playerHealth != null) playerHealth.TakeDamage(damage);
        }
    }
    
    void StartCooldown()
    {
        isCharging = false;
        isOnCooldown = true;
        carController?.Break(carController.breaking);
        
        // Start coroutine to resume charging after cooldown
        StartCoroutine(CooldownCoroutine());
    }
    
    System.Collections.IEnumerator CooldownCoroutine()
    {
        Debug.Log($"{gameObject.name} starting cooldown for {cooldownTime} seconds");
        yield return new WaitForSeconds(cooldownTime);
        
        isOnCooldown = false;
        isCharging = true;
        Debug.Log($"{gameObject.name} resuming charge!");
    }
}