using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.TryGetComponent(out Health hp))
        {
            Debug.Log("Has Health");
            if (hp.getHealth() < hp.getMaxHealth())
            {
                Debug.Log("Can Use");
                hp.takeDamage(-50);
                Destroy(this);
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.TryGetComponent(out Health hp))
        {
            //Debug.Log("Has Health");
            if (hp.getHealth() < hp.getMaxHealth())
            {
                //Debug.Log("Can Use");
                hp.takeDamage(-50);
                Destroy(gameObject);
            }
        }
    }
}
