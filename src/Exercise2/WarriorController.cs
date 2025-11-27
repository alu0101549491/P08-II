using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float accelerationMultiplier = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rotationSmoothTime = 0.3f;

    [Header("GPS Boundary Settings")]
    // Latitud y Longitud de La Laguna
    [SerializeField] private float minLatitude = 28.4f;
    [SerializeField] private float maxLatitude = 28.5f;
    [SerializeField] private float minLongitude = -16.4f;
    [SerializeField] private float maxLongitude = -16.3f;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    // Sensores
    private Accelerometer accelerometer;
    private MagneticFieldSensor compass;
    
    // Variables de movimiento
    private Vector3 currentVelocity = Vector3.zero;
    private Quaternion targetRotation;
    private bool isMovementEnabled = true;

    // GPS
    private bool gpsInitialized = false;
    private float currentLatitude;
    private float currentLongitude;

    void Start()
    {
        InitializeSensors();
        InitializeGPS();
        targetRotation = transform.rotation;
    }

    void InitializeSensors()
    {
        // Habilitar acelerómetro
        accelerometer = Accelerometer.current;
        if (accelerometer != null)
        {
            InputSystem.EnableDevice(accelerometer);
            Debug.Log("Accelerometer enabled");
        }
        else
        {
            Debug.LogWarning("Accelerometer not available");
        }

        // Habilitar sensor magnético (brújula)
        compass = MagneticFieldSensor.current;
        if (compass != null)
        {
            InputSystem.EnableDevice(compass);
            Debug.Log("Magnetic Field Sensor enabled");
        }
        else
        {
            Debug.LogWarning("Magnetic Field Sensor not available");
        }
    }

    void InitializeGPS()
    {
        // Verificar si el servicio de localización está habilitado
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("GPS not enabled by user");
            return;
        }

        // Iniciar servicio de localización
        Input.location.Start(1f, 1f); // Precisión de 1 metro, distancia mínima 1 metro
        Debug.Log("GPS service started");
    }

    void Update()
    {
        UpdateGPSLocation();
        CheckBoundaries();

        if (isMovementEnabled)
        {
            UpdateRotationFromCompass();
            UpdateMovementFromAccelerometer();
        }

        if (showDebugInfo)
        {
            DisplayDebugInfo();
        }
    }

    void UpdateGPSLocation()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            if (!gpsInitialized)
            {
                gpsInitialized = true;
                Debug.Log("GPS initialized successfully");
            }

            currentLatitude = Input.location.lastData.latitude;
            currentLongitude = Input.location.lastData.longitude;
        }
        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS failed to initialize");
        }
    }

    void CheckBoundaries()
    {
        if (!gpsInitialized) return;

        // Verificar si estamos dentro de los límites
        bool inBounds = currentLatitude >= minLatitude &&
                        currentLatitude <= maxLatitude &&
                        currentLongitude >= minLongitude &&
                        currentLongitude <= maxLongitude;

        if (!inBounds && isMovementEnabled)
        {
            StopMovement();
            Debug.Log("Out of bounds! Movement stopped.");
        }
        else if (inBounds && !isMovementEnabled)
        {
            isMovementEnabled = true;
            Debug.Log("Back in bounds! Movement enabled.");
        }
    }

    void UpdateRotationFromCompass()
    {
        if (compass == null || !compass.enabled) return;

        // Obtener el campo magnético
        Vector3 magneticField = compass.magneticField.ReadValue();

        // Calcular el ángulo hacia el norte magnético (Unity tiene el norte magnético en +Z)
        float heading = Mathf.Atan2(magneticField.x, magneticField.y) * Mathf.Rad2Deg;

        // Crear rotación objetivo mirando al norte
        targetRotation = Quaternion.Euler(0, -heading, 0);

        // Interpolar suavemente hacia la rotación objetivo usando Slerp
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            Time.deltaTime / rotationSmoothTime
        );
    }

    void UpdateMovementFromAccelerometer()
    {
        if (accelerometer == null || !accelerometer.enabled) return;

        Vector3 acceleration = accelerometer.acceleration.ReadValue();

        // Invertir Z (adelante/atrás) y usarlo solo cuando el dispositivo está en horizontal
        float forwardAcceleration = -acceleration.z;

        Vector3 targetVelocity = transform.forward * forwardAcceleration * accelerationMultiplier;

        // Velocidad límite
        if (targetVelocity.magnitude > maxSpeed)
        {
            targetVelocity = targetVelocity.normalized * maxSpeed;
        }

        // Suavizar la velocidad
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * 5f);

        transform.position += currentVelocity * Time.deltaTime;
    }

    void StopMovement()
    {
        isMovementEnabled = false;
        currentVelocity = Vector3.zero;
    }

    void DisplayDebugInfo()
    {
        if (accelerometer != null && accelerometer.enabled)
        {
            Vector3 acc = accelerometer.acceleration.ReadValue();
            Debug.Log($"Acceleration: {acc}");
        }

        if (compass != null && compass.enabled)
        {
            Vector3 mag = compass.magneticField.ReadValue();
            Debug.Log($"Magnetic Field: {mag}");
        }

        if (gpsInitialized)
        {
            Debug.Log($"GPS: Lat={currentLatitude}, Lon={currentLongitude}, InBounds={isMovementEnabled}");
        }
    }

    void OnDestroy()
    {
        // Deshabilitar sensores
        if (accelerometer != null && accelerometer.enabled)
        {
            InputSystem.DisableDevice(accelerometer);
        }

        if (compass != null && compass.enabled)
        {
            InputSystem.DisableDevice(compass);
        }

        // Detener GPS
        Input.location.Stop();
    }

    void OnApplicationQuit()
    {
        OnDestroy();
    }
}