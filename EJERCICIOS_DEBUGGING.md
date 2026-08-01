# Ejercicios de búsqueda y corrección de errores

El backend contiene errores intencionales de comportamiento. No todos producen una excepción: algunos devuelven una respuesta aparentemente correcta, pero guardan datos incorrectos o permiten una operación que la clínica debería rechazar.

El objetivo no es adivinar una línea de código. En cada ejercicio debes:

1. Preparar los datos indicados.
2. Reproducir el problema desde Swagger.
3. Anotar la petición, el código HTTP y la respuesta obtenida.
4. Consultar nuevamente el recurso para comprobar qué se guardó realmente.
5. Seguir el flujo `Controller → Service → Repository/Domain` con el depurador.
6. Explicar la causa antes de modificar el código.
7. Corregir solamente lo necesario.
8. Repetir el caso que fallaba y los casos de control indicados.

## Preparación general

Antes de comenzar:

1. Ejecuta los scripts de tablas y datos de demostración indicados en `README.md`.
2. Inicia la API y abre Swagger.
3. Usa los endpoints `GET` para localizar IDs existentes. No asumas que un registro tendrá un ID específico, porque los valores `IDENTITY` pueden cambiar entre bases de datos.
4. Para los ejercicios de citas, elige fechas futuras. En los ejemplos se usa el **15 de diciembre de 2026**; si esa fecha ya pasó cuando hagas la práctica, cámbiala por cualquier día futuro conservando las mismas horas.
5. Antes de reutilizar un horario, consulta `GET /api/appointments` y asegúrate de que tus pruebas anteriores no hayan dejado una cita que interfiera con el ejercicio actual.

Datos que conviene anotar antes de empezar:

- ID de un paciente activo.
- ID de dos doctores activos.
- ID de dos consultorios activos.
- ID de dos especialidades activas.
- IDs de los estados `Scheduled`, `Confirmed`, `Completed` y `Cancelled`, consultando `GET /api/appointment-statuses`.

Los ejemplos usan marcadores como `<doctorAId>`. Debes sustituirlos por los IDs encontrados en tu base de datos.

---

## Nivel 1 — Fácil

### Ejercicio 1: estado de un consultorio

#### Funcionalidad que estás probando

Este ejercicio **no se realiza al crear el consultorio ni al actualizar su nombre o ubicación**. Debes probar específicamente el endpoint:

```http
PATCH /api/consultingrooms/{id}/status
```

Este endpoint recibe el estado deseado:

```json
{
  "isActive": false
}
```

El significado es directo: `false` solicita dejar el consultorio inactivo y `true` solicita dejarlo activo. No es una orden para invertir el estado actual.

#### Preparación

1. Ejecuta `GET /api/consultingrooms`.
2. Selecciona un consultorio que tenga `isActive: true`.
3. Anota su `id`, nombre y estado inicial.

Si no tienes uno activo, crea un consultorio con `POST /api/consultingrooms`:

```json
{
  "name": "Consultorio Debug 1",
  "location": "Primer piso"
}
```

Un consultorio nuevo debe comenzar activo.

#### Caso A: desactivar

Envía:

```http
PATCH /api/consultingrooms/<consultingRoomId>/status
```

```json
{
  "isActive": false
}
```

Resultado esperado:

- La respuesta debe ser exitosa.
- El JSON devuelto debe contener `isActive: false`.
- Un `GET /api/consultingrooms/<consultingRoomId>` posterior también debe devolver `isActive: false`.
- El valor almacenado en la tabla `ConsultingRooms` debe ser `0`.

#### Caso B: activar

Sobre el mismo consultorio, envía:

```json
{
  "isActive": true
}
```

Resultado esperado:

- La respuesta y el `GET` posterior deben contener `isActive: true`.
- El valor almacenado en SQL Server debe ser `1`.

#### Casos de control: llamadas repetidas

1. Envía `isActive: true` dos veces seguidas. El consultorio debe continuar activo en ambas llamadas.
2. Envía `isActive: false` dos veces seguidas. El consultorio debe continuar inactivo en ambas llamadas.

Estas llamadas repetidas comprueban que el endpoint establece el estado solicitado y que no funciona como un interruptor que cambia de activo a inactivo en cada ejecución.

#### Pistas para investigar, sin revelar la solución

- Coloca breakpoints en la acción del controlador y en el método de servicio que actualiza el estado.
- Comprueba el valor de `dto.IsActive` al entrar al servicio.
- Observa qué método de la entidad se ejecuta en cada rama.
- Compara el objeto devuelto con el resultado del `GET` posterior.

