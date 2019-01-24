using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public GameObject rotateAround;
    private Vector3 offset;
    void Start () {
	    offset = transform.position - rotateAround.transform.position;
    }

    void Update () {
        float rotX = Input.GetAxis("Mouse X") * 6;
        float rotationX = Mathf.Clamp(rotX, -2, 2);

        float rotY = Input.GetAxis("Mouse Y") * 6;
        float rotationY = -Mathf.Clamp(rotY, -2, 2);

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 2, Vector3.up) * offset;
        transform.position = rotateAround.transform.position + offset;
        transform.LookAt(rotateAround.transform.position);
    }
}
