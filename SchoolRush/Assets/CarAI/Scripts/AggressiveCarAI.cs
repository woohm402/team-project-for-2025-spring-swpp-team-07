using UnityEngine;

public class AggressiveCarAI : MonoBehaviour
{
    [Header("Target Settings")]
    private Transform target;
    
    [Header("Aggressive Behavior")]
    [Tooltip("Charge speed (km/h)")]
    public int chargeSpeed = 100;
    
    [Tooltip("Maximum distance to continue chasing if target is lost")]
    public float maxChaseDistance = 500f;
    
    [Tooltip("Rotation speed toward target")]
    public float turnSpeed = 2f;
    
    [Tooltip("Time to predict target position (seconds)")]
    public float predictionTime = 4f;
    
    [Tooltip("Time before switching to real target tracking")]
    public float switchToRealTargetTime = 0.5f;
    
    [Tooltip("Direct force for acceleration")]
    public float accelerationForce = 30000f;
    
    [Header("Detection")]
    [Tooltip("Obstacle detection distance")]
    public float obstacleDetectionDistance = 10f;
    
    [Tooltip("Obstacle detection layer mask")]
    public LayerMask obstacleLayerMask = Physics.AllLayers;
    
    [Tooltip("Cooldown time after hitting target (seconds)")]
    public float cooldownTime = 5f;
    
    private CarAIController carController;
    private Rigidbody rb;
    private bool isCharging = true;
    private bool isOnCooldown = false;
    private float lastTargetDistance;
    private float spawnTime;
    private Vector3 predictedTargetPosition;
    private bool isTrackingPredicted = true;
    private CarAIController targetCarController; // 타겟의 CarController 참조
    
    void Start()
    {
        carController = GetComponent<CarAIController>();
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time;
        
        if (carController != null)
        {
            // Modify existing AI controller settings
            carController.isCarControlledByAI = false; // Disable default AI
            carController.speedLimit = chargeSpeed;
            carController.recklessnessThreshold = 20; // More aggressive
            carController.distanceFromObjects = 1f; // Get closer to obstacles
            carController.acceleration = 100000f; // 매우 높은 가속도 설정
        }
        
        // Ensure rigidbody uses gravity to land properly when spawned at height
        if (rb != null)
        {
            rb.useGravity = true;
        }
        
        lastTargetDistance = float.MaxValue;
        
        // 타겟의 CarController 참조 획득
        if (target != null)
        {
            targetCarController = target.GetComponent<CarAIController>();
        }
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
        
        // 스폰 후 1.5초가 지났는지 확인
        float timeSinceSpawn = Time.time - spawnTime;
        if (timeSinceSpawn >= switchToRealTargetTime && isTrackingPredicted)
        {
            isTrackingPredicted = false;
            Debug.Log($"{gameObject.name} switching to real target tracking");
        }
        
        Vector3 targetPosition;
        
        // 예측 위치 또는 실제 위치 결정
        if (isTrackingPredicted)
        {
            targetPosition = predictedTargetPosition;
        }
        else
        {
            targetPosition = target.position;
        }
            
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        
        // Stop chasing if the target is too far
        if (distanceToTarget > maxChaseDistance)
        {
            Debug.Log($"{gameObject.name} lost target - too far away");
            SelfDestruct();
            return;
        }
        
        // Move toward the target (minDistanceToTarget 조건 제거)
        ChaseTarget(targetPosition);
        
        // Detect and avoid obstacles (우선순위를 추적보다 낮게)
        if (!HandleObstacles())
        {
            // 장애물이 없으면 추가 힘 적용
            float currentSpeedMPS = rb.velocity.magnitude;
            float targetSpeedMPS = chargeSpeed / 3.6f;
            
            if (currentSpeedMPS < targetSpeedMPS)
            {
                rb.AddForce(transform.forward * accelerationForce, ForceMode.Force);
                carController?.Accelerate(carController.acceleration);
                carController?.Break(0);
            }
        }
        
        lastTargetDistance = distanceToTarget;
    }
    
