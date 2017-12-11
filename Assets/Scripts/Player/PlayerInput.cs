using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode SoarKey;
    public KeyCode JumpKey;

    public Vector2 DirectionalInput { get { return new Vector2(Input.GetAxisRaw("Horizontal"), 0); } }

    public bool CaptureSoarInput()
    {
        return Input.GetKeyDown(SoarKey);
    }

    public bool CaptureJumpInputDown()
    {
        return Input.GetKeyDown(JumpKey);
    }

    public bool CaptureJumpInputUp()
    {
        return Input.GetKeyUp(JumpKey);
    }
}
