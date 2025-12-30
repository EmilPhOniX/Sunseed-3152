using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveActionRef;
    public float moveSpeed;
    public float rotationSpeed;

    private void Update()
    {
        moveDrone();
    }

    private void moveDrone()
    {
        Vector2 stickDirection = moveActionRef.action.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * stickDirection.y * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;
        transform.Rotate(Vector3.up, stickDirection.x * rotationSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9.5f, 13.5f),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -8f, 6f)
        );
    }
}