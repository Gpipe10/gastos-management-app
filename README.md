# Gastos Management App

Aplicación web full-stack para la gestión de gastos mensuales.

## Tecnologías

- Backend: .NET 10 Web API  
- Entity Framework Core  
- SQL Server  
- Frontend: Angular + TypeScript  

## Requisitos

Para ejecutar el proyecto se necesita:

- Git  
- .NET SDK 10.0  
- Node.js 18+ y npm  
- SQL Server (LocalDB, Express o Developer)  

Verificar que esten los requisitos corerctamente instalados valda en cmd win:

git --version

dotnet --version

node --version

npm --version


Si el comando `dotnet` no se reconoce, asegúrate de que la herramienta `dotnet` esté instalada y que la ruta `C:\Program Files\dotnet\` esté agregada a la variable de entorno **PATH**. Luego reinicia la terminal.

---

## Clonar el repositorio

git clone https://github.com/Gpipe10/gastos-management-app.git  
cd gastos-management-app  

---
## Configuración de Puerto 

## Configuración de Base de Datos

Editar el archivo:

backend/GastosManagement.Api/appsettings.Development.json  

Agregar o configurar la sección antes de :  

### Opción LocalDB

"ConnectionStrings": {  
  "GastosDb": "Server=(localdb)\\MSSQLLocalDB;Database=GastosManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"  
}

### Opción SQL Server Express

"ConnectionStrings": {  
  "GastosDb": "Server=localhost\\SQLEXPRESS;Database=GastosManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"  
}

---

## Crear la Base de Datos

IMPORTANTE: Los siguientes comandos deben ejecutarse desde la raíz del proyecto (no dentro de la carpeta backend).

Restaurar dependencias:

dotnet restore .\backend\GastosManagement.Infrastructure\GastosManagement.Infrastructure.csproj  
dotnet restore .\backend\GastosManagement.Api\GastosManagement.Api.csproj  

Instalar la herramienta EF :

dotnet tool install --global dotnet-ef  

Si el comando `dotnet ef` no se reconoce, asegúrate de que la herramienta `dotnet-ef` esté instalada y que la ruta `C:\Users\TU_USUARIO\.dotnet\tools` esté agregada a la variable de entorno **PATH**. Luego reinicia la terminal.


Aplicar migraciones:

dotnet ef database update --project .\backend\GastosManagement.Infrastructure --startup-project .\backend\GastosManagement.Api  

Esto creará automáticamente la base de datos y las tablas.

---

## Ejecutar el Backend

Desde la raíz del proyecto:

dotnet run --project .\backend\GastosManagement.Api  

Swagger estará disponible en:

https://localhost:7001/swagger/index.html

http://localhost:5000/swagger/index.html

(El puerto exacto se mostrará en la consola)

---

## Ejecutar el Frontend

En otra terminal:

cd frontend/gastos-management-ui  
npm install  
npm start  

Abrir en navegador:

http://localhost:4200  

---

## Nota

El backend permite conexiones desde http://localhost:4200 mediante CORS.  

Si se cambia el puerto del frontend, deberá actualizarse la política CORS en Program.cs.

---

## Autor

Gpipe10
