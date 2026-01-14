import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Products.css';

function Products() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const initData = async () => {
            await Promise.all([fetchCategories(), fetchProducts()]);
            setLoading(false);
        };
        initData();
    }, []);

    const fetchCategories = async () => {
        try {
            const response = await fetch('/api/category');
            if (response.ok) {
                const data = await response.json();
                setCategories(data);
            }
        } catch (error) {
            console.error('Error fetching categories:', error);
        }
    };

    const fetchProducts = async (categoryId = null) => {
        setLoading(true);
        try {
            let url = '/api/products';
            if (categoryId) {
                url += `?categoryId=${categoryId}`;
            }

            const response = await fetch(url);
            if (!response.ok) throw new Error('Failed to fetch products');

            const data = await response.json();
            // Handle both array (direct list) and object (paginated/wrapped) formats
            if (Array.isArray(data)) {
                setProducts(data);
            } else {
                setProducts(data.products || []);
            }
            setSelectedCategory(categoryId);
        } catch (error) {
            console.error('Error fetching products:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleCategoryClick = (categoryId) => {
        fetchProducts(categoryId);
    };

    return (
        <div className="container my-5">
            <div className="row">
                {/* Sidebar Filters */}
                <div className="col-lg-3 mb-4">
                    <div className="card border-0 shadow-sm">
                        <div className="card-header bg-white py-3">
                            <h5 className="mb-0 fw-bold">Categories</h5>
                        </div>
                        <div className="list-group list-group-flush">
                            <button
                                className={`list-group-item list-group-item-action d-flex justify-content-between align-items-center ${!selectedCategory ? 'active' : ''}`}
                                onClick={() => handleCategoryClick(null)}
                            >
                                All Products
                                <i className="fas fa-chevron-right small"></i>
                            </button>
                            {categories.map(category => (
                                <button
                                    key={category.id}
                                    className={`list-group-item list-group-item-action d-flex justify-content-between align-items-center ${selectedCategory === category.id ? 'active' : ''}`}
                                    onClick={() => handleCategoryClick(category.id)}
                                >
                                    {category.name}
                                    <span className="badge bg-light text-dark rounded-pill">{category.productCount}</span>
                                </button>
                            ))}
                        </div>
                    </div>
                </div>

                {/* Products Grid */}
                <div className="col-lg-9">
                    <div className="d-flex justify-content-between align-items-center mb-4">
                        <h2 className="fw-bold mb-0">
                            {selectedCategory
                                ? categories.find(c => c.id === selectedCategory)?.name
                                : 'All Products'}
                        </h2>
                        <span className="text-muted">{products.length} Products Found</span>
                    </div>

                    {loading ? (
                        <div className="text-center py-5">
                            <div className="spinner-border text-primary" role="status">
                                <span className="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    ) : products.length === 0 ? (
                        <div className="text-center py-5 bg-light rounded">
                            <i className="fas fa-search fa-3x text-muted mb-3"></i>
                            <h4>No products found</h4>
                            <p className="text-muted">Try selecting a different category.</p>
                        </div>
                    ) : (
                        <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                            {products.map((product) => (
                                <div className="col" key={product.id}>
                                    <div className="card h-100 shadow-sm border-0 position-relative product-card">
                                        <div className="position-absolute top-0 end-0 p-3" style={{ zIndex: 1 }}>
                                            <span className="badge bg-white text-dark shadow-sm">{product.categoryName}</span>
                                        </div>

                                        <div className="position-absolute top-0 start-0 p-3" style={{ zIndex: 1 }}>
                                            <span className="badge bg-white text-muted shadow-sm">
                                                <i className="fas fa-store me-1"></i> {product.sellerName || 'Seller'}
                                            </span>
                                        </div>

                                        <div className="overflow-hidden bg-light rounded-top-lg" style={{ height: '200px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                                            <img
                                                src={product.imageUrl}
                                                className="card-img-top w-100 h-100"
                                                style={{ objectFit: 'cover' }}
                                                alt={product.name}
                                            />
                                        </div>

                                        <div className="card-body d-flex flex-column p-3">
                                            <h5 className="card-title text-truncate mb-2" title={product.name}>
                                                {product.name}
                                            </h5>
                                            <p className="card-text text-muted small text-truncate mb-2">
                                                {product.description}
                                            </p>

                                            <div className="mb-2">
                                                <span className="fw-bold text-primary fs-5">
                                                    Rp {product.price.toLocaleString('id-ID')}
                                                </span>
                                            </div>

                                            <div className="mt-auto d-grid gap-2">
                                                <Link to={`/product/${product.id}`} className="btn btn-outline-primary btn-sm">
                                                    View Details
                                                </Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default Products;
