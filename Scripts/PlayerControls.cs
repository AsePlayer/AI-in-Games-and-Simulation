using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float speed = 10;
    public Transform playerPos;

	public void Update()
    {
        // Use GetRawAxis for Keyboards to get snappy movement. GetAxis is floaty and smooth because it is meant for controllers or something with "sensitivity".
        // https://medium.com/@yousafzai.kamran60/unity-getaxis-vs-getaxisraw-ac501ad8f22

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0);
        movement = movement.normalized * speed * Time.deltaTime;

        playerPos.transform.position += movement;
    }

}