---

## Nivel 2 — Intermedio

### Ejercicio 2: especialidades repetidas en un doctor

#### Funcionalidad que estás probando

Un cliente puede enviar accidentalmente IDs repetidos en `specialtyIds`. La API debe interpretar la lista como un conjunto: cada especialidad debe quedar asignada una sola vez.

Puedes reproducirlo al crear un doctor con `POST /api/doctors`. Después de corregirlo, también debes validar el mismo comportamiento al editar con `PUT /api/doctors/{id}`.

#### Preparación

1. Ejecuta `GET /api/specialties` y elige dos especialidades activas diferentes.
2. Anota sus IDs como `<specialtyAId>` y `<specialtyBId>`.
3. Utiliza una cédula profesional que no exista todavía.

#### Caso principal: crear con IDs repetidos

Envía:

```http
POST /api/doctors
```

```json
{
  "firstName": "Elena",
  "lastName": "Prueba",
  "professionalLicense": "DEBUG-DUP-001",
  "phoneNumber": "555-3001",
  "email": "elena.prueba@example.com",
  "specialtyIds": [<specialtyAId>, <specialtyAId>, <specialtyBId>]
}
```

Resultado esperado:

- La API no debe responder con un error interno `500`.
- Debe crear el doctor correctamente.
- El doctor debe mostrar exactamente dos especialidades: A y B.
- La relación con la especialidad A debe existir una sola vez.

#### Comprobación posterior

1. Guarda el `id` del doctor creado.
2. Ejecuta `GET /api/doctors/<doctorId>`.
3. Comprueba que `specialties` no contenga elementos duplicados.
4. Si quieres confirmarlo en SQL Server, consulta `DoctorSpecialties` agrupando por `DoctorId` y `SpecialtyId`.

#### Caso de control: actualizar

Ejecuta `PUT /api/doctors/<doctorId>` conservando los datos del doctor y enviando otra vez una lista repetida. El resultado debe continuar teniendo una sola relación por especialidad.

#### Pistas para investigar

- Revisa la lista recibida antes de consultar las especialidades.
- Sigue el punto donde la lista de IDs se convierte en entidades `DoctorSpecialty`.
- Distingue entre validar que todos los IDs existan y eliminar duplicados de la entrada.
- Una restricción de base de datos puede evitar datos duplicados, pero no sustituye el manejo correcto de la petición.

---

### Ejercicio 3: agenda consecutiva

#### Funcionalidad que estás probando

Dos citas son consecutivas cuando la primera termina exactamente en el instante en que comienza la segunda. Por ejemplo:

- Cita A: 09:00–10:00.
- Cita B: 10:00–11:00.

No comparten ningún minuto, por lo que ambas deben aceptarse.

#### Preparación

Localiza un paciente, un doctor, un consultorio y el estado `Scheduled`, todos activos. Elige una fecha futura sin citas existentes para esos recursos.

#### Paso 1: crear la primera cita

```http
POST /api/appointments
```

```json
{
  "patientId": <patientId>,
  "doctorId": <doctorAId>,
  "consultingRoomId": <roomAId>,
  "appointmentStatusId": <scheduledId>,
  "startDateTime": "2026-12-15T09:00:00",
  "endDateTime": "2026-12-15T10:00:00",
  "notes": "Primera cita del ejercicio 3"
}
```

Esta petición debe crear la cita.

#### Paso 2: crear la cita consecutiva

Usa el mismo doctor y consultorio:

```json
{
  "patientId": <patientId>,
  "doctorId": <doctorAId>,
  "consultingRoomId": <roomAId>,
  "appointmentStatusId": <scheduledId>,
  "startDateTime": "2026-12-15T10:00:00",
  "endDateTime": "2026-12-15T11:00:00",
  "notes": "Cita consecutiva del ejercicio 3"
}
```

Resultado esperado:

- Debe crearse con éxito.
- Ambas citas deben aparecer al consultar `GET /api/appointments`.

#### Casos de control

- Una cita de 09:30 a 10:30 sí debe rechazarse porque se traslapa.
- Una cita de 08:00 a 09:00 debe aceptarse porque también es consecutiva por el otro extremo.

#### Pistas para investigar

- Dibuja en papel los intervalos antes de revisar código.
- Observa cuidadosamente qué comparadores se usan con la hora inicial y final.
- Prueba la condición con horas iguales: `finA == inicioB`.

---

