using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerceApp.Tests;

/// <summary>
/// Unit tests for ProductService - specifically testing the Search & Filter functionality
/// Following AAA pattern (Arrange, Act, Assert) and best practices
/// </summary>
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly ProductService _productService;
    private readonly List<Product> _testProducts;

    public ProductServiceTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _productService = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object);
        
        // Setup test data
        _testProducts = CreateTestProducts();
    }

    #region Test Data Setup
    
    private List<Product> CreateTestProducts()
    {
        var category1 = new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" };
        var category2 = new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" };
        var seller = new ApplicationUser { Id = "seller1", FirstName = "Test", LastName = "Seller" };


        return new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop Gaming Pro",
                Description = "High performance gaming laptop",
                Price = 15000000,
                Stock = 10,
                ImageUrl = "/images/laptop.jpg",
                CategoryId = 1,
                Category = category1,
                SellerId = "seller1",
                Seller = seller,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-5)
            },
            new Product
            {
                Id = 2,
                Name = "Smartphone Ultra",
                Description = "Latest smartphone with great camera",
                Price = 8000000,
                Stock = 25,
                ImageUrl = "/images/phone.jpg",
                CategoryId = 1,
                Category = category1,
                SellerId = "seller1",
                Seller = seller,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-3)
            },
            new Product
            {
                Id = 3,
                Name = "T-Shirt Premium",
                Description = "Comfortable cotton t-shirt",
                Price = 150000,
                Stock = 100,
                ImageUrl = "/images/tshirt.jpg",
                CategoryId = 2,
                Category = category2,
                SellerId = "seller1",
                Seller = seller,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-1)
            },
            new Product
            {
                Id = 4,
                Name = "Jacket Winter",
                Description = "Warm winter jacket",
                Price = 500000,
                Stock = 50,
                ImageUrl = "/images/jacket.jpg",
                CategoryId = 2,
                Category = category2,
                SellerId = "seller1",
                Seller = seller,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        };
    }

    #endregion

    #region Search Tests

    [Fact]
    public async Task GetFilteredProductsAsync_WithSearchTerm_ReturnsMatchingProducts()
    {
        // Arrange
        var filter = new ProductFilterDto { Search = "laptop" };
        var expectedProducts = _testProducts.Where(p => 
            p.Name.ToLower().Contains("laptop") || 
            p.Description.ToLower().Contains("laptop")).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Contain("Laptop");
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithEmptySearch_ReturnsAllProducts()
    {
        // Arrange
        var filter = new ProductFilterDto { Search = "" };

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(_testProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithNoMatchingSearch_ReturnsEmptyList()
    {
        // Arrange
        var filter = new ProductFilterDto { Search = "nonexistent" };

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Category Filter Tests

    [Fact]
    public async Task GetFilteredProductsAsync_WithCategoryId_ReturnsProductsInCategory()
    {
        // Arrange
        var filter = new ProductFilterDto { CategoryId = 1 };
        var electronicsProducts = _testProducts.Where(p => p.CategoryId == 1).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(electronicsProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.CategoryName == "Electronics").Should().BeTrue();
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithInvalidCategoryId_ReturnsEmptyList()
    {
        // Arrange
        var filter = new ProductFilterDto { CategoryId = 999 };

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Price Range Filter Tests

    [Fact]
    public async Task GetFilteredProductsAsync_WithMinPrice_ReturnsProductsAboveMinPrice()
    {
        // Arrange
        var filter = new ProductFilterDto { MinPrice = 1000000 };
        var expensiveProducts = _testProducts.Where(p => p.Price >= 1000000).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(expensiveProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.Price >= 1000000).Should().BeTrue();
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithMaxPrice_ReturnsProductsBelowMaxPrice()
    {
        // Arrange
        var filter = new ProductFilterDto { MaxPrice = 500000 };
        var cheapProducts = _testProducts.Where(p => p.Price <= 500000).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(cheapProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.Price <= 500000).Should().BeTrue();
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithPriceRange_ReturnsProductsInRange()
    {
        // Arrange
        var filter = new ProductFilterDto { MinPrice = 100000, MaxPrice = 1000000 };
        var rangeProducts = _testProducts.Where(p => p.Price >= 100000 && p.Price <= 1000000).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(rangeProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(2); // T-Shirt (150000) and Jacket (500000)
        result.All(p => p.Price >= 100000 && p.Price <= 1000000).Should().BeTrue();
    }

    #endregion

    #region Sorting Tests

    [Fact]
    public async Task GetFilteredProductsAsync_WithSortByPriceAsc_ReturnsSortedProducts()
    {
        // Arrange
        var filter = new ProductFilterDto { SortBy = "price_asc" };
        var sortedProducts = _testProducts.OrderBy(p => p.Price).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(sortedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(4);
        resultList[0].Price.Should().BeLessThan(resultList[1].Price);
        resultList[1].Price.Should().BeLessThan(resultList[2].Price);
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithSortByPriceDesc_ReturnsSortedProducts()
    {
        // Arrange
        var filter = new ProductFilterDto { SortBy = "price_desc" };
        var sortedProducts = _testProducts.OrderByDescending(p => p.Price).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(sortedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(4);
        resultList[0].Price.Should().BeGreaterThan(resultList[1].Price);
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithSortByNewest_ReturnsSortedByDate()
    {
        // Arrange
        var filter = new ProductFilterDto { SortBy = "newest" };
        var sortedProducts = _testProducts.OrderByDescending(p => p.CreatedAt).ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(sortedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(4);
        resultList[0].Name.Should().Be("Jacket Winter"); // Most recent
    }

    #endregion

    #region Combined Filter Tests

    [Fact]
    public async Task GetFilteredProductsAsync_WithCombinedFilters_ReturnsCorrectProducts()
    {
        // Arrange
        var filter = new ProductFilterDto 
        { 
            Search = "laptop",
            CategoryId = 1,
            MinPrice = 10000000,
            MaxPrice = 20000000,
            SortBy = "price_desc"
        };
        
        var expectedProducts = _testProducts
            .Where(p => p.Name.ToLower().Contains("laptop") && 
                        p.CategoryId == 1 && 
                        p.Price >= 10000000 && 
                        p.Price <= 20000000)
            .OrderByDescending(p => p.Price)
            .ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Laptop Gaming Pro");
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithSearchAndCategory_FiltersBoth()
    {
        // Arrange
        var filter = new ProductFilterDto 
        { 
            Search = "smart",
            CategoryId = 1
        };
        
        var expectedProducts = _testProducts
            .Where(p => (p.Name.ToLower().Contains("smart") || 
                         p.Description.ToLower().Contains("smart")) && 
                        p.CategoryId == 1)
            .ToList();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Smartphone Ultra");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task GetFilteredProductsAsync_WithNullFilter_ReturnsAllProducts()
    {
        // Arrange
        var filter = new ProductFilterDto();

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                null, null, null, null, null))
            .ReturnsAsync(_testProducts);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);

        // Assert - Repository should be called with null values
        _mockProductRepository.Verify(x => x.GetFilteredProductsAsync(
            null, null, null, null, null), Times.Once);
    }

    [Fact]
    public async Task GetFilteredProductsAsync_MapsProductToDto_Correctly()
    {
        // Arrange
        var filter = new ProductFilterDto { Search = "laptop" };
        var singleProduct = new List<Product> { _testProducts[0] };

        _mockProductRepository
            .Setup(x => x.GetFilteredProductsAsync(
                filter.Search, filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.SortBy))
            .ReturnsAsync(singleProduct);

        // Act
        var result = await _productService.GetFilteredProductsAsync(filter);
        var dto = result.First();

        // Assert - Verify DTO mapping
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Laptop Gaming Pro");
        dto.Description.Should().Be("High performance gaming laptop");
        dto.Price.Should().Be(15000000);
        dto.CategoryName.Should().Be("Electronics");
        dto.SellerName.Should().Be("Test Seller");
    }

    #endregion
}
