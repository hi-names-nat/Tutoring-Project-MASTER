    /*******************************************************************************************************
 * 
 * ***NAME***
 * FPSCharacterMovement.cs
 * 
 * ***DESCRIPTION***
 * A simple all-in-one First-Person controller.
 * 
 * ***USAGE***
 * Place this script onto a gameobject with rigidbody and capsule OR box collider components, with the main camera childed to this object.
 * 
 * ***AUTHOR***
 * Natalie Soltis
 * 
 * ***PURPOSE***
 * This code was written for an unreleased two-week long Game Jam project, which will eventually find a release on itch.io.
 * 
 ******************************************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

//requirements for script
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Movement_FPS : MonoBehaviour
{
    //Static variables 
    private static float SPEED = 10f;

    //Serialized vars

    /*
     * @name: playerSpeed
     * @desc: Speed for the player. Used to calculate speed, both on the ground and in the air.
     */
    [Header("Ground movement options")]
    [SerializeField]
    [Tooltip("The player's speed")]
    private float playerSpeed = SPEED;

    /****************
    * For next iteration. 
    * 
    * //[SerializeField]
    * //[Tooltip("Curve of the player's accel")]
    * //private AnimationCurve acelerationCurve;
    *
    * //[SerializeField]
    * //[Tooltip("Curve of the player's decel")]
    * //private AnimationCurve decelerationCurve;
    ****************/

    /// <summary>
    /// XML GJAKWK"J:   
    /// </summary>
    [SerializeField]
    [Tooltip("The angle the player can look up/down")]
    private float maxYAngle = 90f;

    /*
     * @name: jumpHeight
     * @desc: height that the player can jump. Applied with a twenty-times multiplier as a rigidbody.addforce().
     */
    [SerializeField]
    [Tooltip("Height the player can jump")]
    public float jumpHeight = 10f;

    /// <summary>
    /// multiplied to the applied speed of the player when the Sprint key is pressed.
    /// </summary>
    public float sprintModifier = 1f;

    /*
     * @name: airControl
     * @desc: the amount of control the player has in the air. 0 is mone, 1 is full control.
     */
    [Header("Air control parameters")]
    [SerializeField]
    [Tooltip("How much air control the player has\n0: none\n 1: same control as on ground")]
    [Range(0.0f, 1.0f)]
    private float airControl = .25f;

    /*
     * @name: AirSpeed
     * @desc: maximum speed the player can go in the air.
     */
    [SerializeField]
    [Tooltip("How fast the player can go while not grounded.")]
    private float AirSpeed;

    /*
     * @name: TPPos
     * @desc: vector3 offset of the third person mode. Will be added or subtracted from the Camera's xyz when the third person function is called.
     */
    [Header("First/TP")]
    [SerializeField]
    [Tooltip("Third-Person offset")]
    private Vector3 TPPos;

    /*
     * @name: displayDebugInfo
     * @desc: if we want to display debug information such as rays. Will eventually be attached to a larger debug manager.
     */
    [Header("Debug")]
    [SerializeField]
    [Tooltip("Wether to display debug info and renders")]
    private bool displayDebugInfo;

    /*
     * @name: groundedCheckdist
     * @desc: determines the length of the raycast operation duing the isGrounded() check.
     */
    [SerializeField]
    [Tooltip("length for raycast for jump, default value 1.5f")]
    private float groundedCheckdist = 1.5f;

    /*
     * @name: destroyOnLoad
     * @desc: if we want to destroy this gameobject when loading. Setting to true will run the DoNotDestroyOnLoad Unity function on this parent gameobject.
     */
    [Header("Load settings")]
    [SerializeField]
    [Tooltip("Wether to destroy or not destroy when loading the next level \n true: do destroy\nfalse: do not destroy")]
    private bool destroyOnLoad;


    [Header("DEPRICATED")]
    /*
     * @name: sensitivity
     * @desc: depricated way to modulate the sensitivity of the mouse input. Prefer the use of the new Input Sensitivity system
     */
    [SerializeField]
    [Tooltip("The Sensitivity of the camera\nDEPRICATED, USE NEW INPUT SENSITIVITY")]
    private float sensitivity = 1f;



    //nonserialized vars

    /*
     * @name: currentRotation
     * @desc: container variable for the current rotation of the parent gameobject. Used to contain, manipulate, and 
        apply modifiers.
     */
    private Vector2 currentRotation;

    /*
     * @name: cameraRot
     * @desc: Camera Rotation. Used as a shortcut for the camera gameobject's rotation.
     */
    private Vector2 cameraRot;

    /*
     * @name:  moveInputForce
     * @desc: Input Force container for new Input Engine. Contains the new values to be passed.
     */
    private Vector2 moveInputForce;

    /*
     * @name: cameraObject
     * @desc: container variable for camera. Used to make calculation with camera look just a bit prettier.
     */
    private GameObject cameraObject;

    /*
     * @name: FPPos
     * @desc: xyz for the first person camera position. Used as placeholder until new camera righting is finished.
     */
    private Vector3 FPPos;

    /*
     * @name: InputController
     * @desc: container for new input system manager.
     */
    private MasterInput inputController;

    /*
     * @name: curveFloat
     * @desc: used to ramp up walking instead of on-off behavior.
     */
    private float curveFloat;

    /*
     * @name: isTP
     * @desc: boolean to determine if the current state of the player character is third-person mode or not.
     */
    public bool isTP;


    [NonSerialized]
    public GameObject target = null;

    private float sMod = 1f;



    // NOT USED IN CURRENT IT
    //private AnimationCurve currentCurve;


    //Awake is used to assign variables and statuses on the gameobject.
    private void Awake()
    {
        //set mass on rigidbody to ensure physics feel right.
        transform.GetComponent<Rigidbody>().mass = 1;

        cameraObject = transform.GetChild(0).gameObject;

        FPPos = cameraObject.transform.localPosition;

        currentRotation = cameraObject.transform.rotation.eulerAngles;


        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        //initializes the input script tied to the New Input Manager.
        inputController = new MasterInput();
    }

    private void OnEnable()
    {
        inputController.Enable();

        //Binding inputs!!

        //vectors
        //Move
        inputController.Player.Move.performed += ctx /*grabs context for input*/ => moveInputForce = ctx.ReadValue<Vector2>();
        inputController.Player.Move.canceled += _ /*context is not used, so we put a _ to signify. */ => moveInputForce = Vector2.zero;

        //Look
        inputController.Player.Look.performed += ctx /*grabs context for input*/ => MoveCamera(ctx.ReadValue<Vector2>());
        
        //bools
        
        //Jump
        inputController.Player.Jump.performed += _ /*context is not used, so we put a _ signify. */ => Jump();

        //Switch Perspective
        inputController.Player.SwitchPerspective.performed += _ /*context is not used, so we put a _ to signify. */ => SwitchCameraPos();

        inputController.Player.Use.performed += _ => Use();

        inputController.Player.LockCursor.performed += _ => ToggleCursorLock();

        inputController.Player.Sprint.performed += _ => switchSprint();
    }

    private void OnDisable()
    {
        inputController.Disable();

        //Unbinding inputs!!

        //vectors
        //Move
        inputController.Player.Move.performed -= ctx /*grabs context for input*/ => movePlayer(ctx.ReadValue<Vector2>());
        inputController.Player.Move.canceled -= _ /*context is not used, so we put a _ to signifiy.. */ => moveInputForce = Vector2.zero;

        //Look
        inputController.Player.Look.performed -= ctx /*grabs context for input*/ => MoveCamera(ctx.ReadValue<Vector2>());


        //bools
        //Jump
        inputController.Player.Jump.performed -= _ /*context is not used, so we put a _ to signifiy.. */ => Jump();
        //Swtch Perspective
        inputController.Player.SwitchPerspective.performed -= _ /*context is not used, so we put a _ to signifiy.. */  => SwitchCameraPos();

        inputController.Player.Use.performed -= _ => Use();

        inputController.Player.LockCursor.performed -= _ => ToggleCursorLock();
    }

    private void FixedUpdate()
    {
        //We move the player in this fixedupdate to ensure we don't need to use DeltaTime, and also because it is a mass of physics calculations.
         movePlayer(moveInputForce);
    }

    /*
     * does WASD movement, both grounded and not.
     */
    private void movePlayer(Vector2 movementForce)
    {
        if (isGrounded())
        {
            //rampup calculation
            if (Mathf.Abs(moveInputForce.x) == 0 && Mathf.Abs(moveInputForce.y) == 0)
            {
                transform.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.MoveTowards(transform.GetComponent<Rigidbody>().velocity.x, 0,
                Time.deltaTime * 50), transform.GetComponent<Rigidbody>().velocity.y, Mathf.MoveTowards(transform.GetComponent<Rigidbody>().velocity.z, 0, Time.deltaTime * 50));
                curveFloat = 0;
                return;
            }
            curveFloat = Mathf.MoveTowards(curveFloat, playerSpeed, Time.deltaTime * 10);

            //creation and assignment of new velocity, called transformForce.
            Vector3 transformForce = transform.forward * movementForce.y + transform.right * movementForce.x;
            transformForce = new Vector3(transformForce.normalized.x * curveFloat * sMod, transformForce.y, transformForce.normalized.z * curveFloat * sMod);
            transformForce.y = transform.GetComponent<Rigidbody>().velocity.y;
            //print(sMod);
            transform.GetComponent<Rigidbody>().velocity = transformForce;


            //DEPRICATED ADDFORCE
            //transform.GetComponent<Rigidbody>().drag = 5;
            //transform.GetComponent<Rigidbody>().AddForce(transformForce * 10 * playerSpeed);
            //Mathf.Clamp(transform.GetComponent<Rigidbody>().velocity.x, -playerSpeed * movementForce.x, playerSpeed * movementForce.x);
            //Mathf.Clamp(transform.GetComponent<Rigidbody>().velocity.y, -playerSpeed * movementForce.y, playerSpeed * movementForce.y);
        }
        else
        {
            //If airborne

            transform.GetComponent<Rigidbody>().AddForce((transform.forward * movementForce.y * playerSpeed * airControl) + (transform.right * movementForce.x * playerSpeed));

            //sets and clamps new values
            float newx, newz;
            newx = Mathf.Clamp(transform.GetComponent<Rigidbody>().velocity.x, -AirSpeed, AirSpeed);
            newz = Mathf.Clamp(transform.GetComponent<Rigidbody>().velocity.z, -AirSpeed, AirSpeed);
            transform.GetComponent<Rigidbody>().AddForce(new Vector3(newx, transform.GetComponent<Rigidbody>().velocity.y, newz));

            //Airborne stopping code. If player is moving in X direction and user presses the key to move in -X, player will come to a stop, A-La Source.
            if (transform.InverseTransformPoint(transform.GetComponent<Rigidbody>().velocity).y / Mathf.Abs(transform.InverseTransformPoint(transform.GetComponent<Rigidbody>().velocity).y) 
                == movementForce.y / Mathf.Abs(movementForce.y) && movementForce.y != 0)
            {
                //calculated through worldtolocalmatrix so the local z value can be modified, instead of the global z value.
                Vector3 tempVec = transform.worldToLocalMatrix * transform.GetComponent<Rigidbody>().velocity;
                tempVec.z /= 1.1f;
                transform.GetComponent<Rigidbody>().velocity = transform.localToWorldMatrix * tempVec;
            }

            //backwards movement modifier. Makes sure if you hit -Y on controller while moving in +Y the player instantly stops moving in +Y
            //if (movementForce.y < -.1f && transform.InverseTransformPoint(transform.GetComponent<Rigidbody>().velocity).y < .01f)
            //{
            //    //calculated through worldtolocalmatrix so the local z value can be modified, instead of the global z value.
            //    Vector3 tempVec = transform.worldToLocalMatrix * transform.GetComponent<Rigidbody>().velocity;
            //    tempVec.z /= 1.1f;
            //    transform.GetComponent<Rigidbody>().velocity = transform.localToWorldMatrix * tempVec;
            //}

            //if (movementForce.y < -.1f && transform.InverseTransformPoint(transform.GetComponent<Rigidbody>().velocity).y < .01f)
            //{     
            //    //calculated through worldtolocalmatrix so the local z value can be modified, instead of the global z value.
            //    Vector3 tempVec = transform.worldToLocalMatrix * transform.GetComponent<Rigidbody>().velocity;
            //    tempVec.z /= 1.1f;
            //    transform.GetComponent<Rigidbody>().velocity = transform.localToWorldMatrix * tempVec;
            //}
        }
    }


    /*
     * two-stage check to see if the player is grounded
     */
    public bool isGrounded()
    {
        //creating and assigning locations for the raycast tests.
        float rad = transform.GetComponent<Collider>().bounds.extents.y;
        Vector3 modUp = new Vector3(rad / 2, 0, 0), modDown = new Vector3(-rad / 2, 0, 0), modLeft = new Vector3(0, 0, -rad / 2), modRight = new Vector3(0, 0, rad / 2);
        if (transform.GetComponent<Rigidbody>().velocity.y < -.1)
        {
            return false;
        }
        //below is to visualize the raws drawn for raycast test.
        if (displayDebugInfo)
        {
            UnityEngine.Debug.DrawRay(transform.localPosition, -transform.up, Color.green, groundedCheckdist);
            UnityEngine.Debug.DrawRay(transform.localPosition + modLeft, -transform.up, Color.green, groundedCheckdist);
            UnityEngine.Debug.DrawRay(transform.localPosition + modUp, -transform.up, Color.green, groundedCheckdist);
            UnityEngine.Debug.DrawRay(transform.localPosition + modDown, -transform.up, Color.green, groundedCheckdist);
            UnityEngine.Debug.DrawRay(transform.localPosition + modRight, -transform.up, Color.green, groundedCheckdist);
        }
        //checks if any of the player is close enough to the ground
        if (Physics.Raycast(transform.localPosition, -transform.up, groundedCheckdist) || Physics.Raycast(transform.localPosition + modLeft, -transform.up, groundedCheckdist) ||
            Physics.Raycast(transform.localPosition + modUp, -transform.up, groundedCheckdist) || Physics.Raycast(transform.localPosition + modDown, -transform.up, groundedCheckdist) ||
            Physics.Raycast(transform.localPosition + modRight, -transform.up, groundedCheckdist)) //Many tests ensures a result that feels good to the player.
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    /*
    * handles the camera's Y and the parent's X (therefore also handling the camera's X)
    */
    private void MoveCamera(Vector2 rotation)
    {
        //does playerobject rotation
        currentRotation.y += rotation.x * sensitivity;
        currentRotation.y = Mathf.Repeat(currentRotation.y, 360);
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);

        //manipulates the child camera to sync with the y value of the parent.
        cameraRot.x -= rotation.y * sensitivity;

        //clamps the y rotation of the camera from -maxYangle to maxYangle to prevent the camera inverting itself
        cameraRot.x = Mathf.Clamp(cameraRot.x, -maxYAngle, maxYAngle);

        cameraObject.transform.localRotation = Quaternion.Euler(cameraRot.x, 0, 0);
    }

    /*
    * adds jumpheight to player as an addforce, to simulate a jump.
    */
    private void Jump()
    {
        if (isGrounded())
        {
            transform.GetComponent<Rigidbody>().AddForce(0, jumpHeight * 20, 0);
        }
    }

    /*
    * switches the camera's localposition from FPPos to FPPos+TPPos.
    */
    private void SwitchCameraPos()
    {
        if (isTP)
        {
            print("running this");
            cameraObject.transform.localPosition = FPPos;
            isTP = !isTP;
        } else
        {
            cameraObject.transform.localPosition += TPPos;
            isTP = !isTP;
        }
    }


    private void Use()
    {
        print("okay, so this is working.");
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float d = Vector3.Dot(direction, transform.forward);

            if (d >= .75)
            {
                Debug.Log("hey, we triggered the thing!");
                //play press sound
                return;
            }
        }
        //play fail sound
        return;
    }

    // Temporary function to lock the cursor to the screen (was too annoying to keep clicking outside the screen)
    private bool lockCursor = false;
    private void ToggleCursorLock() {
        lockCursor = !lockCursor;
        if (lockCursor) {
            Debug.Log("locking...");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Debug.Log("unlocking...");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void switchSprint()
    {
        sMod = sMod == sprintModifier ? 1f : sprintModifier;
    }
}