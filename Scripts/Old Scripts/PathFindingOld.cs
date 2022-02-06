using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from: https://youtu.be/alU04hvz6L4
public class PathfindingOld
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;



    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public PathfindingOld(int width, int height, int offset)
    {
        grid = new Grid<PathNode>(width, height, 10f, new Vector3(0, 0, 0), (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.getGridObject(startX, startY);
        PathNode endNode = grid.getGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.getWidth(); x++)
        {
            for(int y = 0; y < grid.getHeight(); y++)
            {
                PathNode pathNode = grid.getGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.calculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = calculateDistanceCost(startNode, endNode);
        startNode.calculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = getLowestCostNode(openList);
            if(currentNode == endNode)
            {
                // Reached final node
                return calculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighborNode in getNeighborList(currentNode))
            {
                if (closedList.Contains(neighborNode)) continue;
                int tentativeGCost = currentNode.gCost + calculateDistanceCost(currentNode, neighborNode);
                if(tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = calculateDistanceCost(neighborNode, endNode);
                    neighborNode.calculateFCost();

                    if(!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }

        }

        // Out of nodes on the open list
        return null;

    }

    private List<PathNode> getNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if(currentNode.x - 1 >= 0)
        {
            // Left
            neighborList.Add(grid.getNode(new Vector3(currentNode.x - 1, currentNode.y)));
            // Left Down
            if(currentNode.y - 1 >= 0) neighborList.Add(grid.getNode(new Vector3(currentNode.x - 1, currentNode.y - 1)));
            // Left Up
            if(currentNode.y + 1 < grid.getHeight()) neighborList.Add(grid.getNode(new Vector3(currentNode.x - 1, currentNode.y + 1)));
        }
        if (currentNode.x + 1 < grid.getWidth())
        {
            // Right
            neighborList.Add(grid.getNode(new Vector3(currentNode.x + 1, currentNode.y)));
            // Right Down
            if (currentNode.y - 1 >= 0) neighborList.Add(grid.getNode(new Vector3(currentNode.x + 1, currentNode.y - 1)));
            // Right Up
            if (currentNode.y + 1 < grid.getHeight()) neighborList.Add(grid.getNode(new Vector3(currentNode.x + 1, currentNode.y + 1)));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighborList.Add(grid.getNode(new Vector3(currentNode.x, currentNode.y - 1)));
        // Up
        if (currentNode.y - 1 >= 0) neighborList.Add(grid.getNode(new Vector3(currentNode.x, currentNode.y + 1)));

        return neighborList;
    }

    private List<PathNode> calculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int calculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode getLowestCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public Grid<PathNode> getGrid()
    {
        return grid;
    }
}
