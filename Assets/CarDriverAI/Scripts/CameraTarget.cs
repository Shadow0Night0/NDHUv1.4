using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    private void Update() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 100f;
        transform.position += new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
    }

}
