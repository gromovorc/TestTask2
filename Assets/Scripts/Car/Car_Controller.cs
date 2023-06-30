using System;
using TMPro;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    [Header("Car stats")]

    [Range(100, 200)]
    [SerializeField] private int maxSpeed;
    [Range(250, 600)]
    [SerializeField] private int horsePowers;
    [Range(0.175f, 0.350f)]
    [SerializeField] private float accelerationMultiplier = 0.225f;
    [Range(10, 120)]
    [SerializeField] private int maxReverseSpeed = 45;
    [Space(10)]
    [Range(300.0f, 600.0f)]
    [SerializeField] private float brakeForce = 400.0f;
    [Range(1.01f, 1.5f)]
    [SerializeField] private float decelerationMultiplier = 1.01f;
    [Space(10)]
    [Range(45.0f, 60.0f)]
    [SerializeField] private float maxWheelAngle;
    [Range(1.0f, 5.0f)]
    [SerializeField] private float steeringSpeed = 0.1f;
    [Space(10)]
    [SerializeField] private Vector3 centerOfMass;

    [Space(20)]
    [Header("Wheels")]

    [SerializeField] private Transform _frontLeftWheel;
    [SerializeField] private WheelCollider _frontLeftWheelCollider;
    [SerializeField] private Transform _frontRightWheel;
    [SerializeField] private WheelCollider _frontRightWheelCollider;
    [SerializeField] private Transform _backLeftWheel;
    [SerializeField] private WheelCollider _backLeftWheelCollider;
    [SerializeField] private Transform _backRightWheel;
    [SerializeField] private WheelCollider _backRightWheelCollider;

    [Space(20)]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TMP_Text _speedometer;

    [HideInInspector]
    public float currentSpeed, directionValue, accelerationValue;

    private float wheelAngle, throttleValue, initialPitch;

    private Rigidbody _carRigidbody;

    private void Start()
    {
        _carRigidbody = GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = centerOfMass;
        initialPitch = _audioSource.pitch;
    }

    private void FixedUpdate()
    {
        WheelAnimation();
        CarSound();
        ShowSpeed();
    }

    public void Accelerate()
    {
        throttleValue += accelerationMultiplier < 0 ? 0 : throttleValue += accelerationMultiplier;
        SetMotorTorque(accelerationValue * horsePowers * Mathf.Clamp01(throttleValue));

        switch (accelerationValue)
        {
            case 1:
                if (currentSpeed > maxSpeed) SetMotorTorque();
                break;
            case -1:
                if (currentSpeed > maxReverseSpeed)
                {
                    SetMotorTorque();
                    throttleValue -= accelerationMultiplier;
                }
                break;
            default:
                SetMotorTorque(accelerationValue * horsePowers * Mathf.Clamp01(throttleValue));
                break;
        }
    }

    public void Decelerate()
    {
        if (throttleValue != 0f) throttleValue -= decelerationMultiplier;
        _carRigidbody.velocity /= decelerationMultiplier;

        SetMotorTorque();

        if (_carRigidbody.velocity.magnitude < 0.5f)
        {
            _carRigidbody.velocity = Vector3.zero;
            throttleValue = 0f;
            CancelInvoke("Decelerate");
        }
    }

    public void Breaks()
    {
        _frontLeftWheelCollider.brakeTorque = brakeForce;
        _frontRightWheelCollider.brakeTorque = brakeForce;
        _backLeftWheelCollider.brakeTorque = brakeForce;
        _backRightWheelCollider.brakeTorque = brakeForce;
    }

    public void Breaks(float value)
    {
        _frontLeftWheelCollider.brakeTorque = value;
        _frontRightWheelCollider.brakeTorque = value;
        _backLeftWheelCollider.brakeTorque = value;
        _backRightWheelCollider.brakeTorque = value;
    }

    private void SetMotorTorque(float value = 0)
    {
        _frontLeftWheelCollider.motorTorque = value;
        _frontRightWheelCollider.motorTorque = value;
        _backLeftWheelCollider.motorTorque = value;
        _backRightWheelCollider.motorTorque = value;
    }

    public void WheelSteering()
    {
        wheelAngle += directionValue * steeringSpeed * Time.fixedDeltaTime * maxWheelAngle;

        SetSteerAngle(steeringSpeed, Mathf.Clamp(wheelAngle, -maxWheelAngle, maxWheelAngle));
    }

    public void ResetWheelSteering()
    { 
        wheelAngle += -directionValue * steeringSpeed * Time.fixedDeltaTime * maxWheelAngle;

        wheelAngle = Mathf.Clamp(wheelAngle, -maxWheelAngle, maxWheelAngle);

        if (Mathf.Abs(wheelAngle) < 0.01) CancelInvoke("ResetWheelSteering");
        SetSteerAngle(steeringSpeed, Mathf.Clamp(wheelAngle, -maxWheelAngle, maxWheelAngle));
    }

    private void SetSteerAngle(float steeringSpeed, float neededWheelAngle)
    {
        _frontLeftWheelCollider.steerAngle = Mathf.Lerp(
            _frontLeftWheelCollider.steerAngle
            , neededWheelAngle
            , steeringSpeed);
        _frontRightWheelCollider.steerAngle = Mathf.Lerp(
            _frontLeftWheelCollider.steerAngle
            , neededWheelAngle
            , steeringSpeed);
    }

    private void WheelAnimation()
    {
        WheelRotation(_frontLeftWheelCollider, _frontLeftWheel);
        WheelRotation(_frontRightWheelCollider, _frontRightWheel);
        WheelRotation(_backLeftWheelCollider, _backLeftWheel);
        WheelRotation(_backRightWheelCollider, _backRightWheel);
    }

    private void WheelRotation(WheelCollider _wheelCollider, Transform _wheelTransform)
    {
        _wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        _wheelTransform.SetPositionAndRotation(position, rotation);
    }

    private void CarSound()
    {
        float engineSoundPitch = initialPitch + (Mathf.Abs(_carRigidbody.velocity.magnitude) / 30.0f);
        _audioSource.pitch = engineSoundPitch;
    }
    private void ShowSpeed()
    {
        currentSpeed = Mathf.Floor(_carRigidbody.velocity.magnitude * 3.6f);

        _speedometer.text = $"{currentSpeed} km/h";
    }
}
