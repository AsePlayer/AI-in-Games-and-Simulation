using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Bullet module that can be added to any projectile Object.
 * The bullet will gain the ability to despawn after a certain amount of time, get destroyed when colliding with objects, and ignore holes/owner collision.
 */

public class Bullet : MonoBehaviour
{
    public GameObject owner;

    Rigidbody2D rb;
    float angle;
    float timer;

    [SerializeField]
    int damage;
    [SerializeField]
    public float speed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.GetComponent<Health>() != null && collision.gameObject != owner)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Health>().takeDamage(damage);
        }
    }

    void OnEnable()
    {
        // Bullets will ignore Hole collision while Holes still maintain their physical blocking abilities for Units.
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Hole");
        foreach (GameObject hole in holes)
        {
            Physics2D.IgnoreCollision(hole.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}