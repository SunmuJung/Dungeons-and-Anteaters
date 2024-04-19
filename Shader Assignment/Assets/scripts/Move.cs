using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1.5f;

    public Transform point1;
    public Transform point2;
    public Transform sphere;
    int direction = 1;
    void Start()
    {
    }
    void Update()
    {
        Vector3 target = currentMovementTarget();
        sphere.position= Vector3.Lerp(sphere.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector3)sphere.position).magnitude;
        if(distance < 0.01f)
        {
            direction *= -1;
        }
    }

    public Vector3 currentMovementTarget()
    {
        if (direction == 1)
        {
            return point1.position;
        }
        else
        {
            return point2.position;
        }
    }
}
