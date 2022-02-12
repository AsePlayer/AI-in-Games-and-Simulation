using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
}
