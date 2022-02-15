using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRepositionState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Reposition;
    }

    public void Enter(AiAgent agent)
    {
        agent.aiPath.enabled = true;
        agent.rs.enabled = true;
    }

    public void Exit(AiAgent agent)
    {
        
            agent.rs.enabled = false;
    }

    public void Update(AiAgent agent)
    {
        // Look at player
        Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;

        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        agent.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
