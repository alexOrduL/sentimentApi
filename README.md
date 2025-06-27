# Sentiment API

API RESTful para gestionar comentarios de productos y analizar su sentimiento. Desarrollada en **ASP.NET Core**, usando **Entity Framework Core** con **SQL Server**, y contenida con **Docker**.

---

## 🚀 Características

- CRUD de comentarios asociados a productos.
- Análisis básico de sentimiento basado en contenido textual.
- Endpoints REST versionados bajo `/api`.
- Base de datos persistente en SQL Server (Docker).
- Pruebas unitarias y de integración opcionales.
- Documentación automática con Swagger.

---

## 🧱 Requisitos


- [Docker](https://www.docker.com/)
- (Opcional) DBeaver para explorar la base de datos.

---

## 🐳 Ejecución con Docker


1. Clona el repositorio:

   ```bash
   git clone https://github.com/tu_usuario/sentiment-api.git
   cd sentiment-api
   ```
   
2. Levanta los servicios (API + SQL Server) con Docker Compose:

      ```bash
      docker compose up -d --build
      ```

3. Accede a la API y documentación Swagger en:
      ```bash
      http://localhost:5001/swagger/index.html
      ```
4. Para detener los contenedores:
      ```bash
      docker compose down
      ```

