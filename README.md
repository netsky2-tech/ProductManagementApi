# Introducci�n

Este es una API desarrollada en .NET Core 8 para la gesti�n de productos, categorias y unidades de medida

El objetivo principal es demostrar habilidades en la implementaci�n de APIs con buenas pr�cticas y dise�o profesional.

---

## Requisitos previos

Antes de iniciar asegurate de tener instalado los siguientes requisitos:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
- Un editor de c�digo como [Visual Studio Code](https://code.visualstudio.com/) o [Visual Studio](https://visualstudio.microsoft.com/)

---

# 1. Clonar el repositorio

git clone https://github.com/netsky2-tech/ProductManagementApi.git

cd ProductManagementApi

# 2. Configurar la base de datos

Aseg�rate de tener un servidor SQL Server en ejecuci�n

Modifica la cadena de conexi�n en el archivo appsettings.json ubicado en la ra�z del proyecto  

# 3. Instalas las dependencias necesarias necesarias

*Omitir este paso si ya se cuenta con las herramientas instaladas en el equipo*

*dotnet tool install --global dotnet-ef**

Verificar si la instalaci�n fue correcta con 

*dotnet ef --version*

# 4. Ejecutar las migraciones de la base de datos

Ejecuta los siguientes comandos de dotnet en la terminal para aplicar las migraciones

dotnet ef migrations add InitialCreate
dotnet ef database update


# 5. Ejecuta la API

Ejecuta el proyecto 

La api estar� disponible en https://localhost:7078

