using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTest : MonoBehaviour
{
    public Joystick joystick;

    private void Start()
    {
        joystick.StickTapped += OnJoystickTapped; // subscribe to the event
    }

    private void OnJoystickTapped()
    {
        Debug.Log("Joystick tapped! ");
        // tap logic here
    }

    private void OnDestroy()
    {
        joystick.StickTapped -= OnJoystickTapped; // unsbscribe from the event
    }
}