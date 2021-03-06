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
            print(currentHealth);
            if (currentTime >= InvulnerableTime)
            {
                currentTime = 0;
                isInvulnerable = false;
            }
        }
    }

    override public void DealHealth(int dealt, bool useDefense = true)
    {
        print("player says ow");
        if (isInvulnerable) return;
        isInvulnerable = true;
        base.DealHealth(dealt, useDefense);
    }

    override public void PushBack(float factor, Vector3 direction)
    {
        GetComponent<CharacterController>().transform.Translate(direction * factor);
    }
}
