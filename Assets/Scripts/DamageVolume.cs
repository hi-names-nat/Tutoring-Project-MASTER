using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] bool doPushBack;
    [SerializeField] PlayerProto referenceObj;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.TryGetComponent<standardManagedHealth>(out standardManagedHealth h))
        {
            if (doPushBack) h.PushBack(1.5f, referenceObj.transform.forward);
            h.DealHealth(damage, false);
        }
    }
}
