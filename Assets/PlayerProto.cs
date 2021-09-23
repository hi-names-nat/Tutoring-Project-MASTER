using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerProto : MonoBehaviour
{
    //Public

    //Serialized

    //Unserialized
    private CharacterController controller;
    private PlayerInput input;
    

    private void Awake()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("No Character controller attached to player GameObject!");
        }
        if (!TryGetComponent<PlayerInput>(out input))
        {
            Debug.LogError("No Player Input component attached");
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
