using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Movement module that can be added to any Object.
 * They will gain the ability to pass in their speed, rigidbody2d, and vector2 movement to impact their velocity.
 */

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 1f;

    public void move(Rigidbody2D rg, Vector2 movement)
    {
        rg.velocity = movement * moveSpeed;
    }
}
