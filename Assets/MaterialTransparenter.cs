using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransparenter : MonoBehaviour
{
    [SerializeField] PlayerProto player;
    [SerializeField] float transparency;
    RaycastHit[] hits;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hits != null)
            foreach (RaycastHit h in hits)
            {
                if (h.collider.GetComponent<Renderer>() != null)
                {
                    Color color = h.collider.GetComponent<Renderer>().material.color;
                    color = h.collider.GetComponent<Renderer>().material.color;
                    h.collider.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1);
                }
            }
        int layerMask;
        layerMask = 1;
        print("is here");
        Debug.DrawRay(transform.position, player.transform.position - transform.position);
        hits = Physics.SphereCastAll(transform.position, .5f, player.transform.position - transform.position, layerMask, (int)QueryTriggerInteraction.Ignore);
        foreach (RaycastHit h in hits)
        {
            if (h.collider.gameObject == player.gameObject) return;
            if (h.collider.GetComponent<Renderer>() != null)
            {
                print(h.collider.gameObject.name);
                Color color = h.collider.GetComponent<Renderer>().material.color;
                h.collider.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, transparency);
            }
        }

    }
}
