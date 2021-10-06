using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface managedHealth
{
    public void DealHealth(int dealt, bool useDefense = true);
}

class standardManagedHealth : MonoBehaviour, managedHealth
{
    [SerializeField] public int maxHealth { get; protected set; }

    public int currentHealth { get; protected set; }

    public float defenseValue { get; protected set; } = 0;

    public void DealHealth(int dealt, bool useDefense = true)
    {
        if (!useDefense)
        {
            currentHealth -= dealt; return;
        }

        currentHealth -= (int)Mathf.Floor(dealt - ((float)dealt * defenseValue));
    }
}