### Ejercicio 4: disponibilidad del doctor

#### Funcionalidad que estás probando

El conflicto del doctor debe evaluarse independientemente del consultorio. Cambiar de consultorio no hace que el mismo doctor pueda atender dos pacientes al mismo tiempo.

#### Preparación

Necesitas un doctor activo, dos consultorios activos diferentes, un paciente activo y el estado `Scheduled`. Elige una fecha futura libre.

#### Paso 1: crear la cita base

Crea una cita de 12:00 a 13:00 con el doctor A en el consultorio A.

```json
{
  "patientId": <patientId>,
  "doctorId": <doctorAId>,
  "consultingRoomId": <roomAId>,
  "appointmentStatusId": <scheduledId>,
  "startDateTime": "2026-12-15T12:00:00",
  "endDateTime": "2026-12-15T13:00:00",
  "notes": "Cita base del ejercicio 4"
}
```

#### Paso 2: intentar el traslape

Intenta crear otra cita de 12:30 a 13:30 con **el mismo doctor**, pero en el consultorio B.

Resultado esperado:

- La segunda petición debe rechazarse con un error de validación, normalmente HTTP `400`.
- Solo la primera cita debe quedar guardada.
- La razón debe ser la falta de disponibilidad del doctor.

#### Casos de control

- Otro doctor sí puede atender de 12:30 a 13:30 en el consultorio B.
- El mismo doctor sí puede atender después de las 13:00, siempre que no exista otra cita que lo impida.

#### Pistas para investigar

- Identifica qué datos recibe la consulta de conflictos.
- Pregúntate si la condición actual exige que coincidan simultáneamente doctor y consultorio.
- Separa mentalmente estas dos preguntas: “¿está ocupado el doctor?” y “¿está ocupado el consultorio?”.

---

### Ejercicio 5: disponibilidad del consultorio

#### Funcionalidad que estás probando

El conflicto del consultorio también debe evaluarse por separado. Elegir otro doctor no permite usar el mismo consultorio para dos citas traslapadas.

#### Preparación

Necesitas dos doctores activos diferentes, un consultorio activo, un paciente y el estado `Scheduled`. Usa una fecha futura libre.

#### Paso 1: crear la cita base

Crea una cita de 14:00 a 15:00 con el doctor A en el consultorio A.

#### Paso 2: intentar el traslape

Intenta crear otra cita de 14:30 a 15:30 con **otro doctor**, pero en el mismo consultorio A.

Resultado esperado:

- La segunda petición debe rechazarse con un error de validación, normalmente HTTP `400`.
- Solo la cita base debe quedar almacenada.
- La razón debe ser la falta de disponibilidad del consultorio.

#### Ejemplo del segundo cuerpo

```json
{
  "patientId": <patientId>,
  "doctorId": <doctorBId>,
  "consultingRoomId": <roomAId>,
  "appointmentStatusId": <scheduledId>,
  "startDateTime": "2026-12-15T14:30:00",
  "endDateTime": "2026-12-15T15:30:00",
  "notes": "Traslape de consultorio del ejercicio 5"
}
```

#### Casos de control

- El doctor B sí puede atender en ese horario si utiliza el consultorio B.
- El consultorio A puede reutilizarse a partir de las 15:00 si la cita anterior termina exactamente a esa hora.

#### Pistas para investigar

- Compara este ejercicio con el 4: la regla temporal es la misma, pero cambia el recurso cuya disponibilidad importa.
- Evita corregir un caso rompiendo el otro; vuelve a ejecutar ambos ejercicios después de cada cambio.

---

## Nivel 3 — Complejo

### Ejercicio 6: una cita cancelada y la agenda

#### Funcionalidad que estás probando

Una cita cancelada debe conservarse como parte del historial, pero ya no debe bloquear el horario del doctor ni del consultorio.

Este ejercicio se realiza en tres operaciones diferentes: crear una cita, cambiar su estado mediante `PATCH` y después intentar reservar nuevamente el periodo.

#### Preparación

Localiza los IDs de `Scheduled` y `Cancelled` mediante `GET /api/appointment-statuses`. Elige un paciente, doctor y consultorio activos, además de una fecha futura libre.

#### Paso 1: crear la cita original

Crea una cita de 16:00 a 17:00 con estado `Scheduled` y guarda el `id` que devuelve la API.

#### Paso 2: cancelarla

Ejecuta:

```http
PATCH /api/appointments/<appointmentId>/status
```

```json
{
  "appointmentStatusId": <cancelledId>
}
```