    void ChaseTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
        Vector3 localTarget = transform.InverseTransformPoint(targetPosition);
        float steerAngle = Mathf.Clamp(localTarget.x / localTarget.magnitude * 45f, -45f, 45f);
        
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 10f)
        {
            steerAngle *= 1.5f;
            steerAngle = Mathf.Clamp(steerAngle, -60f, 60f);
        }
        
        // 현재 속도 계산 (m/s)
        float currentSpeedMPS = rb.velocity.magnitude;
        float targetSpeedMPS = chargeSpeed / 3.6f; // km/h를 m/s로 변환
        
        // Direct force application for faster acceleration
        if (carController != null)
        {
            if (currentSpeedMPS < targetSpeedMPS)
            {
                // Rigidbody에 직접 힘 적용 (훨씬 빠른 가속)
                Vector3 forceDirection = transform.forward;
                rb.AddForce(forceDirection * accelerationForce, ForceMode.Force);
                
                // CarController도 함께 사용
                carController.Accelerate(carController.acceleration);
                carController.Break(0);
            }
            else
            {
                // 목표 속도에 도달했으면 속도 유지
                carController.SetSpeed(chargeSpeed);
            }
            
            carController.Turn(steerAngle);
        }
    }
    
    bool HandleObstacles()
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
                return true;
            }
        }
        
        #if UNITY_EDITOR
        Debug.DrawRay(rayOrigin, transform.forward * obstacleDetectionDistance, Color.red);
        #endif
        
        return false;
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
        spawnTime = Time.time;
        isTrackingPredicted = true;
        
        // 타겟의 CarController 참조 획득 (있으면)
        targetCarController = target.GetComponent<CarAIController>();
        if (targetCarController != null)
        {
            Debug.Log($"{gameObject.name} target has CarAIController");
        }
        else
        {
            Debug.Log($"{gameObject.name} target is player (no CarAIController)");
        }
        
        // 타겟의 예측 위치 계산
        CalculatePredictedTargetPosition();
        
        // 예측 위치를 향해 초기 방향 설정
        Vector3 directionToPredicted = (predictedTargetPosition - transform.position).normalized;
        if (directionToPredicted != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToPredicted);
        }
        
        Debug.Log($"{gameObject.name} targeting {newTarget.name} at predicted position {predictedTargetPosition}");
    }
    
    void CalculatePredictedTargetPosition()
    {
        if (target == null)
        {
            predictedTargetPosition = Vector3.zero;
            return;
        }
        
        Vector3 targetVelocity = Vector3.zero;
        
        // 타겟의 속도 계산 - 플레이어는 KartController 사용
        KartController kartController = target.GetComponent<KartController>();
        if (kartController != null)
        {
            // KartController의 sphere(Rigidbody)에서 속도 가져오기
            targetVelocity = kartController.sphere.velocity;
            Debug.Log($"Using KartController sphere velocity: {targetVelocity}");
        }
        else if (targetCarController != null)
        {
            // CarAIController가 있는 경우 (다른 AI 차량을 타겟으로 할 때)
            float speedMPS = targetCarController.kmh / 3.6f;
            targetVelocity = target.forward * speedMPS;
            Debug.Log($"Using CarAIController velocity: {targetVelocity}");
        }
        else
        {
            // Rigidbody가 직접 붙어있는 경우 (fallback)
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                targetVelocity = targetRb.velocity;
                Debug.Log($"Using direct Rigidbody velocity: {targetVelocity}");
            }
            else
            {
                Debug.Log("No velocity component found, using zero velocity");
            }
        }
        
        // 예측 위치 계산
        predictedTargetPosition = target.position + targetVelocity * predictionTime;
        
        Debug.Log($"Target current position: {target.position}, velocity: {targetVelocity.magnitude:F2}m/s, predicted position: {predictedTargetPosition}");
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