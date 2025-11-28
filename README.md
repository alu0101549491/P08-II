# P08 - II

## Ejercicio 1

### Medida del Laboratorio

### Medida del Patio de la ESIT

### Explicación

Para este ejercicio, se desarrollaron 2 scripts: `SensorManager.cs` y `SensorDisplay.cs`. 

El primero simplemente accede a todos los `devices` del `InputSystem` que se reconocen como sensores y los activa mediante `InputSystem.EnableDevice(sensor)`.

El segundo script accede explícitamente a cada uno de los valores de cada sensor elegido:

* `accelerometer`
* `gyroscope`
* `gravity`
* `attitude`
* `linearAcceleration`
* `magneticField`
* `light`
* `pressure`
* `proximity`
* `humidity`
* `ambientTemperature`
* `stepCounter`

Para posteriormente, modificar el texto de un objeto `TextMeshProUGUI` del canvas añadiendo los valores de cada uno de los sensores.

## Ejercicio 2

