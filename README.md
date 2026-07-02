# LaTiendaAPI - ASP.NET Core Web API .NET 8.0

## 📋 Descripción
API REST completa con arquitectura MVC, Entity Framework Core, AutoMapper, JWT Authentication.

## 🔧 Dependencias (todas versión 8.0.0)
| Paquete | Versión |
|---|---|
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.0 |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.0 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 |
| Microsoft.IdentityModel.Tokens | 8.0.0 |
| System.IdentityModel.Tokens.Jwt | 8.0.0 |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 12.0.1 |
| Swashbuckle.AspNetCore | 6.6.2 |

## 🗄️ Base de Datos
- **Servidor:** `DESKTOP-FBIH4AI\SQLEXPRESS`
- **Base de datos:** `latienda`
- **Autenticación:** Windows (Integrated Security)

## 🚀 Pasos para ejecutar

### 1. Crear la base de datos
Ejecutar el script `BaseDatos_latienda.sql` en SQL Server Management Studio.

### 2. Abrir el proyecto en Visual Studio 2022
Abrir `LaTiendaAPI.csproj`

### 3. Verificar appsettings.json
La cadena de conexión ya apunta a `DESKTOP-FBIH4AI\SQLEXPRESS`.

### 4. Instalar paquetes NuGet
Visual Studio los instala automáticamente. Si hay error, ejecutar en la Consola del Administrador de Paquetes:
```
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.0
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.0
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection -Version 12.0.1
Install-Package Swashbuckle.AspNetCore -Version 6.6.2
```

### 5. Ejecutar el proyecto
Presionar F5 o Ctrl+F5. Se abrirá Swagger en el navegador.

## 📁 Estructura del Proyecto
```
LaTiendaAPI/
├── Controllers/
│   ├── ProductoController.cs   ← CRUD de productos
│   └── AuthController.cs       ← Login y Registro
├── Models/
│   ├── Categoria.cs
│   ├── Producto.cs
│   ├── Role.cs
│   ├── Usuario.cs
│   ├── UsuarioRole.cs
│   └── LatiendaContext.cs      ← DbContext (EF Core)
├── DTOs/
│   ├── ProductoDto.cs          ← ProductoDto, ProductoCreateDto, CategoriaDto
│   └── Auth/
│       └── AuthDtos.cs         ← LoginRequestDto, LoginResponseDto, RegisterRequestDto
├── Mappings/
│   └── MappingProfile.cs       ← AutoMapper profiles
├── Services/
│   └── JwtService.cs           ← Generador de tokens JWT
├── Helpers/
│   └── PasswordHelper.cs       ← Hash/Salt de contraseñas
├── appsettings.json
├── Program.cs
└── BaseDatos_latienda.sql
```

## 🔌 Endpoints disponibles

### Productos (`/api/producto`)
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/producto/Lista` | Listar todos los productos |
| GET | `/api/producto/Obtener/{id}` | Obtener un producto por ID |
| POST | `/api/producto/Guardar` | Crear nuevo producto |
| PUT | `/api/producto/Editar/{id}` | Editar un producto |
| DELETE | `/api/producto/Eliminar/{id}` | Eliminar un producto |

### Auth (`/api/auth`)
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/auth/login` | Iniciar sesión → retorna JWT |
| POST | `/api/auth/Registrar` | Registrar nuevo usuario |

## 🧪 Pruebas en Swagger

### Registrar usuario:
```json
POST /api/auth/Registrar
{
  "tipoDoc": "CC",
  "nroDoc": "44444",
  "nombre": "Magno",
  "email": "Hola.com",
  "password": "44444",
  "roles": [1]
}
```

### Login:
```json
POST /api/auth/login
{
  "email": "Hola",
  "password": "44444"
}
```

### Guardar producto:
```json
POST /api/producto/Guardar
{
  "idCategoria": 2,
  "nombre": "Camisa",
  "precio": 4200,
  "stock": 15,
  "estado": true
}
```
