# 🚀 Task Management Application

This is a small full‑stack **Task Management** system built as a portfolio/demo project.

- **Backend:** ASP.NET Core (C#) + Dapper + xUnit tests  
- **Frontend:** React + Vite + TypeScript + MobX  
- **Database:** SQL Server (running in Docker)  
- **Orchestration:** Docker Compose – runs API + UI + DB together

The app supports:
- Creating, editing and deleting tasks
- Marking tasks as **Done / Undo**
- Filtering by status: **All / Active / Completed**
- Setting **priority** (Low / Medium / High)
- Optional **due date** for each task

---

## 📂 Project layout

The solution is split into two repositories: one for the backend and one for the frontend.

The expected folder layout on disk is:

```
<parent-folder>/
│
├─ TestManagement/              # Backend repo (this one)
│   ├─ TestManagement.sln
│   ├─ Dockerfile
│   ├─ docker-compose.yml
│   ├─ TestManagement/          # ASP.NET Core project
│   │   ├─ Controllers/
│   │   ├─ Dtos/
│   │   ├─ Models/
│   │   ├─ Repositories/
│   │   ├─ appsettings.json
│   │   └─ Program.cs
│   └─ TestManagement.Tests/    # xUnit tests
│
└─ Frontend/
    └─ task-management-ui/      # React + Vite + TS + MobX frontend
        ├─ src/
        │   ├─ api/
        │   ├─ components/
        │   ├─ pages/
        │   ├─ stores/
        │   └─ types/
        ├─ Dockerfile
        └─ vite.config.ts
```

Docker Compose assumes that:
- you run it **from the `TestManagement` backend folder**, and  
- the frontend repository is located at `../Frontend/task-management-ui` relative to that folder.

---

## ✅ Prerequisites

To run the app with Docker you need:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- [Git](https://git-scm.com/downloads)  

On Windows, Docker Desktop also requires:
- Virtualization enabled in BIOS (Intel VT‑x / AMD‑V)  
- WSL2 installed and configured

---

## 📥 Cloning the repositories

In a folder of your choice (for example `C:\Projects`):

```bash
git clone https://github.com/Dejan21/TestManagement
git clone https://github.com/Dejan21/task-management-ui Frontend/task-management-ui
```

After this, the structure under `C:\Projects` should match the layout shown above.



---

## 🐳 Running the full stack with Docker

1. Open a terminal **in the backend folder**:

   ```bash
   cd TestManagement
   ```

2. Build and start all services:

   ```bash
   docker compose up --build
   ```

   This will start:
   - SQL Server container  
   - ASP.NET Core API  
   - React frontend

3. Once containers are running, open:

   - Frontend UI: **http://localhost:5173**  
   - API Swagger UI: **http://localhost:8080/swagger**  

---

## 🗄 Database connection

The SQL Server instance runs inside Docker and is exposed on port **1434** on the host.

Connection details:

- **Server:** `localhost,1434`  
- **User:** `sa`  
- **Password:** `TaskUser!123`  
- **Database:** `TaskManagementDb`  

The backend uses a connection string equivalent to:

```text
Server=db,1433;Database=TaskManagementDb;User Id=sa;Password=TaskUser!123;TrustServerCertificate=True;
```

(`db` is the container name used inside the Docker network.)

---

## 🖥 Running the projects without Docker (optional)

You can also run the backend and frontend separately on your local machine.

### 1. Backend (ASP.NET Core)

Requirements:
- .NET SDK installed (version matching the project)

Commands:

```bash
cd TestManagement
dotnet restore
dotnet run
```

By default the API will listen on `http://localhost:8080` (or the port configured in `launchSettings.json`).

Swagger will be available at:

```text
http://localhost:8080/swagger
```

### 2. Frontend (React + Vite)

Requirements:
- Node.js (LTS)  
- npm

Commands:

```bash
cd Frontend/task-management-ui
npm install
npm run dev -- --host --port 5173
```

The app will be available at:

```text
http://localhost:5173
```

Make sure the frontend is configured to call the correct API base URL (for example `http://localhost:8080`), either via `VITE_API_BASE_URL` in `.env` or via Docker environment variables.

---

## 🧪 Tests

The backend contains an `TestManagement.Tests` project with xUnit tests for the API.

To run the tests:

```bash
cd TestManagement
dotnet test
```

---

## ⚠️ Common issues

| Problem                          | Possible fix                                                                 |
|----------------------------------|------------------------------------------------------------------------------|
| Docker Desktop does not start    | Enable virtualization (Intel VT‑x / AMD‑V) in BIOS and ensure WSL2 is set up |
| `WSL2 is not supported` errors   | Install WSL2: `wsl --install`, then restart                                 |
| Port 8080 or 5173 already in use | Stop the other app or change ports in `docker-compose.yml`                  |
| Cannot connect to SQL Server     | Use `localhost,1434` and check that containers are running                  |

---

## 👤 Author

**Dejan Jangelovski**

If you find this project useful, feel free to star the repository.
