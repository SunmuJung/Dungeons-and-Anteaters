using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spear : MonoBehaviour
{
    [SerializeField] private float speed, lifetime;
    private Vector2 direction = Vector2.right;
    public Vector2 Direction {set { direction = value;} }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

}
