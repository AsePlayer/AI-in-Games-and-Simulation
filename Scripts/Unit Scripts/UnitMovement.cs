using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 1f;

    public void move(Rigidbody2D rg, Vector2 movement)
    {
        rg.velocity = movement * moveSpeed;
    }
}
