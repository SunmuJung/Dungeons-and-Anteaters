using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireFist : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private float skillTime;
    [SerializeField] private float damage;
    private Rigidbody2D rb;
    [SerializeField] private float radius, range;
    [SerializeField] private LayerMask attackLayer;
    private PlayerMovement playerMovement;

    void Start()
    {
        Invoke("DestroyFireFist", skillTime);
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
        Vector2 dir = Vector2.left;
        if (playerMovement.FacingRight)
        {
            dir = Vector2.right;
        }
        rb.velocity = dir * speed;

        // RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, dir, range, attackLayer);
        // hit.collider?.GetComponent<Health>().Damage(damage);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, dir, range, attackLayer);
        foreach (RaycastHit2D hit in hits)
        {
            Health healthComponent;
            if (hit.collider.TryGetComponent<Health>(out healthComponent))
            {
                healthComponent.Damage(damage); // does this change the pointer or the original value?
            }
        }


    }


    private void DestroyFireFist()
    {
        Destroy(gameObject);
    }
}
