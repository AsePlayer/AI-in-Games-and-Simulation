using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{

    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public AiAgentConfig config;


    public Seeker seeker;
    public Rigidbody2D rb;
    public AIPath aiPath;
    public Transform target;
    public RunnerScript rs;

    public Transform t;
    public Transform aimGunEndPointTransform;

    public AimWeapon aim;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiRepositionState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(initialState);

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        rs = GetComponent<RunnerScript>();
        t = GetComponent<Transform>();
        aim = GetComponent<AimWeapon>();
        aimGunEndPointTransform = GameObject.Find("Aim").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
