# Simple script to clear database
Write-Host "Clearing database..." -ForegroundColor Cyan

$connectionString = "User Id=ECOMMERCE_USER;Password=Oracle2024!;Data Source=localhost:1521/XEPDB1"

try {
    $dllPath = Join-Path $PSScriptRoot "ECommerceApp.API\bin\Debug\net8.0\Oracle.ManagedDataAccess.dll"
    
    if (-not (Test-Path $dllPath)) {
        Write-Host "Building API project..." -ForegroundColor Yellow
        Set-Location $PSScriptRoot
        dotnet build "ECommerceApp.API\ECommerceApp.API.csproj" --verbosity quiet
    }
    
    Add-Type -Path $dllPath
    
    $connection = New-Object Oracle.ManagedDataAccess.Client.OracleConnection($connectionString)
    $connection.Open()
    Write-Host "Connected to database" -ForegroundColor Green

    $cmd = $connection.CreateCommand()

    $tables = @(
        'ProductGalleryImages',
        'OrderItems', 
        'Orders',
        'CartItems',
        'Carts',
        'Products',
        'Categories',
        'AspNetUserRoles',
        'AspNetUserClaims',
        'AspNetUserLogins',
        'AspNetUserTokens',
        'AspNetUsers',
        'AspNetRoleClaims',
        'AspNetRoles'
    )

    $totalDeleted = 0
    
    foreach ($table in $tables) {
        $quotedTable = '"' + $table + '"'
        $cmd.CommandText = "DELETE FROM $quotedTable"
        try {
            $rows = $cmd.ExecuteNonQuery()
            Write-Host "$table : $rows rows deleted" -ForegroundColor Green
            $totalDeleted += $rows
        } catch {
            Write-Host "$table : $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    $connection.Close()
    
    Write-Host ""
    Write-Host "DATABASE CLEARED!" -ForegroundColor Green  
    Write-Host "Total rows deleted: $totalDeleted" -ForegroundColor Cyan

} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    if ($connection) { $connection.Close() }
    exit 1
}
