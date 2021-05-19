using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI : MonoBehaviour {

    [SerializeField] private Transform targetPositionTranform;
    [SerializeField] private bool isClockwise;
    public Transform path;

    private List<Transform> nodes;
    private CarController carDriver;
    private Vector3 targetPosition;
    private int currentnode=0;

    private void Awake() {
        carDriver = GetComponent<CarController>();
    }

    private void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        if (isClockwise)
        {
            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != path.transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }
        }
        else
        {
            for (int i = pathTransforms.Length-1; i >=0; i--)
            {
                if (pathTransforms[i] != path.transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }
        }
        SetTargetPosition(nodes[currentnode].position);
    }

    private void Update() {

        Debug.Log(currentnode);
        float forwardAmount = 0f;
        float turnAmount = 0f;
        float angleToDir=0f;
        float reachedTargetDistance = 5f;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > reachedTargetDistance) {
            // Still too far, keep going
            Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePosition);
            if (dot > 0) {
                // Target in front
                forwardAmount = 1f;

                float stoppingDistance = 20f;
                float stoppingSpeed = 40f;
                if (distanceToTarget < stoppingDistance && carDriver.GetSpeed() > stoppingSpeed) {
                    // Within stopping distance and moving forward too fasts
                    forwardAmount = -1f;
                }
            } else {
                // Target behind
                float reverseDistance = 2f;
                if (distanceToTarget > reverseDistance) {
                    // Too far to reverse
                    forwardAmount = 1f;
                } else {
                    forwardAmount = -1f;
                }
            }

            angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

            if (angleToDir > 0) {
                turnAmount = 1f;
            } else {
                turnAmount = -1f;
            }
        } else {
            // Reached target
            currentnode++;
            if (currentnode >= nodes.Count)
            {
                currentnode = 0;
            }
            SetTargetPosition(nodes[currentnode].position);
            if (carDriver.GetSpeed() > 15f) {
                forwardAmount = -1f;
            } else {
                forwardAmount = 0f;
            }
            turnAmount = 0f;
            
            
        }

        carDriver.SetInputs(forwardAmount, angleToDir); //inputs to drive car, probably
    }

    public void SetTargetPosition(Vector3 targetPositionA) {
        this.targetPosition = targetPositionA;
    }

}
