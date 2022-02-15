using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This script serves as a general-purpose Health module that can be added to any Object.
 * They will gain the ability to takeDamage and die, and bullets will be causing damages.
 */

public class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 100;
    [SerializeField] private int health;

    AiAgent agent;

    // Optimize with pooling system later
    //public event Action onDied;

    private void Awake()
    {
        health = startingHealth;
        agent = GetComponent<AiAgent>();
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
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        // deathState.whatever to access it
        agent.stateMachine.ChangeState(AiStateId.Death);
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
