using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform camera;
    public Rigidbody2D player;

    public bool gameBegan;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        gameBegan = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !gameBegan)
        {
            Time.timeScale = 1;
            gameBegan = true;
        }
        camera.transform.position = new Vector3(player.position.x, player.position.y, -100);
    }
}
