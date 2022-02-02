using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from: https://youtu.be/alU04hvz6L4
public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + ", " + y;
    }

    public void calculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector3 toVector()
    {
        return new Vector3(x, y, 0);
    }

}
