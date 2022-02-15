using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent)
    {
       
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public void Update(AiAgent agent)
    {
        // Look at player
        Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;

        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        agent.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
