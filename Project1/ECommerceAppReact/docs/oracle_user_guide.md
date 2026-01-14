# Create Oracle Database User - Panduan Lengkap

## 🎯 Tujuan
Membuat user database khusus untuk aplikasi **ECommerceApp** dengan nama `ECOMMERCE_USER`.

> [!IMPORTANT]
> **Jangan gunakan user SYS atau SYSTEM untuk aplikasi!**
> Best practice adalah membuat user khusus untuk setiap aplikasi.

---

## 📋 Cara 1: Menggunakan SQL*Plus (Recommended - Cepat)

### Step 1: Buka SQL*Plus sebagai SYSDBA

```powershell
# Di PowerShell atau Command Prompt
sqlplus sys as sysdba
```

Masukkan password SYS yang Anda set saat instalasi.

### Step 2: Copy-Paste Script Berikut

```sql
-- Switch ke Pluggable Database
ALTER SESSION SET CONTAINER = XEPDB1;

-- Create user (ganti password jika mau!)
CREATE USER ECOMMERCE_USER IDENTIFIED BY "Oracle2024!";

-- Grant privileges dasar
GRANT CONNECT, RESOURCE TO ECOMMERCE_USER;

-- Grant privileges untuk database objects
GRANT CREATE VIEW, CREATE SYNONYM TO ECOMMERCE_USER;
GRANT CREATE SEQUENCE, CREATE TABLE, CREATE TRIGGER TO ECOMMERCE_USER;

-- Grant privileges untuk EF migrations
GRANT ALTER ANY TABLE, DROP ANY TABLE TO ECOMMERCE_USER;

-- Grant unlimited tablespace
GRANT UNLIMITED TABLESPACE TO ECOMMERCE_USER;
```

### Step 3: Verify User Created

```sql
-- Check user exists
SELECT username, account_status 
FROM dba_users 
WHERE username = 'ECOMMERCE_USER';
```

**Expected output:**
```
USERNAME          ACCOUNT_STATUS
----------------- ---------------
ECOMMERCE_USER    OPEN
```

### Step 4: Test Connection

```sql
-- Exit dari session SYSDBA
exit;
```

Kemudian test login sebagai user baru:

```powershell
sqlplus ECOMMERCE_USER/Oracle2024!@localhost:1521/XEPDB1
```

Jika berhasil, Anda akan melihat:
```
Connected to:
Oracle Database 21c Express Edition Release 21.0.0.0.0
```

Test query:
```sql
-- Show current user
SHOW USER;
-- Should show: USER is "ECOMMERCE_USER"

-- Test create table
CREATE TABLE test_table (id NUMBER);
DROP TABLE test_table;

-- Exit
exit;
```

✅ **Selesai!** User sudah siap digunakan.

---

## 📋 Cara 2: Menggunakan Enterprise Manager (GUI)

Anda sudah login ke Enterprise Manager di browser. Berikut langkah-langkahnya:

### Step 1: Navigate ke Users Management

1. Di Oracle EM, klik menu **"Configuration"** atau **"Administration"**
2. Cari section **"Security"**
3. Klik **"Users"**

### Step 2: Create New User

1. Klik tombol **"Create User"** atau **"+"**
2. Isi form:
   - **Name**: `ECOMMERCE_USER`
   - **Profile**: `DEFAULT`
   - **Authentication**: Password
   - **Password**: `Oracle2024!` (atau password pilihan Anda)
   - **Confirm Password**: (ulangi password)
   - **Default Tablespace**: `USERS`
   - **Temporary Tablespace**: `TEMP`
   - **Status**: `Unlocked`

### Step 3: Grant Privileges

Di tab **"Roles"**, centang:
- ✅ `CONNECT`
- ✅ `RESOURCE`

Di tab **"System Privileges"**, tambahkan:
- ✅ `CREATE VIEW`
- ✅ `CREATE SYNONYM`
- ✅ `CREATE SEQUENCE`
- ✅ `CREATE TABLE`
- ✅ `CREATE TRIGGER`
- ✅ `ALTER ANY TABLE`
- ✅ `DROP ANY TABLE`

Di tab **"Quotas"**, set:
- Tablespace: `USERS`
- Quota: `UNLIMITED`

### Step 4: Save

Klik **"Create"** atau **"OK"**

---

## ✅ Verification Checklist

Setelah create user, pastikan:

- [ ] User `ECOMMERCE_USER` muncul di daftar users
- [ ] Account status = `OPEN` (not locked)
- [ ] Bisa login dengan `sqlplus ECOMMERCE_USER/password@localhost:1521/XEPDB1`
- [ ] Bisa create dan drop table (test privileges)

---

## 🔐 Informasi User yang Dibuat

| Property | Value |
|----------|-------|
| **Username** | `ECOMMERCE_USER` |
| **Password** | `Oracle2024!` (atau yang Anda set) |
| **Database** | `XEPDB1` (Pluggable Database) |
| **Host** | `localhost` |
| **Port** | `1521` |
| **Service Name** | `XEPDB1` |

### Connection String untuk .NET

```
User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1
```

### Connection String (Alternative Format)

```
User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)))
```

---

## ⚠️ Troubleshooting

### Error: "user already exists"
```sql
-- Drop existing user first (HATI-HATI!)
DROP USER ECOMMERCE_USER CASCADE;
-- Then create again
```

### Error: "insufficient privileges"
- Pastikan Anda login sebagai `sys as sysdba`
- Bukan sebagai `system` atau user biasa

### Error: "container not found"
```sql
-- Check available containers
SELECT name, open_mode FROM v$pdbs;
-- Use the correct PDB name (should be XEPDB1)
```

### User tidak bisa login
```sql
-- Check account status
SELECT username, account_status FROM dba_users WHERE username = 'ECOMMERCE_USER';

-- If locked, unlock it:
ALTER USER ECOMMERCE_USER ACCOUNT UNLOCK;
```

### Reset password
```sql
-- As SYSDBA
ALTER USER ECOMMERCE_USER IDENTIFIED BY "NewPassword123!";
```

---

## 🚀 Next Steps

Setelah user berhasil dibuat:

1. ✅ **Update appsettings.json** dengan connection string baru
2. ✅ **Install Oracle NuGet packages** di project
3. ✅ **Update Program.cs** untuk gunakan Oracle
4. ✅ **Create dan run migrations**

Mari lanjut ke langkah berikutnya! 🎉
