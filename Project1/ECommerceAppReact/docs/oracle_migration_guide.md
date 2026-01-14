# Database Migration Guide: SQL Server to Oracle

## 📊 Data Type Mapping Reference

### Common SQL Server to Oracle Mappings

| SQL Server Type | Oracle Type | EF Core Mapping | Notes |
|----------------|-------------|-----------------|-------|
| `int` | `NUMBER(10)` | `.HasColumnType("NUMBER(10)")` | 32-bit integer |
| `bigint` | `NUMBER(19)` | `.HasColumnType("NUMBER(19)")` | 64-bit integer |
| `smallint` | `NUMBER(5)` | `.HasColumnType("NUMBER(5)")` | 16-bit integer |
| `tinyint` | `NUMBER(3)` | `.HasColumnType("NUMBER(3)")` | 8-bit integer |
| `bit` | `NUMBER(1)` | `.HasColumnType("NUMBER(1)")` | Boolean (0/1) |
| `decimal(p,s)` | `NUMBER(p,s)` | `.HasPrecision(p,s)` | Fixed precision |
| `money` | `NUMBER(19,4)` | `.HasPrecision(19,4)` | Currency |
| `float` | `BINARY_DOUBLE` | `.HasColumnType("BINARY_DOUBLE")` | Double precision |
| `real` | `BINARY_FLOAT` | `.HasColumnType("BINARY_FLOAT")` | Single precision |
| `varchar(n)` | `VARCHAR2(n)` | `.HasMaxLength(n)` | Variable char |
| `nvarchar(n)` | `NVARCHAR2(n)` | `.HasMaxLength(n)` | Unicode |
| `varchar(MAX)` | `CLOB` | `.HasColumnType("CLOB")` | Large text |
| `nvarchar(MAX)` | `NCLOB` | `.HasColumnType("NCLOB")` | Large unicode |
| `char(n)` | `CHAR(n)` | `.HasColumnType("CHAR(n)")` | Fixed char |
| `text` | `CLOB` | `.HasColumnType("CLOB")` | Text |
| `ntext` | `NCLOB` | `.HasColumnType("NCLOB")` | Unicode text |
| `datetime` | `TIMESTAMP` | `.HasColumnType("TIMESTAMP")` | Date+Time |
| `datetime2` | `TIMESTAMP(7)` | `.HasColumnType("TIMESTAMP(7)")` | High precision |
| `date` | `DATE` | `.HasColumnType("DATE")` | Date only |
| `time` | `TIMESTAMP` | `.HasColumnType("TIMESTAMP")` | Time storage |
| `datetimeoffset` | `TIMESTAMP WITH TIME ZONE` | `.HasColumnType("TIMESTAMP WITH TIME ZONE")` | With timezone |
| `uniqueidentifier` | `RAW(16)` | `.HasColumnType("RAW(16)")` | GUID as binary |
| `uniqueidentifier` | `VARCHAR2(36)` | `.HasMaxLength(36)` | GUID as string |
| `binary(n)` | `RAW(n)` | `.HasColumnType("RAW(n)")` | Fixed binary |
| `varbinary(n)` | `RAW(n)` | `.HasColumnType("RAW(n)")` | Variable binary |
| `varbinary(MAX)` | `BLOB` | `.HasColumnType("BLOB")` | Large binary |
| `image` | `BLOB` | `.HasColumnType("BLOB")` | Image data |

## 🔄 ECommerceApp Entity Mappings

### Based on ECommerceApp Domain Models

#### Product Entity
- `Id` (int) → `NUMBER(10)`
- `Name` (string) → `NVARCHAR2(200)`
- `Description` (string) → `NCLOB` or `NVARCHAR2(4000)`
- `Price` (decimal) → `NUMBER(18,2)`
- `Stock` (int) → `NUMBER(10)`
- `ImageUrl` (string) → `NVARCHAR2(500)`
- `CategoryId` (int) → `NUMBER(10)`
- `SellerId` (string) → `NVARCHAR2(450)` (Identity GUID)
- `CreatedAt` (DateTime) → `TIMESTAMP`
- `UpdatedAt` (DateTime?) → `TIMESTAMP`

#### ApplicationUser (Identity)
- `Id` (string/GUID) → `NVARCHAR2(450)`
- `UserName` (string) → `NVARCHAR2(256)`
- `Email` (string) → `NVARCHAR2(256)`
- `PasswordHash` (string) → `NVARCHAR2(4000)`
- Standard Identity fields → Keep Oracle-compatible types

#### Order Entity
- `Id` (int) → `NUMBER(10)`
- `UserId` (string) → `NVARCHAR2(450)`
- `TotalAmount` (decimal) → `NUMBER(18,2)`
- `OrderDate` (DateTime) → `TIMESTAMP`
- `Status` (string/enum) → `NVARCHAR2(50)`

#### OrderItem Entity
- `Id` (int) → `NUMBER(10)`
- `OrderId` (int) → `NUMBER(10)`
- `ProductId` (int) → `NUMBER(10)`
- `Quantity` (int) → `NUMBER(10)`
- `UnitPrice` (decimal) → `NUMBER(18,2)`

## 🛠️ Migration Steps

### Step 1: Create Oracle User for Application

Buka SQL*Plus sebagai SYSDBA:

