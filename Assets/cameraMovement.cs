using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 0.001f;
    float horizontalInput;
    float verticalInput;
    Vector3 mouseStartPosition;
    float zoomInOut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CalculateRotation();
        Move();
        Zoom();
    }
    void CheckInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        zoomInOut = Input.mouseScrollDelta.y;
        if (Input.GetMouseButtonDown(2)) mouseStartPosition = Input.mousePosition;
    }
    void CalculateRotation()
    {
        if (Input.GetMouseButton(2))
        {
            SetCursor.Hold();
            Vector3 rotation = (Input.mousePosition - mouseStartPosition)* Time.deltaTime * rotationSpeed * 25/Camera.main.fieldOfView;
            transform.Rotate(rotation.y, -rotation.x,0);
            mouseStartPosition = Input.mousePosition;
        }
        else
            SetCursor.Default();
    }
    void Move()
    {
        transform.Translate(new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0));
    }
    void Zoom()
    {
        Camera.main.fieldOfView=Mathf.Clamp(Camera.main.fieldOfView - zoomInOut, 3, 25);
    }
}
