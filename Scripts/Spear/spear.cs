using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spear : MonoBehaviour
{
    [SerializeField] private float speed, lifetime, damage;
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
        transform.localScale = new Vector3(direction.x, transform.localScale.y, transform.localScale.z);
    }

    // Detects whether the spear is hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Spear Hit");
        if (collision.gameObject.tag != "Player")
        {
            collision.gameObject.GetComponent<Health>()?.Damage(damage);
            Destroy(gameObject);
        }
            
    }
}
