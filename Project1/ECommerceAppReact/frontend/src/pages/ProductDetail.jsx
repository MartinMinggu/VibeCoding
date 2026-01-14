import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import './ProductDetail.css';

function ProductDetail() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [loading, setLoading] = useState(true);
    const [adding, setAdding] = useState(false);

    useEffect(() => {
        fetchProduct();
    }, [id]);

    const fetchProduct = async () => {
        try {
            const response = await fetch(`/api/products/${id}`);
            if (!response.ok) throw new Error('Product not found');

            const data = await response.json();
            setProduct(data);
        } catch (error) {
            console.error('Error fetching product:', error);
        } finally {
            setLoading(false);
        }
    };

    const addToCart = async () => {
        setAdding(true);
        try {
            const response = await fetch('/api/cart/items', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    productId: parseInt(id),
                    quantity
                })
            });

            if (response.ok) {
                // Show success message
                alert('Product added to cart!');
                // Optionally navigate to cart
                // navigate('/cart');
            } else {
                alert('Failed to add to cart. Please login first.');
            }
        } catch (error) {
            console.error('Error adding to cart:', error);
            alert('Error adding to cart');
        } finally {
            setAdding(false);
        }
    };

    if (loading) {
        return (
            <div className="container my-5 text-center">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
            </div>
        );
    }

    if (!product) {
        return (
            <div className="container my-5 text-center">
                <h3>Product not found</h3>
                <Link to="/products" className="btn btn-primary mt-3">Back to Products</Link>
            </div>
        );
    }

    return (
        <div className="container my-5">
            <nav aria-label="breadcrumb">
                <ol className="breadcrumb">
                    <li className="breadcrumb-item"><Link to="/">Home</Link></li>
                    <li className="breadcrumb-item"><Link to="/products">Products</Link></li>
                    <li className="breadcrumb-item active">{product.name}</li>
                </ol>
            </nav>

            <div className="row">
                <div className="col-md-6">
                    <div className="product-image-container">
                        <img
                            src={product.imageUrl || 'https://via.placeholder.com/500'}
                            alt={product.name}
                            className="img-fluid rounded shadow-sm"
                        />
                    </div>
                </div>

                <div className="col-md-6">
                    <h1 className="display-5 fw-bold mb-3">{product.name}</h1>

                    <div className="mb-3">
                        <span className="badge bg-primary me-2">{product.categoryName}</span>
                        {product.stock > 0 ? (
                            <span className="badge bg-success">In Stock ({product.stock})</span>
                        ) : (
                            <span className="badge bg-danger">Out of Stock</span>
                        )}
                    </div>

                    <div className="mb-4">
                        <h2 className="text-primary fw-bold">
                            Rp {product.price?.toLocaleString('id-ID')}
                        </h2>
                    </div>

                    <div className="mb-4">
                        <h5>Description</h5>
                        <p className="text-muted">{product.description || 'No description available.'}</p>
                    </div>

                    {product.sellerName && (
                        <div className="mb-4">
                            <h6 className="text-muted">
                                <i className="fas fa-store me-2"></i>
                                Sold by: <span className="text-dark">{product.sellerName}</span>
                            </h6>
                        </div>
                    )}

                    {product.stock > 0 ? (
                        <div className="mb-4">
                            <label className="form-label fw-semibold">Quantity</label>
                            <div className="input-group" style={{ maxWidth: '150px' }}>
                                <button
                                    className="btn btn-outline-secondary"
                                    onClick={() => setQuantity(Math.max(1, quantity - 1))}
                                >
                                    <i className="fas fa-minus"></i>
                                </button>
                                <input
                                    type="text"
                                    className="form-control text-center"
                                    value={quantity}
                                    readOnly
                                />
                                <button
                                    className="btn btn-outline-secondary"
                                    onClick={() => setQuantity(Math.min(product.stock, quantity + 1))}
                                >
                                    <i className="fas fa-plus"></i>
                                </button>
                            </div>
                        </div>
                    ) : null}

                    <div className="d-grid gap-2">
                        {product.stock > 0 ? (
                            <>
                                <button
                                    className="btn btn-primary btn-lg"
                                    onClick={addToCart}
                                    disabled={adding}
                                >
                                    {adding ? (
                                        <>
                                            <span className="spinner-border spinner-border-sm me-2"></span>
                                            Adding...
                                        </>
                                    ) : (
                                        <>
                                            <i className="fas fa-cart-plus me-2"></i>
                                            Add to Cart
                                        </>
                                    )}
                                </button>
                                <button className="btn btn-outline-secondary">
                                    <i className="fas fa-heart me-2"></i>
                                    Add to Wishlist
                                </button>
                            </>
                        ) : (
                            <button className="btn btn-secondary btn-lg" disabled>
                                <i className="fas fa-ban me-2"></i>
                                Out of Stock
                            </button>
                        )}
                    </div>
                </div>
            </div>

            {/* Similar Products Section - Optional */}
            <div className="mt-5">
                <h3 className="mb-4">You May Also Like</h3>
                <div className="row">
                    {/* Similar products can be loaded here */}
                    <div className="col-12 text-center text-muted">
                        <p>Loading similar products...</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ProductDetail;
