# GestionITM API 🎓

API REST desarrollada en ASP.NET Core con arquitectura de múltiples capas para la gestión de estudiantes, cursos y matrículas.

---

## 🏗️ Arquitectura

El proyecto sigue el patrón de **N-Capas**:
```
MultiplesCapas/
├── GestionITM.Api           → Controladores, Middleware, configuración HTTP
├── GestionITM.Domain        → Entidades, DTOs, Interfaces, Excepciones
└── GestionITM.Infrastructure → Repositorios, Servicios, DbContext
```

---

## 🚀 Tecnologías

- ASP.NET Core 8
- Entity Framework Core (Code First)
- SQL Server
- AutoMapper
- Arquitectura N-Capas

---

## ⚙️ Configuración inicial

### 1. Clonar el repositorio
```bash
git clone https://github.com/alvarouribe222132/MultiplesCapas.git
cd MultiplesCapas
```

### 2. Configurar la cadena de conexión
En `GestionITM.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=GestionITM;Trusted_Connection=True;"
  }
}
```

### 3. Aplicar migraciones
```bash
Update-Database
```

### 4. Ejecutar
```bash
dotnet run --project GestionITM.Api
```

---

## 📦 Endpoints disponibles

### Estudiantes
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/estudiante` | Obtener todos |
| GET | `/api/estudiante/{id}` | Obtener por ID |
| POST | `/api/estudiante` | Crear estudiante |

---

## 🛡️ Manejo de errores

La API cuenta con un middleware global (`ExceptionMiddleWare`) que intercepta todas las excepciones y responde con el código HTTP correcto:

| Excepción | StatusCode | Cuándo ocurre |
|-----------|------------|---------------|
| `NotFoundException` | 404 | Recurso no encontrado |
| `BadRequestException` | 400 | Datos inválidos |
| `UnauthorizedException` | 401 | Sin permisos |
| `ConflictException` | 409 | Duplicado o conflicto |
| `Exception` (genérica) | 500 | Error inesperado |

---

## 🗃️ Modelo de datos

### Estudiante
| Campo | Tipo | Descripción |
|-------|------|-------------|
| EstudianteId | int | PK autoincremental |
| Name | string | Nombre completo |
| Correo | string | Correo electrónico |
| Telefono | string | Teléfono de contacto |
| Documento | string | Número de documento |
| FechaInscripcion | DateTime | Fecha de registro |

---

## 👨‍💻 Autor
**Álvaro Uribe**  
[GitHub](https://github.com/alvarouribe222132)

## 👨‍💻 Profesor
**Daniel Villamizar**
[GitHub](https://github.com/CSA-DanielVillamizar)


