using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 4f;

    private Vector3 velocityVector = Vector3.zero;
    private Rigidbody rb;

    public float maxVelocityChange = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Calculate movement velocity
        Vector3 movementVelocityVector = CalculateMovementVelocity();

        // Move the player
        Move(movementVelocityVector);
    }

    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;

    }
    private Vector3 CalculateMovementVelocity()
    {
        // Get the movement input from the joystick
        // When the input is less than 0.1f, set it to 0 to avoid miss touch
        float xMovement = Mathf.Abs(joystick.Horizontal) > 0.1f ? joystick.Horizontal : 0f;
        float yMovement = Mathf.Abs(joystick.Vertical) > 0.1f ? joystick.Vertical : 0f;

        // Calculate the movement vector
        Vector3 movementHorizontal = transform.right * xMovement;
        Vector3 movementVertical = transform.forward * yMovement;

        // Return the movement velocity vector (direction * speed)
        return (movementHorizontal + movementVertical).normalized * speed;
    }



    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            //Get rigidbody's current velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);
            //add a force by the amount of velocity change to reach the target velocity
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.Acceleration);


        }
    }
}
