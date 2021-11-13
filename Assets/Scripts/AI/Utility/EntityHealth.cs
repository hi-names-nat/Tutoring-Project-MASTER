﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    private AIEntity controller;
    private bool dead;

    [Header("Ragdoll Physics")]
    private Rigidbody[] ragdoll;

    [Header("Health")]
    [SerializeField]
    private float health;
    public float maxHealth;

    [Header("Death Events")]
    [SerializeField]
    private UnityEvent deathEvents;
    public List<MonoBehaviour> componentsToDisable;
	public GameObject DeathEffect;

    public bool IsDead => dead;
    public GameObject thingtoDestroy;
    public float deathTime = 20f;
    public float Health
    {
        get { return health; }
    }

    [SerializeField]
    private AudioSource hitSource;
    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private Vector2 hitPitchRange;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
        }
        health = maxHealth;
        controller = GetComponent<AIEntity>();
        dead = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            if (!dead)
            {
                dead = true;
                ActivateRagdoll();
                deathEvents.Invoke();

				if(DeathEffect != null )
				{
					GameObject g = Instantiate( DeathEffect );
					g.transform.position = this.transform.position;
					g.transform.SetParent( null );
					Destroy( g, 5f );
				}

            }
        }


        if (dead) {
            //set layer to deadenemy
            //print("set dead enemy");
            //gameObject.layer = 16;

            deathTime -= Time.deltaTime;
            if(deathTime <= 0)
            {
                

                if (thingtoDestroy != null)
                {
                    Destroy(thingtoDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }

                //GetComponent<EntityHealth>().enabled = false;
            }
            
        }


    }

    void ActivateRagdoll()
    {
        if (controller != null)
        {
            if (controller.Agent != null)
            {
                controller.Agent.enabled = false;
            }
            controller.enabled = false;
        }
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = false;
        }
        foreach(MonoBehaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
    }

    public void ExplodeRagdoll(Vector3 position, float force)
    {
        ActivateRagdoll();
        foreach(Rigidbody rb in ragdoll)
        {
            rb.AddExplosionForce(force, position, 5f, 2f);
        }
    }

    public void Damage(float h)
    {
        health -= h;
        PlayHitSound();
    }
    private void PlayHitSound()
    {
        if (hitSource != null)
        {
            hitSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            hitSource.pitch = Random.Range(hitPitchRange.x, hitPitchRange.y);
            hitSource.Play();
        }
    }
}
