feat: Implement Product Recommendations and Stock Availability System

## 🎯 Major Features Added

### 1. Product Recommendations System
Implemented comprehensive product recommendation features to enhance user discovery and engagement:

#### Similar Products
- Added `GetSimilarProductsAsync(int productId, int categoryId, int count)` to `IProductService` and `ProductService`
- Displays 8 random products from the same category on Product Details page
- Excludes the current product from recommendations
- Section titled "You May Also Like" with responsive grid layout
- "View All in [Category]" button for full category browsing

#### Recently Viewed Products
- Implemented client-side tracking using `localStorage` (stores last 10 viewed products)
- JavaScript function `trackRecentlyViewed(productId)` auto-executes on Product Details page
- Added API endpoint `ProductController.GetRecentlyViewedApi` for batch product retrieval by IDs
- Dynamic section on Homepage that only appears when user has browsing history
- "Clear History" button to reset tracking
- Added `GetProductsByIdsAsync(List<int> productIds)` to `ProductService`

#### Popular Products
- Utilizes existing `GetTopSellingProductsAsync` method
- Displays trending/top-selling products on Homepage (already existed, now documented)

**Files Modified:**
- `IProductService.cs` - Added method signatures
- `ProductService.cs` - Implemented recommendation logic
- `ProductController.cs` - Added API endpoints and controller actions
- `Product/Details.cshtml` - UI for similar products + tracking JS
- `Home/Index.cshtml` - UI for recently viewed products

---

### 2. Stock Availability & Inventory Management
Implemented realistic stock display system with urgency messaging and smart UI controls:

#### Stock Status Indicators
- **Color-coded badges**:
  - Green "In Stock" for items with 10+ units
  - Yellow "Low Stock" for items with 1-10 units (displays "Only X left!")
  - Red "Out of Stock" for items with 0 units
- Icons added to badges for visual clarity (✓, ⚠, ✗)
- Small badge sizing for compact display

#### Urgency & FOMO Marketing
- Alert box on Product Details page: "Hurry! Only X left in stock!" for low stock items
- Fire icon (🔥) in low stock badge to create urgency
- Grayed out "Out of Stock" message for unavailable products

#### Smart UI Controls
- Add to Cart button automatically disabled when stock = 0
- Quantity input `max` attribute set to available stock
- "Out of Stock" button replaces "Add to Cart" on listing pages
- Consistent implementation across:
  - Product Details page
  - Product listing page (static cards)
  - Infinite scroll dynamically loaded cards

**Files Modified:**
- `ProductDto.cs` - Stock property already existed, ensured mapping
- `Product/Details.cshtml` - Enhanced stock display with badges and alerts
- `Product/Index.cshtml` - Added stock badges to listing cards (static + JS template)
- `ProductController.cs` - Ensured `GetProductsApi` includes stock in JSON response

---

### 3. Product Card Layout Fixes
Fixed critical UI issues with price display and button overflow:

#### Issues Resolved
- **Price overflow**: Long price strings (e.g., Rp1.499.000,00) were pushing "View" button outside card boundaries
- **Inconsistent spacing**: Excessive white space between price and action buttons
- **Layout breaks**: Price and button on same row caused overlap on small cards

#### Solutions Implemented
- **Separate rows**: Price and buttons now on distinct rows (not side-by-side)
- **Text truncation**: Applied `text-truncate` class with `d-block` to price display
- **Overflow control**: Added `overflow: hidden` to price container
- **Compact spacing**:
  - Reduced card `min-height`: 360px → 310px (listing), 220px (homepage)
  - Tightened margins: `mb-3` → `mb-2` on descriptions
  - Removed description text from Homepage cards for cleaner look
- **Full-width buttons**: `d-grid` applied for consistent button sizing
- **Font size adjustment**: Price reduced to `1.05rem` (listing) / `1.1rem` (homepage)

#### Pages Updated
- `Home/Index.cshtml` - Trending Products section
- `Product/Index.cshtml` - Product listing grid (static + infinite scroll template)

**Before**: Price and "View" button in same flex row → overflow on long prices  
**After**: Stacked layout with price truncation → clean, no overflow

---

## 📝 Technical Details

### API Endpoints Added
```
GET /Product/GetRecentlyViewedApi?ids=1&ids=2&ids=3
  - Returns product details for given IDs array
  - Used by Recently Viewed section

GET /Product/GetProductsApi?page=1&pageSize=20...
  - Enhanced to include `stock` field in JSON response
  - Powers infinite scroll with stock data
```

### Service Layer Methods
```csharp
// IProductService.cs & ProductService.cs
Task<IEnumerable<ProductDto>> GetSimilarProductsAsync(int productId, int categoryId, int count);
Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> productIds);
```

### JavaScript Features
- LocalStorage management for Recently Viewed tracking
- Infinite scroll intersection observer (maintains stock badges)
- Auto-tracking on Product Details page load

---

## 🧪 Testing Notes

### Stock Availability
- Verified stock badges display correctly for all stock levels
- Confirmed Add to Cart disabling works when stock = 0
- Tested low stock urgency alerts on Details page

### Product Recommendations
- Similar Products: Shows 8 random items from same category
- Recently Viewed: Tested LocalStorage persistence across sessions
- API endpoints return correct product data

### UI Layout
- Tested with very long price strings (10+ digits)
- Verified responsive behavior on different screen sizes
- Confirmed infinite scroll cards match static card styling

---

## 🚀 Benefits

1. **Enhanced UX**: Product discovery through recommendations
2. **Increased Sales**: Urgency messaging (FOMO) encourages purchases
3. **Better UI**: No more broken layouts or overflow issues
4. **Realistic Ecommerce**: Stock visibility matches industry standards
5. **Performance**: LocalStorage for Recently Viewed = no extra DB queries

---

## 📋 Files Changed Summary

**Backend (C#):**
- `IProductService.cs`
- `ProductService.cs`
- `ProductController.cs`
- `ProductDto.cs`

**Frontend (Views):**
- `Views/Home/Index.cshtml`
- `Views/Product/Index.cshtml`
- `Views/Product/Details.cshtml`

**Total Lines Changed**: ~500+ (additions + modifications)

---

## ⚠️ Known Issues / Future Work

- BuyerSeeder still commented out in `Program.cs` (causes crashes)
- Recently Viewed section on Homepage uses old layout (not updated yet)
- Consider adding "Notify When Available" feature for out-of-stock items
- Lazy loading for product images could improve performance
