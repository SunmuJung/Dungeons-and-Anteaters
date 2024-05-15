using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spear : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroyspear", 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.y == 0)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right *-1* speed * Time.deltaTime);
        }
    }
    void Destroyspear()
    {
        Destroy(gameObject);
    }

}
