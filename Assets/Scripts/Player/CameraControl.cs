using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float xMouseSensitivity = 10f;
    public float yMouseSensitivity = 10f;
    public Transform cam;
    public Transform orientation;

    Vector2 mousePos;
    Vector2 rotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //get the mouse x and y values
        mousePos.x = Input.GetAxisRaw("Mouse X");
        mousePos.y = Input.GetAxisRaw("Mouse Y");
        //set the rotation values based on the sensitivity
        rotation.y += mousePos.x * xMouseSensitivity;
        rotation.x -= mousePos.y * yMouseSensitivity;
        //clamp the x rotation so that the camera can't rotate past a certtain point on that axis
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        //transform the camera's rotation based on the values
        cam.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        //transform the orientation value so it can be used as the forward in the player movement script
        orientation.transform.localRotation = Quaternion.Euler(0, rotation.y, 0);

    }
}