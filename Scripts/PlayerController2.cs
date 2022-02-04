using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{

    public float speed = 10;
    public Rigidbody2D rg;
    Vector2 movement;

    // Changed from Update to FixedUpdate. Inconsistent speed depending on window size with just Update -Ryan
    public void FixedUpdate()
    {
        // Use GetRawAxis for Keyboards to get snappy movement. GetAxis is floaty and smooth because it is meant for controllers or something with "sensitivity".
        // https://medium.com/@yousafzai.kamran60/unity-getaxis-vs-getaxisraw-ac501ad8f22

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector3(horizontalInput, verticalInput, 0);
        movement = movement.normalized * speed * Time.deltaTime;

        //rg.MovePosition(rg.position + movement * speed * Time.fixedDeltaTime);
        rg.velocity = movement;
    }

}
