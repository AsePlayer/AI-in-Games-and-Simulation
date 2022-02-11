using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;


public class RunnerScript : MonoBehaviour
{
    public GameObject player;
    public MapGrid map;
    public Transform playerSpot;
    public GameObject runaway;

    private float scared;
    private List<int> scores;

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
        if (scared > 0)
        {
            scared -= Time.deltaTime;
        }
            else if (timer >= 0.25f)
            {
            //Debug.Log(gameObject.GetComponent<AIPath>().path.path[0].position);
            //Debug.Log(gameObject.transform.position);
                int playerx = Mathf.RoundToInt(player.transform.position.x);
                int playery = Mathf.RoundToInt(player.transform.position.y);
                int enemyx = Mathf.RoundToInt(gameObject.transform.position.x);
                int enemyy = Mathf.RoundToInt(gameObject.transform.position.y);
                timer = 0f;
            int prio = miniMaxList(playerx, playery, enemyx, enemyy, map, 0, 5, gameObject.GetComponent<AIPath>().path, true);
            Debug.Log(prio);
            if (prio > 0)
            {
                gameObject.GetComponent<AIDestinationSetter>().target = playerSpot;
            }
            else
            {
                int randx;
                int randy;
                int tries = 0;
                do
                {
                    randx = Random.Range(1, map.width - 2);
                    randy = Random.Range(1, map.height - 2);
                    tries++;
                } while ((Mathf.Abs(playerx - enemyx) > Mathf.Abs(playerx - randx) || Mathf.Abs(playery - enemyy) > Mathf.Abs(playery - randy) ||
                !map.getCell(randx, randy).GetComponent<MapCell>().passable) && tries <= 50);

                //Debug.Log(randx + " " + randy);
                runaway.transform.position = new Vector2(randx, randy);
                gameObject.GetComponent<AIDestinationSetter>().target = runaway.transform;
                scared = 1f;
            }
        }
    }

        //This miniMax function should calculate 5 moves ahead
        //The enemy pursues the player if once in line of sight, is 4 cells or closer to the player
        //The enemy runs away if once in line of sight, is more than 4 cells from the player
        //Moves are calculated by making a tree of moves, each spot representing the player and enemy moving 1 tile
    int miniMaxList(int playerx, int playery, int enemyx, int enemyy, MapGrid map, int depth, int maxDepth, Path p, bool enemyTurn)
    {
        if (depth >= maxDepth)
        {
            //Score is determined:
            //Within 4 cells of player = 100 (max score possible)
            //Otherwise, farther distances score lower (100 - distance)
            //In line of sight of player outside 4 cells is very bad, score turns negative.
            float rayDist = Mathf.Sqrt((playerx - enemyx) * (playerx - enemyx) + (playery - enemyy) * (playery - enemyy));
            if (rayDist <= 2)
            {
                return 100;
            }
            else
            {
                int score1 = 100 - (int)rayDist;
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(enemyx, enemyy), new Vector2(playerx - enemyx, playery - enemyy), rayDist);
                //Debug.Log(hit.collider);
                if (hit.collider == null || hit.collider.gameObject.name != "Wall(Clone)")
                {
                    Debug.DrawRay(new Vector2(playerx, playery), new Vector2(enemyx - playerx, enemyy - playery), Color.red, 0.3f);
                    score1 *= -1;
                }
                else
                {
                    Debug.DrawRay(new Vector2(playerx, playery), new Vector2(enemyx - playerx, enemyy - playery), Color.blue, 0.3f);
                }
                return score1;
            }
        }
        else
        {
            if (enemyTurn)
            {
                //checks to see for optimal a* path or standstill first (much faster to compute)
                if (gameObject.GetComponent<AIPath>().path.path.Count > depth + 1)
                {
                    int nextx = (gameObject.GetComponent<AIPath>().path.path[depth + 1].position.x - 37) / 1000;
                    int nexty = (gameObject.GetComponent<AIPath>().path.path[depth + 1].position.y + 21) / 1000;
                    //Debug.Log((gameObject.GetComponent<AIPath>().path.path[depth + 1].position.x - 37) + " " + (gameObject.GetComponent<AIPath>().path.path[depth + 1].position.y + 27));
                    //Debug.Log(nextx + " " + nexty);
                    //return Mathf.Max(miniMaxList(playerx, playery, nextx, nexty, map, depth + 1, maxDepth, p, !enemyTurn), miniMaxList(playerx, playery, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                    return miniMaxList(playerx, playery, nextx, nexty, map, depth + 1, maxDepth, p, !enemyTurn);
                }
                else
                {
                    //checks for available moves and adds them as possibilities
                    List<int> vals = new List<int>();
                    if (map.getCell(enemyx, enemyy-1).GetComponent<MapCell>().passable)
                    {
                        vals.Add(miniMaxList(playerx, playery, enemyx, enemyy-1, map, depth + 1, maxDepth, p, !enemyTurn));
                    }
                    if (map.getCell(enemyx-1, enemyy).GetComponent<MapCell>().passable)
                    {
                        vals.Add(miniMaxList(playerx, playery, enemyx-1, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                    }
                    if (map.getCell(enemyx, enemyy+1).GetComponent<MapCell>().passable)
                    {
                        vals.Add(miniMaxList(playerx, playery, enemyx, enemyy+1, map, depth + 1, maxDepth, p, !enemyTurn));
                    }
                    if (map.getCell(enemyx+1, enemyy).GetComponent<MapCell>().passable)
                    {
                        vals.Add(miniMaxList(playerx, playery, enemyx+1, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                    }
                    //standing still
                    vals.Add(miniMaxList(playerx, playery, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));

                    //returns the least of the list
                    return vals.Max();
                }
                
            }
            else
            {
                //checks for available moves and adds them as possibilities
                List<int> vals = new List<int>();
                if (map.getCell(playerx, playery - 1).GetComponent<MapCell>().passable)
                {
                    vals.Add(miniMaxList(playerx, playery - 1, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                }
                if (map.getCell(playerx - 1, playery).GetComponent<MapCell>().passable)
                {
                    vals.Add(miniMaxList(playerx - 1, playery, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                }
                if (map.getCell(playerx, playery + 1).GetComponent<MapCell>().passable)
                {
                    vals.Add(miniMaxList(playerx, playery + 1, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                }
                if (map.getCell(playerx + 1, playery).GetComponent<MapCell>().passable)
                {
                    vals.Add(miniMaxList(playerx + 1, playery, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));
                }
                //standing still
                vals.Add(miniMaxList(playerx, playery, enemyx, enemyy, map, depth + 1, maxDepth, p, !enemyTurn));

                //returns the least of the list
                return vals.Min();

            }
        }
    }
    ///*
    bool miniMax()
    {
        return true;
    }
        //*/
}