```sql
-- Connect to Oracle
sqlplus sys as sysdba
-- Enter password

-- Switch to Pluggable Database (PDB)
ALTER SESSION SET CONTAINER = XEPDB1;

-- Create application user
CREATE USER ECOMMERCE_USER IDENTIFIED BY "YourStrongPassword123!";

-- Grant necessary privileges
GRANT CONNECT, RESOURCE TO ECOMMERCE_USER;
GRANT CREATE VIEW, CREATE SYNONYM TO ECOMMERCE_USER;
GRANT UNLIMITED TABLESPACE TO ECOMMERCE_USER;

-- For Entity Framework migrations
GRANT CREATE TABLE, CREATE SEQUENCE, CREATE TRIGGER TO ECOMMERCE_USER;
GRANT ALTER ANY TABLE, DROP ANY TABLE TO ECOMMERCE_USER;

-- Exit
EXIT;
```

### Step 2: Update NuGet Packages

Di terminal PowerShell project Anda:

```powershell
# Navigate to Infrastructure project
cd ECommerceApp.Infrastructure

# Remove SQL Server package
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer

# Add Oracle package
dotnet add package Oracle.EntityFrameworkCore --version 8.23.50
dotnet add package Oracle.ManagedDataAccess.Core --version 23.5.1

# Return to root
cd ..
```

### Step 3: Update Connection String

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=ECOMMERCE_USER;Password=YourStrongPassword123!;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)))"
  }
}
```

**Connection String Sederhana:**
```
User Id=ECOMMERCE_USER;Password=YourStrongPassword123!;Data Source=localhost:1521/XEPDB1
```

### Step 4: Update Program.cs

Ganti `UseSqlServer` dengan `UseOracle`:

```csharp
// File: ECommerceApp.Web/Program.cs

// Add using statement
using Oracle.EntityFrameworkCore;

// Update DbContext registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString, 
        oracleOptions => {
            oracleOptions.UseOracleSQLCompatibility("11");
        }));
```

### Step 5: Update ApplicationDbContext (if needed)

Beberapa konfigurasi mungkin perlu penyesuaian:

```csharp
// In OnModelCreating method

// For DateTime seeding - Oracle tidak support DateTime.Now di seed
// Ganti:
CreatedAt = DateTime.Now
// Dengan specific date:
CreatedAt = new DateTime(2024, 1, 1)

// For string lengths - Oracle max VARCHAR2 adalah 4000
builder.Entity<Product>()
    .Property(p => p.Description)
    .HasMaxLength(4000); // atau gunakan CLOB untuk text panjang

// For Identity keys - pastikan compatible
builder.Entity<ApplicationUser>()
    .Property(u => u.Id)
    .HasMaxLength(450);
```

### Step 6: Create Initial Migration for Oracle

```powershell
# Hapus migrations lama (backup dulu jika perlu)
Remove-Item -Recurse -Force ECommerceApp.Infrastructure\Migrations

# Create new migration untuk Oracle
dotnet ef migrations add InitialOracleSetup --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web

# Apply migration ke Oracle database
dotnet ef database update --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web
```

## ⚠️ Common Migration Issues & Solutions

### Issue 1: DateTime.Now in Seed Data
**Error**: Oracle doesn't support DateTime.Now in migrations

**Solution**: Replace dengan fixed dates
```csharp
// Before
CreatedAt = DateTime.Now

// After
CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0)
```

### Issue 2: String Length Exceeds 4000
**Error**: VARCHAR2 maximum is 4000 bytes

**Solution**: Use CLOB for large text
```csharp
builder.Entity<Product>()
    .Property(p => p.Description)
    .HasColumnType("NCLOB");
```

### Issue 3: Index Name Too Long
**Error**: Oracle identifier max is 30 chars (older versions) or 128 chars (12c+)

**Solution**: Specify shorter index names
```csharp
builder.Entity<Product>()
    .HasIndex(p => p.Name)
    .HasDatabaseName("IX_Prod_Name");
```

### Issue 4: Sequence Issues with Identity
**Solution**: Oracle EF Core handles this automatically, but you can customize:
```csharp
builder.Entity<Product>()
    .Property(p => p.Id)
    .UseOracleIdentityColumn();
```

## 📋 Verification Checklist

After migration, verify:

- [ ] All tables created in Oracle
- [ ] All indexes and constraints present
- [ ] Seed data inserted correctly
- [ ] Foreign keys working
- [ ] Application can connect to Oracle
- [ ] CRUD operations work
- [ ] User authentication works
- [ ] All pages load correctly

## 🔍 Useful Oracle Queries for Verification

```sql
-- Connect as application user
sqlplus ECOMMERCE_USER/YourStrongPassword123!@localhost:1521/XEPDB1

-- List all tables
SELECT table_name FROM user_tables ORDER BY table_name;

-- Count rows in each table
SELECT 'Products' as table_name, COUNT(*) as row_count FROM Products
UNION ALL
SELECT 'Categories', COUNT(*) FROM Categories
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'AspNetUsers', COUNT(*) FROM "AspNetUsers";

-- Check constraints
SELECT constraint_name, constraint_type, table_name 
FROM user_constraints 
ORDER BY table_name;

-- Check indexes
SELECT index_name, table_name, uniqueness 
FROM user_indexes
ORDER BY table_name;
```

## 🚀 Next Steps After Migration

1. **Test all features** thoroughly
2. **Backup Oracle database** regularly
3. **Monitor performance** - Oracle may need different indexing strategies
4. **Consider Oracle-specific optimizations**
5. **Update documentation** with new connection info

---

**Pro Tips:**
- Oracle is case-sensitive with quoted identifiers
- Oracle uses different SQL syntax for some operations (e.g., `ROWNUM` instead of `TOP`)
- Consider using Oracle SQL Developer for database management
- Keep both databases running during transition for comparison
