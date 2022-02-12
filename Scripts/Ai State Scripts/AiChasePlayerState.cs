using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AiChasePlayerState : AiState 
{

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    void AiState.Enter(AiAgent agent)
    {
        agent.aiPath.enabled = true;

    }

    void AiState.Exit(AiAgent agent)
    {
        agent.aiPath.enabled = false;
    }


    void AiState.Update(AiAgent agent)
    {
        
    }

}
