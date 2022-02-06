using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private PathfindingOld pathfinding;
    private void Start()
    {
        pathfinding = new PathfindingOld(10, 10, -50);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = getMouseWorldPosition();
            pathfinding.getGrid().getXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f, false);
                }
            }
        }
        /*
        List<PathNode> path2 = pathfinding.FindPath((int)(GameObject.Find("Player").transform.position.x/10), 
            (int)(GameObject.Find("Player").transform.position.y/10), (int)(GameObject.Find("Enemy").transform.position.x/10), 
            (int)(GameObject.Find("Enemy").transform.position.y/10));
        for (int i = 0; i < path2.Count - 1; i++)
        {
            //Debug.DrawLine(new Vector3(path2[i].x, path2[i].y) * 10f + Vector3.one * 5f, new Vector3(path2[i + 1].x, path2[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 0.1f, false);
        }
        */
    }

    public static Vector3 getMouseWorldPosition()
    {
        Vector3 vec = getMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 getMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
