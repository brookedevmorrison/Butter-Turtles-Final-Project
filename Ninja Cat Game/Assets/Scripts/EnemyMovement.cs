using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerWaypoint;
    public float movementSpeed = 1.0f;

    void Update()
    {
        if (playerWaypoint != null)
        {
            // Calculate the direction towards the player's waypoint
            Vector3 direction = (playerWaypoint.position - transform.position).normalized;

            // Ensure that the enemy stays on the ground (restrict along the y-axis)
            direction.y = 0; // Set the y-component to zero to prevent vertical movement
            // Move towards the player's waypoint
            transform.position += direction * movementSpeed * Time.deltaTime;

            // Optionally, you can rotate the enemy to face the player's waypoint
            // Remove this section if you don't want the enemy to rotate
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f * Time.deltaTime);
        }
    }
}
