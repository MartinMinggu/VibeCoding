# 📱 ECommerceApp - Dokumentasi Fitur

> **Platform E-Commerce Modern**  
> Database: Oracle Database 21c Express Edition  
> Framework: ASP.NET Core 8.0 MVC

---

## 🎯 Overview

ECommerceApp adalah platform e-commerce full-featured yang memungkinkan pengguna untuk berbelanja online, seller untuk mengelola produk mereka, dan admin untuk mengelola seluruh sistem.

---

## 👥 User Roles & Authentication

### 1. **Authentication System**
- ✅ **Register**: Buat akun baru dengan email dan password
- ✅ **Login**: Masuk dengan email/password
- ✅ **Logout**: Keluar dari sistem
- ✅ **Password Reset**: Reset password jika lupa
- ✅ **Email Confirmation**: Verifikasi email (optional)

### 2. **User Roles**

#### 🔴 **Admin**
- Full access ke seluruh sistem
- Manage users dan roles
- View semua transactions
- Manage system configurations

#### 🟠 **Content Manager**
- Manage announcements (create, edit, delete)
- Publish promotional content
- Manage site-wide notifications
- Schedule events

#### 🟢 **Seller**
- Create dan manage produk sendiri
- Upload product images (gallery)
- Set prices dan stock
- View orders untuk produk mereka
- Chat dengan buyers
- Dashboard penjualan

#### 🔵 **Customer** (Default)
- Browse products
- Search dan filter products
- Add products ke cart
- Checkout dan place orders
- View order history
- Chat dengan sellers
- Manage profile

---

## 🛒 Shopping Features

### 1. **Product Catalog**

#### Browse Products
- **Product Listing**: View semua products dengan pagination
- **Category Filter**: Filter by kategori (Electronics, Clothing, Books, Home & Garden, Sports)
- **Search**: Cari products by name atau description
- **Sort**: Sort by price, name, newest, popularity

#### Product Details
- Product name dan description
- Price display
- Stock availability
- Seller information
- Product image gallery (multiple images)
- Category badge
- Add to cart button

### 2. **Shopping Cart**

- ➕ **Add to Cart**: Tambah product dengan quantity selector
- ➖ **Remove from Cart**: Hapus item dari cart
- 🔢 **Update Quantity**: Ubah jumlah item
- 💰 **Price Calculation**: Auto-calculate subtotal
- 🧮 **Tax & Shipping**: Calculate total dengan tax dan shipping fee
- 🆓 **Free Shipping**: Otomatis jika order > threshold (configurable)
- 💾 **Persistent Cart**: Cart tersimpan per user

### 3. **Checkout & Orders**

#### Checkout Process
1. Review cart items
2. Enter shipping address
3. Select payment method
4. Confirm order
5. Order placed!

#### Order Management
- **Order History**: View semua orders yang pernah dibuat
- **Order Status Tracking**:
  - 📦 Pending - Order baru dibuat
  - 🔄 Processing - Seller sedang process
  - 🚚 Shipped - Order sudah dikirim
  - ✅ Delivered - Order sudah sampai
  - ❌ Cancelled - Order dibatalkan

- **Order Details**: View items, total, shipping address, payment method
- **Order Filtering**: Filter by status

---

## 🏪 Seller Features

### 1. **Product Management**

#### Create Product
- Product name (max 200 characters)
- Description (detailed)
- Price (decimal, 2 decimal places)
- Stock quantity
- Category selection
- Main product image
- Gallery images (multiple upload)
- Active/Inactive status

#### Edit Product
- Update any product information
- Add/remove gallery images
- Adjust stock levels
- Change pricing

#### Delete Product
- Soft delete (mark as inactive)
- Hard delete (remove from database)

### 2. **Inventory Management**

- 📊 **Stock Tracking**: Real-time stock updates
- ⚠️ **Low Stock Alerts**: Visual indicators
- 📈 **Sales Analytics**: View product performance
- 🔄 **Bulk Updates**: Update multiple products

### 3. **Seller Dashboard**

- Total products
- Recent orders
- Revenue statistics
- Popular products
- Pending shipments

