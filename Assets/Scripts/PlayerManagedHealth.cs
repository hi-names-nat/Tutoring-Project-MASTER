using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagedHealth : standardManagedHealth
{
    [SerializeField] float InvulnerableTime = .5f;
    float currentTime = 0f;
    bool isInvulnerable = false;

    private void Update()
    {
        if (isInvulnerable)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= InvulnerableTime)
            {
                currentTime = 0;
                isInvulnerable = false;
            }
        }    
    }

    new public void DealHealth(int dealt, bool useDefense = true)
    {
        if (isInvulnerable) return;
        isInvulnerable = true;
        base.DealHealth(dealt, useDefense);
    }
}
