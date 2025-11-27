using UnityEngine;
using UnityEngine.InputSystem;

public class SensorManager : MonoBehaviour
{
    void OnEnable()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is Sensor sensor && !sensor.enabled)
            {
                Debug.Log($"Enabling sensor: {sensor.displayName} ({sensor.layout})");
                InputSystem.EnableDevice(sensor);
            }
        }
    }
}
