# 🚀 Git Push Guide - Branch dev-dotnet-oracle

## Langkah-langkah Push ke Git Repository

### 1️⃣ Check Git Status

Lihat file-file yang sudah berubah:

```bash
git status
```

---

### 2️⃣ Create New Branch

Buat dan pindah ke branch baru `dev-dotnet-oracle`:

```bash
git checkout -b dev-dotnet-oracle
```

Atau jika sudah ada branch-nya, pindah saja:

```bash
git checkout dev-dotnet-oracle
```

---

### 3️⃣ Stage Changes

Add semua perubahan file ke staging area:

```bash
git add .
```

**Atau add file spesifik saja:**

```bash
# Configuration files
git add ECommerceApp.Web/appsettings.json
git add ECommerceApp.Web/Program.cs
git add ECommerceApp.Web/ECommerceApp.Web.csproj

# Infrastructure files
git add ECommerceApp.Infrastructure/Data/ApplicationDbContext.cs
git add ECommerceApp.Infrastructure/ECommerceApp.Infrastructure.csproj
git add ECommerceApp.Infrastructure/Migrations/

# Documentation
git add FEATURES.md
git add README.md
```

---

### 4️⃣ Commit Changes

Commit dengan pesan yang deskriptif:

```bash
git commit -m "feat: Migrate database from SQL Server to Oracle 21c XE

- Replaced Microsoft.EntityFrameworkCore.SqlServer with Oracle.EntityFrameworkCore 8.21.121
- Updated connection string to Oracle XEPDB1 (ECOMMERCE_USER)
- Configured Program.cs to use UseOracle instead of UseSqlServer
- Fixed DateTime.Now in seed data for Oracle compatibility
- Added BOOLEAN to NUMBER(1) mapping for Oracle compatibility
- Created InitialOracleSetup migration with Oracle data types (NUMBER, NVARCHAR2, TIMESTAMP)
- Successfully applied migrations to Oracle database
- Verified application functionality with Oracle backend
- Added FEATURES.md documentation

Database: Oracle Database 21c Express Edition
Connection: localhost:1521/XEPDB1
User: ECOMMERCE_USER"
```

---

### 5️⃣ Push to Remote

Push branch baru ke remote repository:

```bash
git push -u origin dev-dotnet-oracle
```

**Penjelasan:**
- `-u` atau `--set-upstream`: Set tracking branch
- `origin`: Remote repository name
- `dev-dotnet-oracle`: Branch name

---

## 🔄 Alternative Commands

### Jika Remote Belum Ada:

Tambahkan remote repository dulu:

```bash
git remote add origin <URL_REPOSITORY_ANDA>
```

Check remote:

```bash
git remote -v
```

---

### Jika Ingin Lihat Diff:

Lihat perubahan sebelum commit:

```bash
git diff
```

Lihat perubahan yang sudah di-stage:

```bash
git diff --staged
```

---

### Jika Ingin Unstage File:

Unstage file tertentu:

```bash
git reset HEAD <filename>
```

Unstage semua:

```bash
git reset HEAD .
```

---

## 📝 Modified Files Summary

Berikut file-file yang diubah dalam migration ini:

### Configuration Changes:
- `ECommerceApp.Web/appsettings.json` - Oracle connection string
- `ECommerceApp.Web/Program.cs` - Added UseOracle configuration

### Project Files:
- `ECommerceApp.Infrastructure/ECommerceApp.Infrastructure.csproj` - Oracle packages
- `ECommerceApp.Web/ECommerceApp.Web.csproj` - Oracle packages

### Database Context:
- `ECommerceApp.Infrastructure/Data/ApplicationDbContext.cs` - BOOLEAN mapping + seed fixes

### Migrations:
- `ECommerceApp.Infrastructure/Migrations/` - Fresh Oracle migrations

### Documentation:
- `FEATURES.md` - New features documentation

---

## ✅ Verification Steps

Setelah push, verify di GitHub/GitLab:

1. **Check Branch**: Pastikan branch `dev-dotnet-oracle` muncul
2. **Check Commits**: Lihat commit history
3. **Check Files**: Verify file changes di web interface

---

## 🔧 Troubleshooting

### Error: "fatal: not a git repository"

```bash
# Initialize git repo dulu
git init
```

### Error: "error: src refspec dev-dotnet-oracle does not match any"

```bash
# Pastikan sudah ada commit
git log
```

### Error: "remote: Repository not found"

```bash
# Check remote URL
git remote -v

# Update remote URL jika salah
git remote set-url origin <CORRECT_URL>
```

### Error: "Updates were rejected"

```bash
# Pull dulu, lalu push lagi
git pull origin dev-dotnet-oracle --rebase
git push -u origin dev-dotnet-oracle
```

---

## 🎯 Quick Commands Cheat Sheet

```bash
# Status
git status

# Create & switch branch
git checkout -b dev-dotnet-oracle

# Stage all
git add .

# Commit
git commit -m "your message"

# Push
git push -u origin dev-dotnet-oracle

# Check branches
git branch -a

# Switch branch
git checkout main

# Merge branch (dari main)
git merge dev-dotnet-oracle

# Delete local branch
git branch -d dev-dotnet-oracle

# Delete remote branch
git push origin --delete dev-dotnet-oracle
```

---

## 📊 Git Flow Recommendation

Untuk development workflow yang baik:

```
main (production)
  └─ dev (development)
      └─ dev-dotnet-oracle (feature branch)
```

**Workflow:**
1. Create feature branch dari `dev`
2. Work on feature
3. Push feature branch
4. Create Pull Request ke `dev`
5. Merge ke `dev` setelah review
6. Deploy `dev` ke staging
7. Merge `dev` ke `main` untuk production

---

## 🚀 Next Steps

Setelah push berhasil:

1. ✅ Create Pull Request di GitHub/GitLab
2. ✅ Request code review dari team
3. ✅ Run CI/CD pipeline (jika ada)
4. ✅ Test di staging environment
5. ✅ Merge ke main branch

---

**Created:** January 13, 2026  
**Branch:** dev-dotnet-oracle  
**Migration:** SQL Server → Oracle 21c XE
