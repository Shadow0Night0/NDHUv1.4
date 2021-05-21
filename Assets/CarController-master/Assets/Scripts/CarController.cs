using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTALARROWS = "HorizontalArrows";
    private const string VERTICALARROWS = "VerticalArrows";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;


    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float turnSpeed=5;
    [SerializeField] private float centerOfMassOffsetInY;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.5f;
    public float frontSensorAngle = 30f;

    private bool avoiding = false;
    private float targetSteerAngle = 0f;



    private void FixedUpdate()
    {
       
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTALARROWS);
        verticalInput = Input.GetAxis(VERTICALARROWS);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    public void SetInputs(float forwardAmount, float turnAmount)
    {
        Sensors();
        if (avoiding) return; //REMEMBER, NOT SURE "applySteer()"
        rearLeftWheelCollider.motorTorque = forwardAmount* motorForce;
        rearRightWheelCollider.motorTorque = forwardAmount* motorForce;
        if ((turnAmount > maxSteerAngle && turnAmount >= 0) || turnAmount< -maxSteerAngle)
        {
            if (turnAmount >= 0 && turnAmount > 15)
            {
                turnAmount = maxSteerAngle;
            }
            else turnAmount = -maxSteerAngle;
        }
        frontLeftWheelCollider.steerAngle = turnAmount;
        frontRightWheelCollider.steerAngle = turnAmount;
        UpdateWheels();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        float avoidMultiplier = 0;
        avoiding = false;


        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Car"))
            {
                ApplyBreaking();
                Debug.DrawLine(sensorStartPos, hit.point);
                avoidMultiplier -= 1f;
            }
        }

        //frontright sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road"))
            {
                avoiding = true;
                Debug.DrawLine(sensorStartPos, hit.point);
                avoidMultiplier -= 1f;
            }
        }

        //front right angle sensor  
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        //front left sensor
        sensorStartPos -= transform.right * 2 * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road"))
            {
                avoiding = true;
                Debug.DrawLine(sensorStartPos, hit.point);
                avoidMultiplier += 1f;
            }
        }

        // angled front left sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        


        //frontcenter sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Road"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }

        if (avoiding)
        {
            frontLeftWheelCollider.steerAngle = maxSteerAngle * avoidMultiplier;
            frontRightWheelCollider.steerAngle = maxSteerAngle * avoidMultiplier;
            ApplyBreaking();
            UpdateWheels();
        }


    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot
;       wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public float GetSpeed()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
        
    }

    private void LerpToSteerAngle()
    {
        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        frontRightWheelCollider.steerAngle = Mathf.Lerp(frontRightWheelCollider.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);

    }
}
