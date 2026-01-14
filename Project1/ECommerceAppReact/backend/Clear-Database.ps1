# PowerShell script to clear all data from Oracle database
# WARNING: This will DELETE ALL data!

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "  CLEAR DATABASE - WARNING!" -ForegroundColor Red
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "This will DELETE ALL data from your database!" -ForegroundColor Red
Write-Host ""

$confirmation = Read-Host "Type 'DELETE ALL' to confirm (case sensitive)"
if ($confirmation -ne "DELETE ALL") {
    Write-Host "Operation cancelled." -ForegroundColor Green
    exit
}

Write-Host ""
Write-Host "Clearing database..." -ForegroundColor Cyan

# Connection string
$connectionString = "User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1"

try {
    # Load Oracle DLL
    $dllPath = "$PSScriptRoot\ECommerceApp.API\bin\Debug\net8.0\Oracle.ManagedDataAccess.dll"
    if (-not (Test-Path $dllPath)) {
        Write-Host "Error: Oracle.ManagedDataAccess.dll not found at $dllPath" -ForegroundColor Red
        Write-Host "Please build the API project first: dotnet build ECommerceApp.API/ECommerceApp.API.csproj" -ForegroundColor Yellow
        exit 1
    }
    
    Add-Type -Path $dllPath
    
    # Create connection
    $connection = New-Object Oracle.ManagedDataAccess.Client.OracleConnection($connectionString)
    $connection.Open()
    Write-Host "Connected to database" -ForegroundColor Green

    # Create command
    $cmd = $connection.CreateCommand()

    # Delete data from all tables (order matters due to foreign keys)
    $deleteSql = @"
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
COMMIT;
"@

    $statements = $deleteSql -split ';' | Where-Object { $_.Trim() -ne '' }
    
    foreach ($statement in $statements) {
        $trimmed = $statement.Trim()
        if ($trimmed -eq 'COMMIT') {
            Write-Host "Committing changes..." -ForegroundColor Cyan
            continue
        }
        
        $tableName = if ($trimmed -match 'DELETE FROM "(.+?)"') { $matches[1] } else { "Unknown" }
        Write-Host "Deleting from $tableName..." -ForegroundColor Cyan
        
        $cmd.CommandText = $trimmed
        try {
            $rowsAffected = $cmd.ExecuteNonQuery()
            Write-Host "  Deleted $rowsAffected rows" -ForegroundColor Green
        } catch {
            Write-Host "  Error or table empty: $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    $connection.Close()
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "  DATABASE CLEARED!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""

} catch {
    Write-Host ""
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    if ($connection) {
        $connection.Close()
    }
    exit 1
}
