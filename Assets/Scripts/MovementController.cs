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

    public float jumpForce = 5f;
    private bool isGrounded = true; 

    public float tiltAmount = 4f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (joystick != null)
        {
            joystick.StickTapped += OnJoystickTapped;
        }
    }

    private void Update()
    {
        // Calculate movement velocity
        Vector3 movementVelocityVector = CalculateMovementVelocity();

        // Move the player
        Move(movementVelocityVector);

        // tilt the player, based on the joystick input aka movement direction.
        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, joystick.Horizontal * speed * tiltAmount * -1);
    }
    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            ApplyVelocityChange(velocityVector);
        }

       
    }
    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private Vector3 CalculateMovementVelocity()
    {
        // Get the movement input from the joystick
        float xMovement = Mathf.Abs(joystick.Horizontal) > 0.1f ? joystick.Horizontal : 0f;
        float yMovement = Mathf.Abs(joystick.Vertical) > 0.1f ? joystick.Vertical : 0f;

        // Calculate the movement vector
        Vector3 movementHorizontal = transform.right * xMovement;
        Vector3 movementVertical = transform.forward * yMovement;

        // Return the movement velocity vector (direction * speed)
        return (movementHorizontal + movementVertical).normalized * speed;
    }

    

    private void ApplyVelocityChange(Vector3 targetVelocity)
    {
        // get the current velocity
        Vector3 velocity = rb.velocity;

        // calculate the velocity change
        Vector3 velocityChange = (targetVelocity - velocity);

        // limit the velocity change to maxVelocityChange
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0; // 不改变 Y 轴速度（防止物体飞起）

        // apply the velocity change
        rb.AddForce(velocityChange, ForceMode.Acceleration);
    }

    private void OnJoystickTapped()
    {
        // 检查是否在地面上
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 标记为非地面状态
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测是否回到地面
        if (collision.contacts.Length > 0 && collision.contacts[0].point.y <= transform.position.y)
        {
            isGrounded = true;
        }
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        if (joystick != null)
        {
            joystick.StickTapped -= OnJoystickTapped;
        }
    }
}
