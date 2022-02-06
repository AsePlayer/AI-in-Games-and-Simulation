using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected int startingHealth = 100;

    private int health;

    // Optimize with pooling system later
    //public event Action onDied;

    private void Awake()
    {
        health = startingHealth;
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Destroy(gameObject);
        /*
         * Eventually propegate this out to Pooling system or whateva instead
         * if(onDied != null)
         * {
         *      onDied();
         * }
         */
    }
}
