using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField] public Vector3 rotation;
    [SerializeField] public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime);
    }
}
