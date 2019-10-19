using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{     
    public GameObject player;
    public float speed = 3.5f;
    private float X;
    private float Y;
 
    void Update() {
        if (Input.GetMouseButton(0)) {
            transform.LookAt(player.transform.position);
            transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X")*speed);
        }
    }
}
