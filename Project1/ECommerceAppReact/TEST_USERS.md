# Test User Accounts

## 👤 Customer Accounts (10 Users)

| No | Email | Password | Name | Role |
|----|-------|----------|------|------|
| 1 | customer1@example.com | Customer123! | John Doe | Customer |
| 2 | customer2@example.com | Customer123! | Jane Smith | Customer |
| 3 | customer3@example.com | Customer123! | Michael Johnson | Customer |
| 4 | customer4@example.com | Customer123! | Emily Brown | Customer |
| 5 | customer5@example.com | Customer123! | David Wilson | Customer |
| 6 | customer6@example.com | Customer123! | Sarah Martinez | Customer |
| 7 | customer7@example.com | Customer123! | James Anderson | Customer |
| 8 | customer8@example.com | Customer123! | Lisa Taylor | Customer |
| 9 | customer9@example.com | Customer123! | Robert Garcia | Customer |
| 10 | customer10@example.com | Customer123! | Jennifer Lee | Customer |

## 🏪 Seller Accounts (10 Users)

| No | Email | Password | Name | Store Name | Role |
|----|-------|----------|------|------------|------|
| 1 | seller1@example.com | Seller123! | Tech Store Owner | TechHub Electronics | Seller |
| 2 | seller2@example.com | Seller123! | Fashion Boutique | StyleVibe Fashion | Seller |
| 3 | seller3@example.com | Seller123! | Sports Gear Pro | ActiveLife Sports | Seller |
| 4 | seller4@example.com | Seller123! | Home Essentials | CozyHome Living | Seller |
| 5 | seller5@example.com | Seller123! | Gadget Master | Digital Haven | Seller |
| 6 | seller6@example.com | Seller123! | Sneaker King | SoleStrike Sneakers | Seller |
| 7 | seller7@example.com | Seller123! | Book Lover | PageTurner Books | Seller |
| 8 | seller8@example.com | Seller123! | Beauty Expert | GlowUp Cosmetics | Seller |
| 9 | seller9@example.com | Seller123! | Gaming Zone | PixelPlay Games | Seller |
| 10 | seller10@example.com | Seller123! | Organic Foods | FreshHarvest Market | Seller |

## 🔑 Quick Test Credentials

### For Quick Login Testing:
- **Customer**: `customer1@example.com` / `Customer123!`
- **Seller**: `seller1@example.com` / `Seller123!`

## 📝 Usage Instructions

### Testing Login:
1. Navigate to http://localhost:3000/login
2. Use any email/password combination from tables above
3. Click "Login" button

### Testing Registration:
1. Navigate to http://localhost:3000/register
2. Fill in the form with new details
3. Check "Register as Seller" for seller account
4. Click "Create Account"

### API Testing with Postman/Thunder Client:

**Login Endpoint:**
```http
POST http://localhost:5211/api/auth/login
Content-Type: application/json

{
  "email": "customer1@example.com",
  "password": "Customer123!"
}
```

**Register Endpoint:**
```http
POST http://localhost:5211/api/auth/register
Content-Type: application/json

{
  "email": "newuser@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "firstName": "New",
  "lastName": "User",
  "registerAsSeller": false
}
```

## 🎯 Account Features by Role

### Customer Can:
- ✅ Browse products
- ✅ Add items to cart
- ✅ Place orders
- ✅ View order history
- ✅ Update profile
- ✅ Leave reviews

### Seller Can:
- ✅ All customer features PLUS:
- ✅ Create new products
- ✅ Manage product inventory
- ✅ View sales analytics
- ✅ Update product prices
- ✅ Upload product images
- ✅ Manage orders for their products

## 💡 Notes

- All passwords follow the same pattern for testing convenience
- In production, enforce stronger password requirements
- Seller accounts have additional permissions
- Customer accounts can browse and purchase
- Use these credentials to test authentication flow
