using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public FixedJoystick _joystick;

    [Header("Player stats")]

    [Range(0.5f, 1.0f)]
    [SerializeField] private float playerMovementSpeed;
    [Range(6.0f, 15.0f)]
    [SerializeField] private float playerRotationSpeed;

    [HideInInspector]
    public bool isWithKey;

    void Update()
    {
        PlayerMove();
    }
    private void PlayerMove()
    {
        transform.Translate(_joystick.Vertical
            * playerMovementSpeed
            * Time.fixedDeltaTime
            * Vector3.forward
            ,Space.Self);
        transform.Rotate(0.0f
            ,_joystick.Horizontal
            * playerRotationSpeed
            * Time.fixedDeltaTime
            ,0.0f);
    }
}