---

## 📢 Announcement System

### Features
- **Create Announcements**: Post news, promo, events
- **Announcement Types**:
  - 🎉 Promo - Sales dan discounts
  - 📰 News - General news
  - 📅 Event - Upcoming events

- **Scheduling**: Set start date dan end date
- **Image Upload**: Featured image untuk announcement
- **Active/Inactive**: Control visibility
- **Homepage Display**: Featured announcements di homepage

### Management (Content Manager)
- Create new announcements
- Edit existing announcements
- Delete announcements
- Schedule future announcements
- View analytics (views, clicks)

---

## 💬 Chat/Messaging System

### Features
- **Real-time Chat**: Komunikasi langsung buyer ↔ seller
- **Conversations**: Organized by product
- **Message History**: Persistent conversation storage
- **Read Status**: Track unread messages
- **Notifications**: Alert untuk new messages

### Use Cases
- Tanya product details
- Negosiasi harga
- Custom orders
- After-sales support

---

## 🖼️ Product Gallery

### Features
- **Multiple Images**: Upload multiple images per product
- **Primary Image**: Set main product image
- **Display Order**: Arrange image sequence
- **Gallery View**: Carousel/grid display
- **Zoom Feature**: Enlarge images
- **Image Management**: Add/remove images anytime

---

## ⚙️ System Configuration (Lookups)

### Configurable Settings
Accessible via database atau admin panel:

| Setting | Default | Category |
|---------|---------|----------|
| **Site Name** | "ECommerce Store" | General |
| **Admin Email** | admin@ecommerce.com | General |
| **Currency** | USD | Payment |
| **Tax Rate** | 10% | Payment |
| **Shipping Fee** | $5.00 | Shipping |
| **Free Shipping Threshold** | $50.00 | Shipping |
| **Max Cart Items** | 50 | Cart |
| **Products Per Page** | 12 | Display |

---

## 🔍 Search & Filter

### Search Features
- **Keyword Search**: Search by product name/description
- **Category Filter**: Filter by specific category
- **Price Range**: Min/max price filter
- **Seller Filter**: Filter by specific seller
- **Availability**: In stock / All products

### Sorting Options
- Price: Low to High
- Price: High to Low
- Name: A-Z
- Name: Z-A
- Newest First
- Best Selling

---

## 👤 User Profile Management

### Profile Features
- **Personal Information**:
  - First Name
  - Last Name
  - Email Address
  - Phone Number
  - Default Shipping Address

- **Account Settings**:
  - Change Password
  - Update Email
  - Profile Picture (optional)
  - Become a Seller (role upgrade request)

- **Activity**:
  - Order History
  - Wishlist (if implemented)
  - Reviews (if implemented)
  - Messages

---

## 🌐 Multi-Language Support

### Localization
- 🇮🇩 **Bahasa Indonesia** (Default)
- 🇺🇸 **English**
- Language switcher di navbar
- Persisted language preference

---

## 📊 Categories

### Pre-defined Categories
1. **📱 Electronics**
   - Electronic devices and accessories
   
2. **👔 Clothing**
   - Men and women clothing
   
3. **📚 Books**
   - Books and magazines
   
4. **🏡 Home & Garden**
   - Home and garden products
   
5. **⚽ Sports**
   - Sports equipment and accessories

---

## 🔐 Security Features

### Implementation
- ✅ **Password Hashing**: Secure password storage (ASP.NET Identity)
- ✅ **HTTPS**: Encrypted connections
- ✅ **CSRF Protection**: Anti-forgery tokens
- ✅ **Input Validation**: Server-side validation
- ✅ **SQL Injection Prevention**: Parameterized queries (EF Core)
- ✅ **Role-Based Authorization**: Access control by role
- ✅ **Session Management**: Secure session handling

---

## 📱 Responsive Design

### Device Support
- 💻 **Desktop**: Full features
- 📱 **Mobile**: Mobile-optimized
- 📱 **Tablet**: Tablet-friendly layouts
- 🖥️ **Large Screens**: Optimized for 4K displays

