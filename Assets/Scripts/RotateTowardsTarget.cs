using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    [SerializeField] private float angularSpeed = 90f; // Degrees per second
    private Transform target;    

    void Update()
    {
        if (target == null) return;

        // Calculate the direction from Object A to Object B
        Vector3 directionToTarget = target.position - transform.position;

        if (directionToTarget == Vector3.zero) return;

        // Determine the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Rotate towards target rotation at given angular speed
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            angularSpeed * Time.deltaTime
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
