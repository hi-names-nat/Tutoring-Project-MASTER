using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[ExecuteInEditMode]
public class PlayerProto : MonoBehaviour
{
    //Public
    // My public variables start with a capital.

    //Serialized
    // My serialized private variables start with a lowercase.

    [Tooltip("The speed at which the player moves by default.")]
    [SerializeField] float movementSpeed = 3f;
    [Tooltip("The accelleration at which the player will fall.")]
    [SerializeField] float fallAcceleration = -3.27f;
    [Tooltip("The max speed at which the player can fall.")]
    [SerializeField] float maxFallSpeed = -4.0f;
    
    
    //Unserialized
    // My unserialized private fields start with an underscore.

    /// <summary>
    /// Reference to the controller.
    /// </summary>
    private CharacterController _controller;
    /// <summary>
    /// The value of the current movement.
    /// </summary>
    private Vector2 _val;
    /// <summary>
    /// the last speed that the player had fallen.
    /// </summary>
    float _lastFallSpeed = 0;

    private void Awake()
    {
        if (!TryGetComponent<CharacterController>(out _controller))
        {
            Debug.LogError("No CharacterController attached to player GameObject!");
        }

    }

    void Update()
    {
        _controller.Move(new Vector3(_val.x * Time.deltaTime, 0, _val.y * Time.deltaTime) * movementSpeed);
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
    }

    private void FixedUpdate()
    {
        doGravity();
    }

    private void doGravity()
    {
        _controller.Move(new Vector3(0, _lastFallSpeed));
        if (!_controller.isGrounded && _lastFallSpeed > maxFallSpeed)
            _lastFallSpeed += fallAcceleration * Time.deltaTime;
        else if (_controller.isGrounded)
            _lastFallSpeed = -.01f;
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// Input System Binds.
    /// Do not change the name of these binds unless you also change the name of the Input Action.
    /// Do not remove the "On"
    ///////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Our movement code. Gets our vector from our input, and stores it in a class variable.
    /// </summary>
    /// <param name="value">The input value, specifically a Vector2</param>
    void OnMove(InputValue value)
    {
        _val = value.Get<Vector2>().normalized;
        if (_val != Vector2.zero)
            transform.forward = new Vector3(_val.x, 0, _val.y);
    }

    void OnAttack(InputValue value)
    {
        //just evoke an animation
    }

    void OnRun(InputValue value)
    {
        //step-up to a new speed, when released step down.
    }

    void OnItem(InputValue value)
    {
        //evoke item
    }

    void OnItem2(InputValue value)
    {
        //evoke item2
    }

    void OnItemMenu(InputValue value)
    {
        //evoke the item menu
    }

    void OnWheel(InputValue value)
    {
        //evoke the item wheel
    }

    void OnPauseMenu(InputValue value)
    {
        //Evoke the pause menu
    }
}
