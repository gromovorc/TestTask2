using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Input : MonoBehaviour
{
    [SerializeField] private Car_Controller _carController;

    public void AccelerationButtonPressed(float value)
    {
        _carController.CancelInvoke("Decelerate");
        _carController.accelerationValue = value;
        _carController.InvokeRepeating("Accelerate", 0, 0.1f);
    }

    public void SteeringButtonPressed(float value)
    {
        _carController.CancelInvoke("ResetWheelSteering");
        _carController.directionValue = value;
        _carController.InvokeRepeating("WheelSteering", 0, 0.1f);
    }

    public void BreaksButtonPressed()
    {
        _carController.InvokeRepeating("Breaks", 0, 0.1f);
    }

    public void BreaksButtonReleased()
    {
        _carController.Breaks(0);
        _carController.CancelInvoke("Breaks");
    }

    public void AccelerationButtonReleased()
    {
        _carController.CancelInvoke("Accelerate");
        _carController.InvokeRepeating("Decelerate", 0, 0.1f);
    }

    public void SteeringButtonReleased()
    {
        _carController.CancelInvoke("WheelSteering");
        _carController.InvokeRepeating("ResetWheelSteering", 0, 0.1f);
    }
}
