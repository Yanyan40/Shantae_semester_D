using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_follow : MonoBehaviour
{
    public float FollowSpeed = 0.125f;
    public Transform target;
    public float yOffset = 1f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, FollowSpeed);
    }
}
