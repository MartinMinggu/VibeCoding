import React, { useState, useEffect } from 'react';
import './Products.css';

function Products() {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Rich varied mock data
        const mockProducts = [
            // Electronics
            {
                id: 1,
                name: 'MacBook Pro M3 16" 2024',
                price: 42500000,
                stock: 5,
                imageUrl: 'https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400&h=300&fit=crop',
                categoryName: 'Laptop',
                sellerName: 'Apple Store Indonesia',
                description: 'Powerful laptop with M3 chip, 32GB RAM, 1TB SSD'
            },
            {
                id: 2,
                name: 'Sony WH-1000XM5 Premium Headphones',
                price: 5299000,
                stock: 18,
                imageUrl: 'https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=400&h=300&fit=crop',
                categoryName: 'Audio',
                sellerName: 'Sony Official Store',
                description: 'Industry-leading noise cancellation wireless headphones'
            },
            {
                id: 3,
                name: 'iPhone 15 Pro Max 256GB Titanium',
                price: 21999000,
                stock: 3,
                imageUrl: 'https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=400&h=300&fit=crop',
                categoryName: 'Smartphone',
                sellerName: 'Apple Store Indonesia',
                description: 'Latest iPhone with A17 Pro chip and titanium design'
            },
            {
                id: 4,
                name: 'Samsung 65" Neo QLED 4K Smart TV',
                price: 28900000,
                stock: 0,
                imageUrl: 'https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?w=400&h=300&fit=crop',
                categoryName: 'TV & Entertainment',
                sellerName: 'Samsung Electronics',
                description: 'Quantum Matrix Technology with Neural Quantum Processor'
            },

            // Fashion
            {
                id: 5,
                name: 'Nike Air Jordan 1 Retro High OG',
                price: 3299000,
                stock: 12,
                imageUrl: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&h=300&fit=crop',
                categoryName: 'Sneakers',
                sellerName: 'Nike Official',
                description: 'Classic basketball sneakers with premium leather'
            },
            {
                id: 6,
                name: 'Adidas Ultra Boost 23 Running Shoes',
                price: 2899000,
                stock: 25,
                imageUrl: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=400&h=300&fit=crop',
                categoryName: 'Sneakers',
                sellerName: 'Adidas Official',
                description: 'Responsive cushioning for energized runs'
            },
            {
                id: 7,
                name: 'Ray-Ban Aviator Classic Sunglasses',
                price: 2150000,
                stock: 8,
                imageUrl: 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=400&h=300&fit=crop',
                categoryName: 'Accessories',
                sellerName: 'Luxottica Indonesia',
                description: 'Iconic teardrop lens shape with metal frame'
            },
            {
                id: 8,
                name: 'Levi\'s 501 Original Fit Jeans',
                price: 1299000,
                stock: 30,
                imageUrl: 'https://images.unsplash.com/photo-1542272604-787c3835535d?w=400&h=300&fit=crop',
                categoryName: 'Fashion',
                sellerName: 'Levi\'s Store',
                description: 'The original blue jean since 1873'
            },

            // Sports & Outdoor
            {
                id: 9,
                name: 'Yonex Astrox 99 Pro Badminton Racket',
                price: 3850000,
                stock: 6,
                imageUrl: 'https://images.unsplash.com/photo-1626224583764-f87db24ac4ea?w=400&h=300&fit=crop',
                categoryName: 'Sports Equipment',
                sellerName: 'Yonex Indonesia',
                description: 'Professional badminton racket used by world champions'
            },
            {
                id: 10,
                name: 'Trek FX 3 Disc Hybrid Bike 2024',
                price: 12500000,
                stock: 4,
                imageUrl: 'https://images.unsplash.com/photo-1576435728678-68d0fbf94e91?w=400&h=300&fit=crop',
                categoryName: 'Cycling',
                sellerName: 'Trek Bicycle Store',
                description: 'Versatile fitness bike with disc brakes'
            },
            {
                id: 11,
                name: 'Garmin Fenix 7X Solar Smartwatch',
                price: 14999000,
                stock: 7,
                imageUrl: 'https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=400&h=300&fit=crop',
                categoryName: 'Wearables',
                sellerName: 'Garmin Official',
                description: 'Multisport GPS watch with solar charging'
            },
            {
                id: 12,
                name: 'Coleman Sundome 4-Person Tent',
                price: 1899000,
                stock: 15,
                imageUrl: 'https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?w=400&h=300&fit=crop',
                categoryName: 'Camping',
                sellerName: 'Outdoor Haven',
                description: 'Easy setup camping tent for weekend adventures'
            },

            // Home & Living
            {
                id: 13,
                name: 'Dyson V15 Detect Absolute Vacuum',
                price: 11499000,
                stock: 9,
                imageUrl: 'https://images.unsplash.com/photo-1558317374-067fb5f30001?w=400&h=300&fit=crop',
                categoryName: 'Home Appliances',
                sellerName: 'Dyson Indonesia',
                description: 'Cordless vacuum with laser dust detection'
            },
            {
                id: 14,
                name: 'KitchenAid Artisan Stand Mixer',
                price: 8750000,
                stock: 11,
                imageUrl: 'https://images.unsplash.com/photo-1585659722983-3a675dabf23d?w=400&h=300&fit=crop',
                categoryName: 'Kitchen',
                sellerName: 'KitchenAid Store',
                description: 'Iconic stand mixer with 10-speed control'
            },
            {
                id: 15,
                name: 'IKEA POÄNG Armchair with Cushion',
                price: 2499000,
                stock: 0,
                imageUrl: 'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=400&h=300&fit=crop',
                categoryName: 'Furniture',
                sellerName: 'IKEA Indonesia',
                description: 'Comfortable bentwood armchair with soft cushion'
            },
            {
                id: 16,
                name: 'Philips Hue Smart Bulb Starter Kit',
                price: 3299000,
                stock: 20,
                imageUrl: 'https://images.unsplash.com/photo-1585076800662-a9cfe7f9e86a?w=400&h=300&fit=crop',
                categoryName: 'Smart Home',
                sellerName: 'Philips Lighting',
                description: 'WiFi-enabled color changing smart bulbs with bridge'
            },
        ];

        setProducts(mockProducts);
        setLoading(false);
    }, []);

    if (loading) {
        return (
            <div className="container my-5 text-center">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
                <p className="mt-3 text-muted">Loading amazing products...</p>
            </div>
        );
    }

    return (
        <div className="container my-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1 className="display-5 fw-bold text-dark">Our Products</h1>
                <span className="badge bg-primary fs-6">{products.length} Products</span>
            </div>

            {/* Products Grid - Match MVC exactly */}
            <div className="row row-cols-1 row-cols-md-3 row-cols-lg-4 g-4">
                {products.map((product) => (
                    <div className="col" key={product.id}>
                        <div className="card h-100 shadow-sm border-0 position-relative product-card">
                            {/* Category Badge */}
                            <div className="position-absolute top-0 end-0 p-3" style={{ zIndex: 1 }}>
                                <span className="badge bg-white text-dark shadow-sm">{product.categoryName}</span>
                            </div>

                            {/* Seller Badge */}
                            <div className="position-absolute top-0 start-0 p-3">
                                <span className="badge bg-white text-muted shadow-sm">
                                    <i className="fas fa-store me-1"></i> {product.sellerName}
                                </span>
                            </div>

                            {/* Product Image */}
                            <div className="overflow-hidden bg-light rounded-top-lg" style={{ height: '240px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                                <img
                                    src={product.imageUrl}
                                    className="card-img-top w-100 h-100"
                                    style={{ objectFit: 'cover' }}
                                    alt={product.name}
                                />
                            </div>

                            {/* Card Body */}
                            <div className="card-body d-flex flex-column pt-3 pb-3" style={{ minHeight: '280px' }}>
                                <h5 className="card-title text-truncate mb-2" title={product.name}>
                                    {product.name}
                                </h5>
                                <p className="card-text text-muted small text-truncate mb-2">
                                    {product.description}
                                </p>

                                {/* Stock Indicator */}
                                <div className="mb-2">
                                    {product.stock > 10 ? (
                                        <span className="badge bg-success-subtle text-success small">
                                            <i className="fas fa-check-circle me-1"></i> In Stock
                                        </span>
                                    ) : product.stock > 0 ? (
                                        <span className="badge bg-warning-subtle text-warning small">
                                            <i className="fas fa-fire me-1"></i> Only {product.stock} left!
                                        </span>
                                    ) : (
                                        <span className="badge bg-danger-subtle text-danger small">
                                            <i className="fas fa-times-circle me-1"></i> Out of Stock
                                        </span>
                                    )}
                                </div>

                                {/* Price */}
                                <div className="mb-2" style={{ overflow: 'hidden' }}>
                                    <span className="fw-bold text-primary d-block text-truncate" style={{ fontSize: '1.05rem' }}>
                                        Rp {product.price.toLocaleString('id-ID')}
                                    </span>
                                </div>

                                {/* Rating */}
                                <div className="text-warning small mb-3">
                                    <i className="fas fa-star"></i>
                                    <i className="fas fa-star"></i>
                                    <i className="fas fa-star"></i>
                                    <i className="fas fa-star"></i>
                                    <i className={product.id % 3 === 0 ? "far fa-star" : "fas fa-star"}></i>
                                    <span className="text-muted ms-1">({Math.floor(Math.random() * 100) + 50})</span>
                                </div>

                                {/* Action Buttons */}
                                <div className="mt-auto d-grid gap-2">
                                    <a href={`/product/${product.id}`} className="btn btn-outline-secondary btn-sm">
                                        View Details
                                    </a>
                                    {product.stock > 0 ? (
                                        <button className="btn btn-primary btn-sm">
                                            <i className="fas fa-cart-plus me-2"></i> Add to Cart
                                        </button>
                                    ) : (
                                        <button className="btn btn-secondary btn-sm" disabled>
                                            <i className="fas fa-ban me-2"></i> Out of Stock
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Products;