---

## 🚀 Performance Features

### Optimization
- ⚡ **Database Indexing**: Optimized queries
- 🗄️ **Connection Pooling**: Efficient database connections
- 📦 **Lazy Loading**: Load data on demand
- 🎯 **Pagination**: Efficient large dataset handling
- 💾 **Caching**: Session caching (if implemented)

---

## 📈 Reporting & Analytics (Future)

### Planned Features
- 📊 Sales reports
- 📉 Revenue charts
- 👥 User analytics
- 🏆 Top products
- 📅 Period comparisons

---

## 🔄 Workflow Examples

### Customer Journey
1. Register/Login
2. Browse products → Filter by category
3. View product details
4. Add to cart
5. Update quantities
6. Proceed to checkout
7. Enter shipping address
8. Select payment method
9. Place order
10. Track order status

### Seller Journey
1. Register as seller
2. Access seller dashboard
3. Create new product
4. Upload images
5. Set pricing & stock
6. Publish product
7. Receive orders
8. Update order status
9. Chat with buyers
10. View sales analytics

### Admin Journey
1. Login as admin
2. View dashboard
3. Manage users & roles
4. Monitor transactions
5. Configure system settings
6. Moderate content
7. Generate reports

---

## 🛠️ Technical Stack

### Backend
- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: Oracle Database 21c XE
- **ORM**: Entity Framework Core 8.0
- **Authentication**: ASP.NET Identity
- **Real-time**: SignalR (for chat)

### Frontend
- **UI Framework**: Razor Pages
- **CSS**: Custom CSS with responsive design
- **JavaScript**: Vanilla JS + jQuery (minimal)
- **Icons**: Font Awesome (if used)

### Database
- **DBMS**: Oracle Database 21c Express Edition
- **Connection**: Oracle.EntityFrameworkCore 8.21.121
- **Host**: localhost:1521/XEPDB1
- **User**: ECOMMERCE_USER

---

## 📞 Support & Help

### Contact Information
- **Admin Email**: admin@ecommerce.com
- **Support**: Via in-app chat
- **FAQ**: Help section (if implemented)

---

## 🔮 Future Enhancements

### Planned Features
- [ ] Payment Gateway Integration (Midtrans, Stripe)
- [ ] Product Reviews & Ratings
- [ ] Wishlist Functionality
- [ ] Email Notifications (order confirmations, shipping updates)
- [ ] SMS Notifications
- [ ] Advanced Analytics Dashboard
- [ ] Coupon & Discount System
- [ ] Product Recommendations (AI-based)
- [ ] Social Media Integration
- [ ] Mobile App (React Native)
- [ ] API for Third-party Integration
- [ ] Multi-vendor Marketplace Features
- [ ] Auction System
- [ ] Subscription Products
- [ ] Gift Cards

---

## 📝 Notes

### Database Schema
- **14 Tables**: Users, Roles, Products, Orders, Cart, Categories, etc.
- **100+ Products**: Pre-seeded with sample data
- **5 Sellers**: Demo seller accounts
- **Sample Data**: Categories, Lookups, Announcements

### Default Credentials
```
Content Manager:
Email: contentmanager@ecommerce.com
Password: Content123!

Demo Seller:
Email: seller@ecommerce.com
Password: Seller123!
```

---

## 🎉 Summary

ECommerceApp adalah **full-featured e-commerce platform** dengan:
- ✅ Complete user authentication & authorization
- ✅ Multi-role support (Admin, Content Manager, Seller, Customer)
- ✅ Product management dengan gallery
- ✅ Shopping cart & checkout
- ✅ Order management & tracking
- ✅ Real-time chat system
- ✅ Announcement system
- ✅ Multi-language support
- ✅ Responsive design
- ✅ Oracle Database backend

**Perfect untuk:** 
- Learning e-commerce development
- Building marketplace platforms
- Portfolio projects
- Small to medium business online stores

---

**Last Updated:** January 13, 2026  
**Version:** 1.0  
**Database:** Oracle 21c XE  
**Framework:** ASP.NET Core 8.0
