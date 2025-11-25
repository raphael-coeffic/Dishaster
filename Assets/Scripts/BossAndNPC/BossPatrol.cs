using UnityEngine;


public class BossPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;  // Path points the boss will follow
    [SerializeField] private float speed = 3f;       // Movement speed

    private int currentIndex = 0;  // Current waypoint index

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentIndex];

        // Horizontal direction toward the waypoint
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        // If close enough to the waypoint → go to the next one
        if (direction.magnitude < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            return;
        }

        Vector3 moveDir = direction.normalized;

        // Move the boss
        transform.position += moveDir * speed * Time.deltaTime;

        // Rotate smoothly toward the movement direction
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.2f);
        }
    }
}
