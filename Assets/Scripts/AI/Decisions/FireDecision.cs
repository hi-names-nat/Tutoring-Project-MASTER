using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Fire Weapon")]
public class FireDecision : AIDecision
{

    public float FireDistance = 1000;
    public override bool Decide(AIEntity controller)
    {
        if (controller.FireTimer == 0f)
        {
            Vector2 v = controller.EnemyStats.fireCooldown;
            controller.FireTimer = Random.Range(v.x, v.y);
            return false;
        }
        else
        {
            if (controller.DistanceToPlayer() < FireDistance)
            {
                controller.FireTimer -= Time.deltaTime;
            }
            else
            {
                Vector2 v = controller.EnemyStats.fireCooldown;
                controller.FireTimer = Random.Range(v.x, v.y);
            }

            if(controller.FireTimer <= 0)
            {
                return true;
            }
        }
        return false;

        
    }

    private bool Fire(AIEntity controller)
    {

        return true;
    }
}
