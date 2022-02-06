using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int health;

    /*
     * In order to make a function that needs to be overritten, do this:
     * 
     * protected virtual void bruh()
     * {
     * 
     * }
     * 
     * and then in the object you're overriding, do this:
     * 
     * protected override void bruh()
     * {
     *      // Do stuff
     *      base.bruh();
     * }
     * 
     */

    public int getMaxHP()
    {
        return maxHealth;
    }

    public void recieveDamage(int damage)
    {
        maxHealth -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
