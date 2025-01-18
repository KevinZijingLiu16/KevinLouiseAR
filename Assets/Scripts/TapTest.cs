using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTest : MonoBehaviour
{
    public Joystick joystick;

    private void Start()
    {
        joystick.StickTapped += OnJoystickTapped; // 订阅事件
    }

    private void OnJoystickTapped()
    {
        Debug.Log("Joystick tapped! ");
        // 在这里实现发射逻辑
    }

    private void OnDestroy()
    {
        joystick.StickTapped -= OnJoystickTapped; // 取消订阅事件，防止内存泄漏
    }
}