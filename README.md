
# ClinicalTrialsAPI

A robust and scalable RESTful API for managing clinical trial metadata. This guide walks you through setting up the application, configuring the database, running migrations, and testing.

---

## **Features**
- Upload JSON metadata for clinical trials.
- Validate and process uploaded files.
- Store and retrieve processed metadata in a PostgreSQL database.
- Fully containerized for consistent deployment.
- Supports integration testing with an in-memory database.

---

## **Prerequisites**
- **.NET 8 SDK**
- **PostgreSQL** (or Docker for containerized setup)
- **Docker & Docker Compose** (optional for containerization)
- **EF Core CLI** for database migrations:
  ```bash
  dotnet tool install --global dotnet-ef
  ```
dotnet ef dbcontext scaffold "Host=localhost;Database=ClinicalTrialsDb;Username=yourusername;Password=yourpassword" Npgsql.EntityFrameworkCore.PostgreSQL -o Models --context ClinicalTrialDbContext --context-dir Persistence --force
---

## **Setup Instructions**

### **1. Clone the Repository**
```bash
https://github.com/ggulbani/ClinicalTrial/tree/master
```

---

### **2. Configure the Database**

#### **Using PostgreSQL**
1. Install PostgreSQL on your system or run it using Docker:
   ```bash
   docker run --name postgres-clinicaltrials -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ClinicalTrialsDb -p 5432:5432 -d postgres:15
   ```

2. Update the connection string in `appsettings.json`:
   **File: `ClinicalTrialsAPI.Api/appsettings.json`**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=ClinicalTrialsDb;Username=yourusername;Password=yourpassword"
     }
   }
   ```

---

### **3. Run Database Migrations**

Use the EF Core CLI to scaffold the database and apply migrations.

#### **Scaffold the Database**
1. Create the initial migration:
   ```bash
   dotnet ef migrations add InitialCreate --project ClinicalTrialsAPI.Infrastructure
   ```
2. Apply the migration to create the database schema:
   ```bash
   dotnet ef database update --project ClinicalTrialsAPI.Infrastructure
   ```

#### **Verify the Database**
Check the `ClinicalTrialsDb` database in your PostgreSQL instance. It should contain tables like `ClinicalTrials`.

---

### **4. Run the Application**

Start the application:
```bash
dotnet run --project ClinicalTrialsAPI.Api
```

The API will run at `https://localhost:5000` (or another configured port).

---

### **5. Testing the API**

#### **Using Swagger**
1. Navigate to `https://localhost:5000/swagger`.
2. Use the interactive Swagger UI to test the endpoints.

#### **Using Postman**
1. Import the provided API collection in Postman (if available).
2. Test the following endpoints:
   - **POST `/api/clinicaltrials/upload`**: Upload a JSON file.
   - **GET `/api/clinicaltrials/{id}`**: Retrieve clinical trial metadata by ID.
   - **GET `/api/clinicaltrials`**: Retrieve clinical trials filtered by status.

---

### **6. Run Tests**

#### **Unit and Integration Tests**
Run the test suite:
```bash
dotnet test
```

The integration tests use an **in-memory database** to avoid affecting the production database.

#### **Custom Test Database**
If you prefer to test with a PostgreSQL database:
1. Set the environment to `Testing` in `Program.cs`:
   ```csharp
   if (builder.Environment.IsEnvironment("Testing"))
   {
       options.UseInMemoryDatabase("TestDatabase");
   }
   ```
2. Configure the `CustomWebApplicationFactory` to override the database provider in tests.

---

### **7. Containerization (Optional)**

#### **Build the Docker Image**
1. Build the Docker image:
   ```bash
   docker build -t clinicaltrialsapi .
   ```

2. Run the Docker container:
   ```bash
   docker run -d -p 5000:5000 --name clinicaltrialsapi clinicaltrialsapi
   ```

#### **Using Docker Compose**
If using `docker-compose.yml`, start the services:
```bash
docker-compose up
```

This will run both the API and a PostgreSQL database in containers.

---

### **8. Advanced Configuration**

#### **Environment Variables**
Configure environment variables in the `docker-compose.yml` file or your deployment environment:
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=ClinicalTrialsDb;Username=postgres;Password=postgres
```

#### **Logging**
Logs are stored in:
- Console (default).
- File (if enabled in `Program.cs`):
  ```plaintext
  Logs/clinicaltrialsapi-log-{Date}.txt
  ```

---

### **9. Troubleshooting**

#### **Common Issues**
1. **Database Connection Error**: Ensure PostgreSQL is running and the connection string is correct.
2. **Missing Migrations**: Run the migration commands in step 3.
3. **Port Conflicts**: Check for conflicting applications using port `5000`.

#### **Logs**
Check application logs for details:
- Console: Output in terminal.
- File: Stored in the `Logs` directory (if configured).

---

### **10. API Endpoints**

#### **Upload Clinical Trial**
- **POST** `/api/clinicaltrials/upload`
- Headers: `Content-Type: multipart/form-data`
- Body: Upload a `.json` file containing clinical trial metadata.

#### **Retrieve Clinical Trial by ID**
- **GET** `/api/clinicaltrials/{id}`

#### **Retrieve Clinical Trials by Status**
- **GET** `/api/clinicaltrials?status=Ongoing`

---
