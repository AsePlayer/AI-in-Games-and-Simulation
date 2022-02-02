using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour
{
    /*
    1 = wall
    2 = grass
    3 = hole
    4 = goal
    */

    public int id;
    public bool passable;
    public bool cover;
    public bool goal;
    public bool closed = false;
    public float walkspeed;

    Vector3 coord;
    
    public MapCell(bool pass, bool cov, float ws)
    {
        passable = pass;
        cover = cov;
        walkspeed = ws;
    }

    public void setCoord(Vector3 c)
    {
        coord = c;
    }

    public Vector3 getCoord()
    {
        return coord;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
