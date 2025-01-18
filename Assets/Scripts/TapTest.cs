using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTest : MonoBehaviour
{
    public Joystick joystick;

    private void Start()
    {
        joystick.StickTapped += OnJoystickTapped; // �����¼�
    }

    private void OnJoystickTapped()
    {
        Debug.Log("Joystick tapped! ");
        // ������ʵ�ַ����߼�
    }

    private void OnDestroy()
    {
        joystick.StickTapped -= OnJoystickTapped; // ȡ�������¼�����ֹ�ڴ�й©
    }
}