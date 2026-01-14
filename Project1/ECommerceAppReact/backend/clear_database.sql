-- =====================================================
-- Clear All Data from ECommerceApp Database
-- WARNING: This will DELETE ALL data from all tables!
-- =====================================================

-- Disable foreign key constraints temporarily
BEGIN
   FOR c IN (SELECT table_name, constraint_name 
             FROM user_constraints 
             WHERE constraint_type = 'R') 
   LOOP
      EXECUTE IMMEDIATE 'ALTER TABLE ' || c.table_name || ' DISABLE CONSTRAINT ' || c.constraint_name;
   END LOOP;
END;
/

-- Delete data from all tables (in reverse dependency order)
DELETE FROM "ProductGalleryImages";
DELETE FROM "OrderItems";
DELETE FROM "Orders";
DELETE FROM "CartItems";
DELETE FROM "Carts";
DELETE FROM "Products";
DELETE FROM "Categories";
DELETE FROM "AspNetUserRoles";
DELETE FROM "AspNetUserClaims";
DELETE FROM "AspNetUserLogins";
DELETE FROM "AspNetUserTokens";
DELETE FROM "AspNetUsers";
DELETE FROM "AspNetRoleClaims";
DELETE FROM "AspNetRoles";

-- Re-enable foreign key constraints
BEGIN
   FOR c IN (SELECT table_name, constraint_name 
             FROM user_constraints 
             WHERE constraint_type = 'R') 
   LOOP
      EXECUTE IMMEDIATE 'ALTER TABLE ' || c.table_name || ' ENABLE CONSTRAINT ' || c.constraint_name;
   END LOOP;
END;
/

-- Reset sequences (if you have any)
-- Example: ALTER SEQUENCE product_id_seq RESTART START WITH 1;

COMMIT;

SELECT 'Database cleared successfully!' AS status FROM DUAL;
