using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityProjectile : MonoBehaviour
{
    /// <summary>
    /// The speed at which the projectile moves
    /// </summary>
    public float Speed;

    /// <summary>
    /// Whether or not this projectile can damage the player
    /// </summary>
    public bool DamagePlayer;

    /// <summary>
    /// How much damage the projectile does
    /// </summary>
    public int Damage;

    /// <summary>
    /// How long in seconds the particle will last for
    /// </summary>
    public float Life;

    //How long we've been alive for
    private float TimeAlive;

	/// <summary>
	/// If we have a trail particle we want to destroy it afterwards, but unparent it and let it linger so it doesn't just disappear.
	/// </summary>
	public ParticleSystem TrailParticle;

    public GameObject OnDestroyCreate;

    public Transform Creator;

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
        Life -= Time.deltaTime;
        TimeAlive += Time.deltaTime;
        if(Life <= 0)
        {
			RemoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DamagePlayer && TimeAlive > 0.1f)
        {
            if (other.tag == "Player")
            {
                PlayerManagedHealth p = other.GetComponent<PlayerManagedHealth>();
                if (p != null)
                {
                    if (Creator == null)
                    {
                        p.DealHealth(Damage, true);
                    }
                    else
                    {
                        p.DealHealth(Damage, true);
					}
                    RemoveProjectile();
                }
            }
            else
            {
                RemoveProjectile();
            }


        }
    }

    public void RemoveProjectile()
    {
        if (OnDestroyCreate != null)
        {
            GameObject g = Instantiate(OnDestroyCreate);
            g.transform.position = transform.position;
            Destroy(g, 5f);
        }
		if(TrailParticle != null )
		{
			Destroy( TrailParticle.gameObject, 5f );
			TrailParticle.transform.SetParent( null );
			

		}

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (OnDestroyCreate != null)
        {

        }
    }

	public static Vector3 FirstOrderIntercept
	(
		Vector3 shooterPosition,
		Vector3 shooterVelocity,
		float shotSpeed,
		Vector3 targetPosition,
		Vector3 targetVelocity
	)
	{
		Vector3 targetRelativePosition = targetPosition - shooterPosition;
		Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
		float t = FirstOrderInterceptTime
		(
			shotSpeed,
			targetRelativePosition,
			targetRelativeVelocity
		);
		return targetPosition + t * (targetRelativeVelocity);
	}
	//first-order intercept using relative target position
	public static float FirstOrderInterceptTime
	(
		float shotSpeed,
		Vector3 targetRelativePosition,
		Vector3 targetRelativeVelocity
	)
	{
		float velocitySquared = targetRelativeVelocity.sqrMagnitude;
		if ( velocitySquared < 0.001f )
			return 0f;

		float a = velocitySquared - shotSpeed * shotSpeed;

		//handle similar velocities
		if ( Mathf.Abs( a ) < 0.001f )
		{
			float t = -targetRelativePosition.sqrMagnitude /
			(
				2f * Vector3.Dot
				(
					targetRelativeVelocity,
					targetRelativePosition
				)
			);
			return Mathf.Max( t, 0f ); //don't shoot back in time
		}

		float b = 2f * Vector3.Dot( targetRelativeVelocity, targetRelativePosition );
		float c = targetRelativePosition.sqrMagnitude;
		float determinant = b * b - 4f * a * c;

		if ( determinant > 0f )
		{ //determinant > 0; two intercept paths (most common)
			float t1 = (-b + Mathf.Sqrt( determinant )) / (2f * a),
					t2 = (-b - Mathf.Sqrt( determinant )) / (2f * a);
			if ( t1 > 0f )
			{
				if ( t2 > 0f )
					return Mathf.Min( t1, t2 ); //both are positive
				else
					return t1; //only t1 is positive
			}
			else
				return Mathf.Max( t2, 0f ); //don't shoot back in time
		}
		else if ( determinant < 0f ) //determinant < 0; no intercept path
			return 0f;
		else //determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max( -b / (2f * a), 0f ); //don't shoot back in time
	}

}
