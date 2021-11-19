using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] PlayerProto player;
    [SerializeField] Mesh mesh;
    [SerializeField] Collider collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject && player.gameState.hasKey())
        {
            player.gameState.useKey();
            GetComponent<MeshFilter>().mesh = mesh;
            collider.enabled = false;
        }
    }
}
