using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerScript : MonoBehaviour
{
    public GameObject player;
    public MapGrid map;
    

    float timer; //so that it doesn't do such a complex calculation every frame

    // Start is called before the first frame update
    void Start()
    {
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.25f)
        {
            int playerx = Mathf.RoundToInt(player.transform.position.x);
            int playery = Mathf.RoundToInt(player.transform.position.y);
            int enemyx = Mathf.RoundToInt(gameObject.transform.position.x);
            int enemyy = Mathf.RoundToInt(gameObject.transform.position.y);
            timer = 0f;
            //if (miniMax(playerx, playery, enemyx, enemyy, map, 5, 0, ))
            {
                //pursue player
            }
            //else
            {
                //run away
            }
        }
    }

    //This miniMax function should calculate 5 moves ahead
    //The enemy pursues the player if once in line of sight, is 4 cells or closer to the player
    //The enemy runs away if once in line of sight, is more than 4 cells from the player
    //Moves are calculated by making a tree of moves, each spot representing the player and enemy moving 1 tile
    //bool miniMax(int playerx, int playery, int enemyx, int enemyy, MapGrid map, int maxDepth, int currentDepth, Path p)
    //{
        
    //}
}
