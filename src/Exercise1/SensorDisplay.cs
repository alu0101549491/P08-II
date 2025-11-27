using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorDisplay : MonoBehaviour
{
    public TextMeshProUGUI output;

    void Update()
    {
        var accelerometerValue = Accelerometer.current?.acceleration?.ReadValue();
        var gyroscopeValue = UnityEngine.InputSystem.Gyroscope.current?.angularVelocity?.ReadValue();
        var gravityValue = GravitySensor.current?.gravity?.ReadValue();
        var attitudeValue = AttitudeSensor.current?.attitude?.ReadValue();
        var linearAccelerationValue = LinearAccelerationSensor.current?.acceleration?.ReadValue();
        var magneticFieldValue = MagneticFieldSensor.current?.magneticField?.ReadValue();

        // Sensores tipo AxisControl / IntegerControl
        var lightValue = LightSensor.current?.lightLevel.ReadValue();
        var pressureValue = PressureSensor.current?.atmosphericPressure.ReadValue();
        var proximityValue = ProximitySensor.current?.distance.ReadValue();
        var humidityValue = HumiditySensor.current?.relativeHumidity.ReadValue();
        var ambientTempValue = AmbientTemperatureSensor.current?.ambientTemperature.ReadValue();
        var stepCounterValue = StepCounter.current?.stepCounter.ReadValue();

        output.text =
            $"<b>Accelerometer:</b> {ReadVector3(accelerometerValue)}\n" +
            $"<b>Gyroscope:</b> {ReadVector3(gyroscopeValue)}\n" +
            $"<b>Gravity Sensor:</b> {ReadVector3(gravityValue)}\n" +
            $"<b>Attitude:</b> {ReadQuaternion(attitudeValue)}\n" +
            $"<b>Linear Acceleration:</b> {ReadVector3(linearAccelerationValue)}\n" +
            $"<b>Magnetic Field:</b> {ReadVector3(magneticFieldValue)}\n" +
            $"<b>Light:</b> {ReadFloat(lightValue)} lux\n" +
            $"<b>Pressure:</b> {ReadFloat(pressureValue)} hPa\n" +
            $"<b>Proximity:</b> {ReadFloat(proximityValue)} cm\n" +
            $"<b>Humidity:</b> {ReadFloat(humidityValue)} %\n" +
            $"<b>Ambient Temp:</b> {ReadFloat(ambientTempValue)} ºC\n" +
            $"<b>Step Counter:</b> {ReadInt(stepCounterValue)}\n";
    }

    string ReadVector3(Vector3? v)
    {
        return v.HasValue ? v.Value.ToString("F2") : "N/A";
    }

    string ReadQuaternion(Quaternion? q)
    {
        return q.HasValue ? q.Value.ToString("F2") : "N/A";
    }

    string ReadFloat(float? f)
    {
        return f.HasValue ? f.Value.ToString("F2") : "N/A";
    }

    string ReadInt(int? i)
    {
        return i.HasValue ? i.Value.ToString() : "N/A";
    }
}
