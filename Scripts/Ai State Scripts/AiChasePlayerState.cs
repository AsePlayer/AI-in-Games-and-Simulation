using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AiChasePlayerState : AiState 
{


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
        // Look at player
        Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;
        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        agent.transform.eulerAngles = new Vector3(0, 0, angle);

        //agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * 2);

        //Vector2 v = agent.rb.velocity;
        //var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //agent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
    }

}
