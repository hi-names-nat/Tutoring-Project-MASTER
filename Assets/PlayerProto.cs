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
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float Fallspeed = -9.81f;
    //Unserialized
    private CharacterController controller;
    private Vector2 val;



    private void Awake()
    {
        if (!TryGetComponent<CharacterController>(out controller))
        {
            Debug.LogError("No Character controller attached to player GameObject!");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
        controller.Move((new Vector3(val.x, 0, val.y)).normalized * movementSpeed * Time.deltaTime);
        print((new Vector3(val.x, 0, val.y)).normalized * movementSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        controller.Move(new Vector3(0, Fallspeed * Time.deltaTime, 0));
    }



    ///////////////////////////////////////////////////////////////////////////////////////////
    /// Input System Binds.
    /// Do not change the name of these binds unless you also change the name of the Input Action.
    /// Do not remove the "On"
    ///////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        val = value.Get<Vector2>();
        
    }

    void OnAttack(InputValue value)
    {
        print(value.Get<bool>());
    }

    void OnRun(InputValue value)
    {

    }

    void OnItem(InputValue value)
    {

    }

    void OnItem2(InputValue value)
    {

    }

    void OnItemMenu(InputValue value)
    {

    }

    void OnWheel(InputValue value)
    {

    }

    void OnPauseMenu(InputValue value)
    {
        
    }
}
