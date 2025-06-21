using UnityEngine;

namespace Peque.Traffic
{
    public class CharacterNavigationController : WaypointNavigator
    {
        public float movementSpeed = 1f;
        public float rotationSpeed = 4f;
        public float raycastHeight = 50f;
        public LayerMask groundMask = ~0;

        private Animator animator;
        private Rigidbody rb;
        private WaypointNavigator navigator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
            navigator = GetComponent<WaypointNavigator>();
        }

        private void Update()
        {
            if (reachedDestination)
            {
                navigator.getNextWaypoint();
                if (reachedDestination)
                {
                    animator.SetFloat("Speed", 0);
                    return;
                }
            }
        }

        private void FixedUpdate()
        {
            // 1) 수평 방향만 계산
            Vector3 dir = destination - rb.position;
            Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
            // if (flatDir.sqrMagnitude < 0.01f)
            // {
            //     animator.SetFloat("Speed", 0);
            //     return;
            // }

            // 2) 애니메이터 속도 세팅
            // animator.SetFloat("Speed", movementSpeed);

            // 3) 이동할 XZ 좌표 계산
            Vector3 nextPos = rb.position + flatDir.normalized * movementSpeed * Time.fixedDeltaTime;

            // 4) 바닥 콜라이더 추적: raycast로 y 높이 맞춤
            Ray ray = new Ray(new Vector3(nextPos.x, rb.position.y + raycastHeight, nextPos.z), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastHeight * 2f, groundMask))
                nextPos.y = hit.point.y;

            rb.MovePosition(nextPos);

            // 5) 회전: 수평 회전만
            Quaternion targetRot = Quaternion.LookRotation(flatDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
