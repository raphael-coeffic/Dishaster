using UnityEngine;

public class ConeFollower : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform boss;   // Reference to the boss to follow
    [SerializeField] private float forwardOffset = 2.5f;  // Distance in front of the boss
    [SerializeField] private float heightOffset = 0.5f;   // Height offset above the ground

    private void LateUpdate()
    {
        if (boss == null)
            return;

        // Position the cone in front of the boss with a small height offset
        transform.position = boss.position + boss.forward * forwardOffset + Vector3.up * heightOffset;

        // Align the cone's rotation with the boss's forward direction
        transform.rotation = Quaternion.LookRotation(boss.forward);
    }
}

