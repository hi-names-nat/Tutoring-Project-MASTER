using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [SerializeField] int damage = 5;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent<standardManagedHealth>(out standardManagedHealth h))
        {
            h.DealHealth(damage, false);
        }
    }
}
