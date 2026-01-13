-- ============================================
-- Create Oracle User for ECommerceApp
-- ============================================
-- Run this script as SYSDBA
-- Command: sqlplus sys as sysdba

-- Switch to Pluggable Database (required for Oracle XE 21c)
ALTER SESSION SET CONTAINER = XEPDB1;

-- Create user with strong password
-- IMPORTANT: Ganti password sesuai keinginan Anda!
CREATE USER ECOMMERCE_USER IDENTIFIED BY "Oracle2024!";

-- Grant basic connection and resource privileges
GRANT CONNECT TO ECOMMERCE_USER;
GRANT RESOURCE TO ECOMMERCE_USER;

-- Grant privileges for creating database objects
GRANT CREATE VIEW TO ECOMMERCE_USER;
GRANT CREATE SYNONYM TO ECOMMERCE_USER;
GRANT CREATE SEQUENCE TO ECOMMERCE_USER;
GRANT CREATE TABLE TO ECOMMERCE_USER;
GRANT CREATE TRIGGER TO ECOMMERCE_USER;

-- Grant privileges for modifying database objects (needed for EF migrations)
GRANT ALTER ANY TABLE TO ECOMMERCE_USER;
GRANT DROP ANY TABLE TO ECOMMERCE_USER;

-- Grant unlimited tablespace (or specify quota)
GRANT UNLIMITED TABLESPACE TO ECOMMERCE_USER;

-- Verify user was created successfully
SELECT username, account_status, default_tablespace 
FROM dba_users 
WHERE username = 'ECOMMERCE_USER';

-- Show granted privileges
SELECT * FROM dba_sys_privs 
WHERE grantee = 'ECOMMERCE_USER'
ORDER BY privilege;

-- Success message
SELECT 'User ECOMMERCE_USER created successfully!' AS status FROM dual;

-- ============================================
-- Connection String untuk appsettings.json:
-- ============================================
-- "User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1"
-- ============================================
