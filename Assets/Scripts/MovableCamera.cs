using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private bool isMoving = false;
    [SerializeField] Vector3 mouseDownPos;
    [SerializeField] Vector3 isNotMoveDistance;
    [SerializeField] int MovableDistanceSensitivityX;
    [SerializeField] int MovableDistanceSensitivityY;

    void Update()
    {
        // Check for mouse or touch input
        CheckForInput();

        // Move the camera if input is detected
        if (isMoving)
        {
            MoveCamera();
        }
    }

    void CheckForInput()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            isMoving = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }
    }

    void MoveCamera()
    {
        Vector3 moveDistance = (mouseDownPos - Input.mousePosition);
        Vector3 moveAmount = moveDistance * moveSpeed;
        Debug.Log(isNotMoveDistance+"");
        isNotMoveDistance = (new Vector3(Mathf.Abs(moveDistance.x), Mathf.Abs(moveDistance.y),0)) - (new Vector3(4,4,0));
        mouseDownPos = Input.mousePosition;
        if(isNotMoveDistance.x > MovableDistanceSensitivityX || isNotMoveDistance.y > MovableDistanceSensitivityY) transform.Translate(moveAmount, Space.Self);
    }
}
