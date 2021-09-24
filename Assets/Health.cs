using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{


    [SerializeField] float maxHealth;
    [SerializeField] float startingHealth;
    float currentHealth;

    [SerializeField] float deathTime;
    [SerializeField] UnityEvent onDeath;

    private void Awake()
    {
       
    }
}