Antes de continuar, ejecuta `GET /api/appointments/<appointmentId>` y confirma:

- `appointmentStatusId` corresponde a `Cancelled`.
- `appointmentStatusName` es `Cancelled`.
- La cita original continúa existiendo; no debe eliminarse físicamente.

#### Paso 3: reservar nuevamente el mismo periodo

Intenta crear una nueva cita con el mismo doctor, el mismo consultorio y el mismo horario de 16:00 a 17:00. Puedes usar el mismo paciente u otro.

Resultado esperado:

- La nueva cita debe aceptarse.
- La cita cancelada debe continuar visible en el historial.
- La nueva cita debe tener su propio ID y estado `Scheduled`.

#### Casos de control

- Una cita `Scheduled` o `Confirmed` sí debe bloquear un periodo traslapado.
- Cancelar una cita no debe borrar ni modificar la nueva cita creada después.

#### Pistas para investigar

- Confirma primero que el cambio de estado se guardó; de lo contrario estarías investigando dos problemas a la vez.
- Sigue la consulta que decide si existe un conflicto y revisa qué estados incluye.
- No dependas de un número fijo para representar `Cancelled`; obtén o compara el estado de una manera coherente con el diseño del proyecto.

---

### Ejercicio 7: transiciones finales de estado

#### Funcionalidad que estás probando

No basta con comprobar que el estado solicitado exista. También debe validarse si el cambio desde el estado actual tiene sentido para el negocio.

Para esta práctica usaremos estas reglas:

| Estado actual | Cambios permitidos |
|---|---|
| `Scheduled` | `Confirmed`, `Cancelled` |
| `Confirmed` | `Completed`, `Cancelled`, `No show` |
| `Completed` | Ninguno |
| `Cancelled` | Ninguno |
| `No show` | Ninguno |

Enviar nuevamente el mismo estado puede aceptarse sin cambios o rechazarse como operación innecesaria, pero debes elegir una conducta consistente y documentarla. Lo obligatorio es que un estado final no pueda regresar a uno anterior.

#### Caso A: cita completada

1. Crea una cita `Scheduled` en una fecha futura.
2. Cámbiala a `Confirmed`.
3. Cámbiala a `Completed`.
4. Confirma con `GET` que quedó completada.
5. Intenta cambiarla nuevamente a `Scheduled`.
6. Intenta cambiarla a `Confirmed`.

Resultado esperado:

- Los intentos de sacar la cita de `Completed` deben rechazarse, normalmente con HTTP `400`.
- Después de cada intento, el `GET` debe seguir devolviendo `Completed`.

#### Caso B: cita cancelada

1. Crea otra cita `Scheduled`.
2. Cámbiala a `Cancelled`.
3. Intenta regresarla a `Scheduled` y después a `Confirmed`.

Resultado esperado:

- Ambos cambios deben rechazarse.
- El estado guardado debe continuar siendo `Cancelled`.

#### Caso de control: transición válida

Comprueba al menos una secuencia válida completa:

```text
Scheduled → Confirmed → Completed
```

También comprueba:

```text
Scheduled → Cancelled
```

#### Pistas para investigar

- La validación necesita conocer tanto el estado actual como el solicitado.
- Piensa si la regla pertenece únicamente al controlador o si debería protegerse en una capa más cercana al comportamiento del negocio.
- Verifica que una excepción no deje el estado modificado parcialmente antes de ser lanzada.

---

## Formato sugerido para documentar cada diagnóstico

Puedes copiar esta plantilla para cada ejercicio:

```text
Ejercicio:

Precondición:

Endpoint y datos enviados:

Resultado observado:

Resultado esperado:

Capas y métodos revisados:

Causa encontrada:

Corrección aplicada:

Pruebas posteriores:
```

## Recomendación de trabajo

Resuelve un ejercicio a la vez y crea un commit por corrección. No cambies varias capas simultáneamente hasta demostrar dónde está la causa. Después de corregir los ejercicios de agenda, vuelve a ejecutar los anteriores porque comparten reglas de disponibilidad.

Ejemplos de mensajes de commit:

```text
fix: respect requested consulting room status
fix: normalize repeated doctor specialty ids
fix: allow consecutive appointments without overlap
fix: validate doctor and room availability independently
fix: release cancelled appointment schedule
fix: enforce final appointment statuses
```

Cuando termines un ejercicio, puedes pedirme que revise tu diagnóstico y tu solución sin revelarte los errores restantes.
