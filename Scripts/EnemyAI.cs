using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from https://youtu.be/db0KWYaWfeM

public class EnemyAI : MonoBehaviour
{
    private Vector3 startingPosition;
    private Pathfinding pathfinding;
    List<PathNode> path;

    public int speed;
    public bool isZombie;

    private void Start()
    {
        startingPosition = transform.position;
        pathfinding = new Pathfinding(10, 10, -50);
        
    }

    private Vector3 getRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        return startingPosition + randomDirection * Random.Range(10f, 70f);
    }

    private void Update()
    {
        path = pathfinding.FindPath(
            (int)(transform.position.x / 10),
            (int)(transform.position.y / 10),
            (int)(GameObject.Find("Player").transform.position.x / 10),
            (int)(GameObject.Find("Player").transform.position.y / 10)
            );

        ///*
        for (int i = 0; i < path.Count - 1; i++)
        {
            /*
            if(Vector3.Distance(transform.position, path[i].toVector()) > .0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[i + 1].x, path[i + 1].y, -1) * 10f + Vector3.one * 5f, 10 * Time.deltaTime);
            }
            */
            Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 0.1f, false);
        }
        //*/

        // Enemy will always follow player unless they are a zombie. A zombie will follow within a limited agro range.
        if ((path.Count > 1 && !isZombie) || (path.Count > 1 && path.Count < 6 && isZombie))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[1].x, path[1].y, -1) * 10f + Vector3.one * 5f, speed * Time.deltaTime);
        }
        
    }

}
