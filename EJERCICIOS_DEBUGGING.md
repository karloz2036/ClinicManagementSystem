# Ejercicios de búsqueda y corrección de errores

El backend contiene errores intencionales de comportamiento. No todos producen una excepción: algunos devuelven una respuesta aparentemente correcta, pero dejan datos incorrectos o permiten una operación que la clínica debe rechazar.

Tu objetivo en cada ejercicio es:

1. Reproducir el problema desde Swagger.
2. Escribir los datos de entrada y el resultado observado.
3. Seguir el flujo Controller → Service → Repository/Domain.
4. Explicar la causa antes de modificar código.
5. Corregirlo y volver a probar el escenario normal y uno límite.

## Nivel 1 — Fácil

### Ejercicio 1: estado de un consultorio

Al enviar `isActive: false` a un consultorio activo, el resultado y el dato guardado deberían quedar inactivos. Al enviar `isActive: true`, deberían quedar activos.

Verifica ambos sentidos y las llamadas repetidas.

## Nivel 2 — Intermedio

### Ejercicio 2: especialidades repetidas en un doctor

Un cliente puede enviar accidentalmente una lista como `[1, 1, 2]`. La API no debe terminar con un error interno ni intentar guardar dos veces la misma relación. El doctor debe quedar relacionado una sola vez con cada especialidad válida.

### Ejercicio 3: agenda consecutiva

Si una cita termina a las 10:00 y otra comienza exactamente a las 10:00, las dos deben ser aceptadas porque no se traslapan.

### Ejercicio 4: disponibilidad del doctor

Un doctor no puede atender dos citas traslapadas, aunque se seleccionen consultorios diferentes.

### Ejercicio 5: disponibilidad del consultorio

Un consultorio no puede ocuparse en dos citas traslapadas, aunque se seleccionen doctores diferentes.

## Nivel 3 — Complejo

### Ejercicio 6: una cita cancelada y la agenda

Una cita cancelada ya no debe bloquear el horario del doctor ni del consultorio. Confirma primero que el estado realmente se guardó y después intenta reservar el mismo periodo.

### Ejercicio 7: transiciones finales de estado

Una cita `Completed` o `Cancelled` representa un estado final. La API debe rechazar intentos de volverla a `Scheduled` o `Confirmed`. Define claramente qué transiciones sí están permitidas antes de programarlas.

## Recomendación de trabajo

Resuelve un solo ejercicio a la vez y crea un commit por corrección. No cambies varias capas simultáneamente hasta demostrar en cuál está la causa.

Ejemplo de mensajes de commit:

```text
fix: respect requested consulting room status
fix: allow consecutive appointments without overlap
```

Cuando termines un ejercicio, puedes pedirme que revise tu diagnóstico y tu solución sin que te revele los errores restantes.
