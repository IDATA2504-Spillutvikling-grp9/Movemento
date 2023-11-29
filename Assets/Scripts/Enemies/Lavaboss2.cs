using UnityEngine;

public class CaterpillarBoss2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 movementBounds = new Vector2(10f, 10f);
    public float changeDirectionInterval = 2f;

    private Vector2 targetPosition;
    private float timer;

    void Start()
    {
        // Initialize the target position to the current position
        targetPosition = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Change target position at intervals
        if (timer > changeDirectionInterval)
        {
            SetNewTargetPosition();
            timer = 0;
        }

        // Move towards the target position
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void SetNewTargetPosition()
    {
        float x = Random.Range(-movementBounds.x, movementBounds.x) + transform.position.x;
        float y = Random.Range(-movementBounds.y, movementBounds.y) + transform.position.y;

        targetPosition = new Vector2(x, y);
    }
}
