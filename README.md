# 📎 API RESTful de Acortamiento de URLs

Este proyecto implementa una API RESTful en **C# con ASP.NET Core** y **SQLite** como base de datos. Permite a los usuarios crear URLs cortas a partir de URLs largas, recuperarlas, actualizarlas, eliminarlas y ver estadísticas de uso. Además, incluye funcionalidad de redirección automática.

https://roadmap.sh/projects/url-shortening-service

## Tecnologías usadas

- **Lenguaje:** C# (.NET 8 o superior)
- **Framework:** ASP.NET Core Web API
- **Base de datos:** SQLite
- **ORM:** Entity Framework Core
- **Testing & Visualización:** Swagger UI

---

##  Estructura del proyecto

UrlShortenerAPI/
├── Controllers/
│ └── ShortenController.cs
├── Data/
│ └── ApplicationDbContext.cs
├── Models/
│ ├── UrlMapping.cs
│ └── UrlMappingRequest.cs
├── Migrations/
├── urlshortener.db ← Se genera automáticamente
├── appsettings.json
├── Program.cs
└── UrlShortenerAPI.csproj

---

##  Configuración inicial

1. **Clona o descarga el proyecto.**
2. Asegúrate de tener instalado `.NET SDK 8+`.
3. Crea la base de datos y sus migraciones:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
Ejecuta el proyecto:

bash
dotnet run

## Endpoints de la API
1. Crear una URL corta
POST /Shorten

Body:

json

{
  "url": "https://www.example.com/some/long/url"
}
Respuesta:

json

{
  "id": 1,
  "url": "https://www.example.com/some/long/url",
  "shortCode": "ABC123",
  "createdAt": "2025-06-25T22:00:00Z",
  "updatedAt": "2025-06-25T22:00:00Z",
  "accessCount": 0
}
2. Redireccionar usando el código corto
GET /Shorten/{shortCode}

 Este endpoint redirige automáticamente a la URL original.

 Nota: Esto no funciona en Swagger por políticas CORS. Usa un navegador o Postman.

3. Obtener los datos originales de la URL corta
GET /Shorten/{shortCode}/stats

Respuesta:

json

{
  "id": 1,
  "url": "https://www.example.com/some/long/url",
  "shortCode": "ABC123",
  "createdAt": "2025-06-25T22:00:00Z",
  "updatedAt": "2025-06-25T22:05:00Z",
  "accessCount": 3
}
4. Actualizar la URL original
PUT /Shorten/{shortCode}

Body:

json

{
  "url": "https://www.example.com/updated/url"
}
Respuesta:

json
Copiar
Editar
{
  "id": 1,
  "url": "https://www.example.com/updated/url",
  "shortCode": "ABC123",
  "createdAt": "...",
  "updatedAt": "...",
  "accessCount": ...
}

5. Eliminar una URL corta
DELETE /Shorten/{shortCode}

Respuesta: 204 No Content

## Ver la base de datos SQLite
Puedes inspeccionar el archivo urlshortener.db con la extensión:

Instala SQLite Viewer o SQLite en VS Code.

Haz clic derecho sobre el archivo urlshortener.db y selecciona Open Database.

Examina la tabla UrlMappings.

### Notas adicionales
El archivo urlshortener.db se crea al ejecutar la app y hacer la primera operación con la base de datos.

Si eliminas el archivo .db, los datos se perderán (es persistente mientras no se borre manualmente).

El endpoint de redirección no se puede probar desde Swagger (usa navegador o Postman).