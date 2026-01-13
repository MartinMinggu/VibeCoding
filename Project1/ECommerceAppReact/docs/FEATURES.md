# Features Documentation

## 📦 Current Features

### 1. Authentication & Authorization
- User registration dengan email confirmation
- Login/Logout functionality
- Role-based access (Admin, Seller, Customer)
- ASP.NET Core Identity integration

### 2. Product Management
- Product listing dengan kategori
- Product detail page
- Seller dapat create/edit/delete products
- Product seeding untuk demo data

### 3. Shopping Cart
- Add/remove items dari cart
- Update quantity
- Cart persistence per user

### 4. Order System
- Checkout process
- Order history
- Order details view

### 5. Content Management
- Announcement CRUD (Admin/Content Manager)
- Announcement display di homepage

### 6. Localization
- Indonesian language support
- English language support
- Language switcher

---

## 🚀 Planned Features

| Priority | Feature | Status |
|----------|---------|--------|
| High | Review & Rating | ⏳ Planned |
| High | Wishlist | ⏳ Planned |
| High | Search & Filter | ✅ Implemented |
| Medium | Payment Gateway | ⏳ Planned |
| Medium | Order Tracking | ⏳ Planned |
| Medium | Coupon System | ⏳ Planned |
| Low | Chat System | ⏳ Planned |
| Low | Analytics Dashboard | ⏳ Planned |

---

## 🏗️ Architecture

```
ECommerceApp/
├── ECommerceApp.Domain/        # Entities, Interfaces
├── ECommerceApp.Application/   # DTOs, Services, Use Cases
├── ECommerceApp.Infrastructure/# Data Access, External Services
├── ECommerceApp.Web/           # MVC Controllers, Views
└── ECommerceApp.Tests/         # Unit & Integration Tests
```
