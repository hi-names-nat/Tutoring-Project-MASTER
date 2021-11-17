using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Flee")]
public class FleeAction : AIAction
{
    public AIState stateToTransitionTo;
    public float fleeRadius;
    public override void Act(AIEntity controller)
    {
        Flee(controller);
        
    }

    private void Flee(AIEntity controller)
    {
        
    }
}