# Clinic Management System — Backend base

API REST construida con .NET 8, Clean Architecture, Entity Framework Core 8 y SQL Server.

Esta entrega contiene solamente el backend. La aplicación Angular se desarrollará en una etapa posterior.

## Módulos incluidos

- Géneros: consulta del catálogo activo.
- Pacientes: consulta, alta, edición y activación/desactivación.
- Especialidades: consulta, alta, edición y activación/desactivación.
- Doctores: consulta, alta, edición, activación/desactivación y asignación de especialidades.
- Consultorios: consulta, alta, edición y activación/desactivación.
- Estados de cita: consulta del catálogo activo.
- Citas: consulta con filtros, alta, reprogramación y cambio de estado.
- Validaciones de entidades, manejo global de excepciones y cancelación de operaciones asíncronas.

## Requisitos

- .NET SDK 8.
- SQL Server 2019 o superior.
- Visual Studio 2022 o VS Code.

## Preparar la base de datos

Ejecuta los scripts de `database/tables` en este orden:

1. `Genders.sql`
2. `Patients.sql`
3. `Specialties.sql`
4. `Doctors.sql`
5. `DoctorSpecialties.sql`
6. `ConsultingRooms.sql`
7. `AppointmentStatus.sql`
8. `Appointments.sql`

Después ejecuta los scripts de `database/seed`:

1. `Seed_Genders_VALUES.sql`
2. `Seed_AppointmentStatus_VALUES.sql`
3. `Seed_Patients.sql`
4. `Seed_ClinicDemoData.sql`

Configura `DefaultConnection` en `src/ClinicManagementSystem.Api/appsettings.Development.json` con tu instancia local de SQL Server.

## Compilar y ejecutar

Desde la raíz de la solución:

```powershell
dotnet restore ClinicManagementSystem.sln
dotnet build ClinicManagementSystem.sln
dotnet test ClinicManagementSystem.sln
dotnet run --project src/ClinicManagementSystem.Api/ClinicManagementSystem.Api.csproj
```

Abre la dirección de Swagger mostrada en la terminal, normalmente `https://localhost:xxxx/swagger`.

## Endpoints principales

| Recurso | Operaciones |
|---|---|
| `/api/gender` | GET |
| `/api/patients` | GET, GET por id, POST, PUT, PATCH status |
| `/api/specialties` | GET, GET por id, POST, PUT, PATCH status |
| `/api/doctors` | GET, GET por id, POST, PUT, PATCH status |
| `/api/consultingrooms` | GET, GET por id, POST, PUT, PATCH status |
| `/api/appointment-statuses` | GET |
| `/api/appointments` | GET con filtros, GET por id, POST, PUT schedule, PATCH status |

## Práctica de depuración

Esta es deliberadamente una versión base de práctica. Compila, pero contiene varios errores de comportamiento introducidos para diagnosticarlos desde Swagger, el depurador, logs y pruebas. Los casos esperados están en `EJERCICIOS_DEBUGGING.md`; ese archivo no indica qué clase o línea debes modificar.

Antes de corregirlos, crea tu propia rama de Git:

```powershell
git switch -c practica/debug-backend
```
