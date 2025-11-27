using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIDebugInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    
    private Accelerometer accelerometer;
    private MagneticFieldSensor compass;

    void Start()
    {
        accelerometer = Accelerometer.current;
        compass = MagneticFieldSensor.current;

        if (debugText == null)
        {
            debugText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (debugText == null) return;

        string info = "=== SENSOR INFO ===\n\n";

        // Acelerómetro
        if (accelerometer != null && accelerometer.enabled)
        {
            Vector3 acc = accelerometer.acceleration.ReadValue();
            info += $"ACCELEROMETER:\n";
            info += $"X: {acc.x:F3}\n";
            info += $"Y: {acc.y:F3}\n";
            info += $"Z: {acc.z:F3}\n\n";
        }
        else
        {
            info += "ACCELEROMETER: Not available\n\n";
        }

        // Brújula (sensor magnético)
        if (compass != null && compass.enabled)
        {
            Vector3 mag = compass.magneticField.ReadValue();
            float heading = Mathf.Atan2(mag.x, mag.y) * Mathf.Rad2Deg;
            
            info += $"COMPASS:\n";
            info += $"Heading: {heading:F1}°\n";
            info += $"X: {mag.x:F2} µT\n";
            info += $"Y: {mag.y:F2} µT\n";
            info += $"Z: {mag.z:F2} µT\n\n";
        }
        else
        {
            info += "COMPASS: Not available\n\n";
        }

        // GPS
        if (Input.location.status == LocationServiceStatus.Running)
        {
            info += $"GPS:\n";
            info += $"Lat: {Input.location.lastData.latitude:F6}\n";
            info += $"Lon: {Input.location.lastData.longitude:F6}\n";
            info += $"Alt: {Input.location.lastData.altitude:F2}m\n";
            info += $"Accuracy: {Input.location.lastData.horizontalAccuracy:F2}m\n";
        }
        else
        {
            info += $"GPS: {Input.location.status}\n";
        }

        debugText.text = info;
    }
}