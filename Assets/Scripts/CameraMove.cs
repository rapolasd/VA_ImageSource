using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotationSpeed = 10f;   // speed of rotation (can be edited from the inspector)
    public float transaltionSpeed = 1f; // speed of translation (can be edited from the inspector)
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // When CTRL (or Cmd) is pressed then the object can be rotated using arrow keys
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand))
        {
            if (Input.GetKey(KeyCode.LeftArrow)) // rotating clockwise along the horizontal world plane
            {
                transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
            }

            if (Input.GetKey(KeyCode.RightArrow)) // rotating anticlockwise along the horizontal world plane
            {
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
            }

            if (Input.GetKey(KeyCode.UpArrow)) // rotating upwards
            {
                transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow)) // rotating downwards
            {
                transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
            }
        }
        // when Shift is pressed the camera can be moved forwards or backwards using up/down arrow keys
        else if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) 
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * transaltionSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.back * Time.deltaTime * transaltionSpeed);
            }
        }
        // Camera moves up/down/left/right using arrows keys only
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * Time.deltaTime * transaltionSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * Time.deltaTime * transaltionSpeed);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * transaltionSpeed);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * Time.deltaTime * transaltionSpeed);
            }
        }
    }
}
