using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{

    float rotationX = 0f;
    float rotationY = 0f;

    public float sensitivity = 15f;
    // Update is called once per frame
    void Update()
    {
        rotationY += Input.GetAxis("Horizontal") * sensitivity;
        rotationX += Input.GetAxis("Vertical") * -1 * sensitivity;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

    }
}
