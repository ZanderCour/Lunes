using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;  // Player's Transform to follow
    public float smoothness = 5.0f;  // Adjust this to control the smoothness of the camera movement
    public float fixedHeight = 2.0f;  // Fixed height of the camera above the player

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned to the camera.");
            return;
        }

        // Calculate the desired position based on the player's position and the fixed height
        Vector3 desiredPosition = new Vector3(target.position.x, fixedHeight, target.position.z);

        // Use SmoothDamp to interpolate the current position to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothness);

        // Look at the player's position
        transform.LookAt(target.position);
    }
}
