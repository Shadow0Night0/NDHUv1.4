using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAITargetMouse : MonoBehaviour {

    [SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject FollowThis;
 
    private bool isFollowing = false;

    private void Update() {
        if (isFollowing) {
            //targetTransform.position = Mouse3D.GetMouseWorldPosition();
            targetTransform.position = FollowThis.transform.position;

        }

        if (Input.GetMouseButtonDown(0)) {
            isFollowing = !isFollowing;
        }
    }

}
