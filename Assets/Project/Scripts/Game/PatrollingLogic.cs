using UnityEngine;

public class PatrollingLogic : MonoBehaviour {


    public float patrolSpeed = 5f;
    public float patrolRange = 10f;
    public int rotationSpeed = 5;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField] private bool simpleStrongEnemy;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        SetNewRandomTargetPosition();
    }


    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // Calculate the direction to the target position
        Vector3 direction = targetPosition - transform.position;

        // Check if the target position has been reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewRandomTargetPosition();
        }
        else if (!simpleStrongEnemy)
        {
            // Rotate the GameObject to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SetNewRandomTargetPosition()
    {
        // Generate random direction within the patrol range
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomOffset = new Vector3(randomDirection.x, 0f, randomDirection.y) * Random.Range(0f, patrolRange);

        // Set the new target position within the restricted area
        targetPosition = startPosition + randomOffset;
    }
}
