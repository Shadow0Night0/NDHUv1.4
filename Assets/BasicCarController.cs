using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCarController : MonoBehaviour
{
    [SerializeField]
    private Transform carRoot;
    private CharacterController carController;
    private const string HORIZONTALARROWS = "HorizontalArrows";
    private const string VERTICALARROWS = "VerticalArrows";

    private float horizontalInput;
    private float verticalInput;
    private float frontnback;
    private float leftnright;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    void Awake()
    {
        carController = GetComponent<CharacterController>();
    }
  void Update()
    {
        GetInput();
        HandleMotor();
    }
   /* private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        //HandleSteering();
    }*/
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTALARROWS) * Time.deltaTime;
        verticalInput = Input.GetAxis(VERTICALARROWS) * Time.deltaTime;
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontnback = horizontalInput;
        leftnright += verticalInput;
        carRoot.localRotation = Quaternion.Euler(0f, leftnright, 0f);
        carController.Move(new Vector3(0, 0, frontnback));

    }

}


