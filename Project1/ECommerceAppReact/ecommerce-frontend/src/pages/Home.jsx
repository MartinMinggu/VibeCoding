import React from 'react';
import './Home.css';

function Home() {
    // Featured products for home page
    const featuredProducts = [
        {
            id: 1,
            name: 'MacBook Pro M3 16"',
            price: 42500000,
            imageUrl: 'https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400&h=300&fit=crop',
            category: 'Laptop'
        },
        {
            id: 2,
            name: 'Sony WH-1000XM5 Headphones',
            price: 5299000,
            imageUrl: 'https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=400&h=300&fit=crop',
            category: 'Audio'
        },
        {
            id: 3,
            name: 'iPhone 15 Pro Max',
            price: 21999000,
            imageUrl: 'https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=400&h=300&fit=crop',
            category: 'Smartphone'
        },
        {
            id: 5,
            name: 'Nike Air Jordan 1 Retro',
            price: 3299000,
            imageUrl: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&h=300&fit=crop',
            category: 'Sneakers'
        },
        {
            id: 9,
            name: 'Yonex Astrox 99 Pro',
            price: 3850000,
            imageUrl: 'https://images.unsplash.com/photo-1626224583764-f87db24ac4ea?w=400&h=300&fit=crop',
            category: 'Sports'
        },
        {
            id: 11,
            name: 'Garmin Fenix 7X Solar',
            price: 14999000,
            imageUrl: 'https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=400&h=300&fit=crop',
            category: 'Wearables'
        },
        {
            id: 13,
            name: 'Dyson V15 Detect Vacuum',
            price: 11499000,
            imageUrl: 'https://images.unsplash.com/photo-1558317374-067fb5f30001?w=400&h=300&fit=crop',
            category: 'Home'
        },
        {
            id: 16,
            name: 'Philips Hue Smart Bulb Kit',
            price: 3299000,
            imageUrl: 'https://images.unsplash.com/photo-1585076800662-a9cfe7f9e86a?w=400&h=300&fit=crop',
            category: 'Smart Home'
        },
    ];

    return (
        <>
            {/* Hero Section - Exact match with MVC */}
            <div className="home-hero py-5 mb-5" style={{ background: 'linear-gradient(135deg, #6366f1 0%, #a855f7 100%)' }}>
                <div className="container text-center py-5">
                    <h1 className="display-3 fw-bold text-white mb-3 animate__animated animate__fadeInDown">Welcome to ECommerceApp</h1>
                    <p className="lead text-white-50 mb-4 animate__animated animate__fadeInUp">Your premium shopping experience starts here.</p>
                    <a href="/products" className="btn btn-light btn-lg rounded-pill px-5 py-3 shadow">
                        <i className="fas fa-shopping-bag me-2"></i> Shop Now
                    </a>
                </div>
            </div>

            {/* Featured Products Section */}
            <div className="container mb-5">
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <h2 className="fw-bold mb-0">Featured Products</h2>
                    <a href="/products" className="btn btn-outline-primary rounded-pill">
                        View All <i className="fas fa-arrow-right ms-2"></i>
                    </a>
                </div>

                <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                    {featuredProducts.map((product) => (
                        <div className="col" key={product.id}>
                            <div className="card h-100 border-0 shadow-sm product-card hover-lift">
                                <div className="position-relative overflow-hidden">
                                    <img
                                        src={product.imageUrl}
                                        className="card-img-top"
                                        alt={product.name}
                                        style={{ height: '200px', objectFit: 'cover' }}
                                    />
                                    <div className="product-badge position-absolute top-0 end-0 m-2">
                                        <span className="badge bg-primary rounded-pill">{product.category}</span>
                                    </div>
                                </div>
                                <div className="card-body d-flex flex-column p-3" style={{ minHeight: '200px' }}>
                                    <h5 className="card-title fw-bold mb-3 text-truncate" title={product.name}>
                                        {product.name}
                                    </h5>

                                    <div className="mb-3" style={{ overflow: 'hidden' }}>
                                        <span className="fw-bold text-primary d-block text-truncate" style={{ fontSize: '1.1rem' }}>
                                            Rp {product.price.toLocaleString('id-ID')}
                                        </span>
                                    </div>

                                    <div className="d-grid mt-auto">
                                        <a href={`/product/${product.id}`} className="btn btn-primary rounded-pill">
                                            View Details
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {/* Features Section */}
            <div className="container mb-5">
                <div className="row g-4">
                    <div className="col-md-4">
                        <div className="card border-0 shadow-sm h-100 text-center p-4">
                            <div className="mb-3">
                                <i className="fas fa-shipping-fast fa-3x text-primary"></i>
                            </div>
                            <h5 className="fw-bold">Fast Shipping</h5>
                            <p className="text-muted mb-0">Free delivery on orders over Rp 500.000</p>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="card border-0 shadow-sm h-100 text-center p-4">
                            <div className="mb-3">
                                <i className="fas fa-shield-alt fa-3x text-success"></i>
                            </div>
                            <h5 className="fw-bold">Secure Payment</h5>
                            <p className="text-muted mb-0">100% secure payment with encryption</p>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="card border-0 shadow-sm h-100 text-center p-4">
                            <div className="mb-3">
                                <i className="fas fa-headset fa-3x text-info"></i>
                            </div>
                            <h5 className="fw-bold">24/7 Support</h5>
                            <p className="text-muted mb-0">Dedicated support team ready to help</p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Home;
