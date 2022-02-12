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

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.ChangeState(initialState);

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
