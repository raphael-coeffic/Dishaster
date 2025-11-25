using UnityEngine;

public class DanceNPC : MonoBehaviour
{
    [Header("Home Position")]
    [SerializeField] private bool useCustomHomePosition = false;
    [SerializeField] private Vector3 homePosition;

    [Header("Return Movement")]
    [SerializeField] private float returnSpeed = 2f;
    [SerializeField] private float maxOffset = 0.2f;

    [Header("Jump / Dance")]
    [SerializeField] private float jumpHeight = 0.25f;
    [SerializeField] private float jumpSpeed = 3f;

    private float timeOffset;

    private void Start()
    {
        if (!useCustomHomePosition)
            homePosition = transform.position;

        timeOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        Vector3 currentPos = transform.position;

        Vector2 currentXZ = new Vector2(currentPos.x, currentPos.z);
        Vector2 homeXZ = new Vector2(homePosition.x, homePosition.z);

        if (Vector2.Distance(currentXZ, homeXZ) > maxOffset)
        {
            Vector2 newXZ = Vector2.Lerp(currentXZ, homeXZ, Time.deltaTime * returnSpeed);
            currentPos.x = newXZ.x;
            currentPos.z = newXZ.y;
        }

        float t = Time.time + timeOffset;
        float yOffset = Mathf.Abs(Mathf.Sin(t * jumpSpeed)) * jumpHeight;

        currentPos.y = homePosition.y + yOffset;
        transform.position = currentPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(homePosition, 0.1f);
    }
}
