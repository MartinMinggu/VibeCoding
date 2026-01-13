# Quick Start Commands - Oracle Database Setup

## 📝 After Installation - First Steps

### 1. Verify Oracle Installation

```powershell
# Check if Oracle service is running
Get-Service -Name "OracleServiceXE" | Select-Object Name, Status, DisplayName

# Check Oracle Listener
Get-Service -Name "OracleOraDB21Home1TNSListener*" | Select-Object Name, Status
```

Expected output:
```
Name              Status
----              ------
OracleServiceXE   Running
```

### 2. Test SQL*Plus Connection

```powershell
# Open SQL*Plus as SYSDBA
sqlplus sys as sysdba
```

Enter your password when prompted, then run:

```sql
-- Check Oracle version
SELECT banner FROM v$version;

-- Check database status
SELECT instance_name, status FROM v$instance;

-- Exit
exit;
```

### 3. Create Application Database User

Copy dan paste script ini ke SQL*Plus (setelah login sebagai SYSDBA):

```sql
-- Switch to Pluggable Database
ALTER SESSION SET CONTAINER = XEPDB1;

-- Create user (ganti password sesuai keinginan)
CREATE USER ECOMMERCE_USER IDENTIFIED BY "Oracle2024!";

-- Grant privileges
GRANT CONNECT, RESOURCE TO ECOMMERCE_USER;
GRANT CREATE VIEW, CREATE SYNONYM TO ECOMMERCE_USER;
GRANT UNLIMITED TABLESPACE TO ECOMMERCE_USER;
GRANT CREATE TABLE, CREATE SEQUENCE, CREATE TRIGGER TO ECOMMERCE_USER;
GRANT ALTER ANY TABLE, DROP ANY TABLE TO ECOMMERCE_USER;

-- Verify user created
SELECT username FROM dba_users WHERE username = 'ECOMMERCE_USER';

-- Should return: ECOMMERCE_USER

exit;
```

### 4. Test Application User Connection

```powershell
# Connect as application user
sqlplus ECOMMERCE_USER/Oracle2024!@localhost:1521/XEPDB1
```

If successful:
```sql
-- Show current user
SHOW USER;

-- Should show: USER is "ECOMMERCE_USER"

exit;
```

### 5. Update Project NuGet Packages

Di folder project:

```powershell
# Navigate to Infrastructure project
cd ECommerceApp.Infrastructure

# Remove SQL Server
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer

# Add Oracle packages
dotnet add package Oracle.EntityFrameworkCore --version 8.23.50
dotnet add package Oracle.ManagedDataAccess.Core --version 23.5.1

# Back to root
cd ..
```

### 6. Update appsettings.json

Edit file: `ECommerceApp.Web\appsettings.json`

Ganti connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1"
  }
}
```

### 7. Test Connection from .NET

Create a test file or run this in Program.cs temporarily:

```csharp
using Oracle.ManagedDataAccess.Client;

try 
{
    var connectionString = "User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1";
    using (var conn = new OracleConnection(connectionString))
    {
        conn.Open();
        Console.WriteLine("✅ Connected to Oracle successfully!");
        Console.WriteLine($"Oracle Version: {conn.ServerVersion}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Connection failed: {ex.Message}");
}
```

## 🔍 Troubleshooting Quick Commands

### Check if Oracle is listening

```powershell
netstat -an | Select-String "1521"
```

Should show:
```
TCP    0.0.0.0:1521           0.0.0.0:0              LISTENING
```

### Restart Oracle Services

```powershell
# Restart database service
Restart-Service -Name OracleServiceXE -Force

# Restart listener
Restart-Service -Name "OracleOraDB21Home1TNSListener*" -Force
```

### Check Oracle Environment Variables

```powershell
# Check ORACLE_HOME
$env:ORACLE_HOME

# Check if Oracle is in PATH
$env:PATH -split ';' | Select-String "oracle"
```

### View Oracle Error Logs

```powershell
# Navigate to Oracle trace directory
cd C:\app\<YourUsername>\product\21c\dbhomeXE\diag\rdbms\xe\xe\trace

# View alert log (latest errors)
Get-Content alert_xe.log -Tail 50
```

## ✅ Verification Checklist

After running all commands above, verify:

- [ ] Oracle service is running
- [ ] Can connect with SQL*Plus as SYS
- [ ] Application user ECOMMERCE_USER created
- [ ] Can connect as ECOMMERCE_USER
- [ ] Oracle NuGet packages installed
- [ ] Connection string updated
- [ ] .NET can connect to Oracle

## 🚀 Next: Run Migrations

Once all above are ✅, run:

```powershell
# From project root
cd ECommerceApp.Web

# Create new Oracle migration (hapus folder Migrations lama terlebih dahulu!)
dotnet ef migrations add InitialOracleSetup --project ..\ECommerceApp.Infrastructure

# Apply to database
dotnet ef database update --project ..\ECommerceApp.Infrastructure

# Should create all tables in Oracle!
```

## 📊 Verify Tables Created

```sql
-- Connect as ECOMMERCE_USER
sqlplus ECOMMERCE_USER/Oracle2024!@localhost:1521/XEPDB1

-- List all tables
SELECT table_name FROM user_tables ORDER BY table_name;

-- You should see:
-- AspNetRoles
-- AspNetUsers
-- Products
-- Categories
-- Orders
-- OrderItems
-- CartItems
-- etc.
```

---

**Simpan script ini untuk referensi! 📌**
