using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

interface managedHealth
{
    public void DealHealth(int dealt, bool useDefense = true);

}

public class standardManagedHealth : MonoBehaviour, managedHealth
{
    public UnityEvent onDeath;

    [SerializeField] public int maxHealth;

     public int currentHealth { get; protected set; }

    public float defenseValue { get; protected set; } = 0;

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    virtual public void DealHealth(int dealt, bool useDefense = true)
    {
        
        if (!useDefense)
        {
            currentHealth -= dealt;
        }
        else currentHealth -= (int)Mathf.Floor(dealt - ((float)dealt * defenseValue));

        if (currentHealth <= 0) onDeath.Invoke();
    }

    virtual public void PushBack(float factor, Vector3 direction)
    {
        GetComponent<NavMeshAgent>().Move(direction * factor);
    }
}