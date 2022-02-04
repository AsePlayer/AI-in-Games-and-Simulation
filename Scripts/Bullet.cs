using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       //rb.AddForce(transform.up * 5f);
    }

    // TODO: bullet despawns after 5 seconds
    // TODO: bullet gets destroyed on obstacles/enemies
    // TODO: bullet inflicts damage
}
