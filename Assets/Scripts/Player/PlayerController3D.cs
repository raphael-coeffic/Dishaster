using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController3D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (Keyboard.current == null)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float h = 0f;
        float v = 0f;

        if (Keyboard.current.upArrowKey.isPressed) v += 1f;
        if (Keyboard.current.downArrowKey.isPressed) v -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed) h += 1f;
        if (Keyboard.current.leftArrowKey.isPressed) h -= 1f;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;

        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        rb.linearVelocity = moveDir * moveSpeed;

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            if (Vector3.Dot(moveDir, transform.forward) > -0.5f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.15f);
            }
        }
    }
}

