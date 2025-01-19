using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpped = 3600f;
    public bool doSpin = false;

    private Rigidbody rb;

    [SerializeField] GameObject playerGraphics;

    private void FixedUpdate()
    {
        if (doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinSpped * Time.deltaTime, 0));
        }
    }
}
