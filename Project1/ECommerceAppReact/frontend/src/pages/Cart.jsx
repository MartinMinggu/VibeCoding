import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './Cart.css';

function Cart() {
    const [cart, setCart] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        fetchCart();
    }, []);

    const fetchCart = async () => {
        try {
            const response = await fetch('/api/cart');
            if (!response.ok) throw new Error('Failed to fetch cart');

            const data = await response.json();
            setCart(data);
        } catch (error) {
            console.error('Error fetching cart:', error);
        } finally {
            setLoading(false);
        }
    };

    const updateQuantity = async (cartItemId, quantity) => {
        if (quantity < 1) return;

        try {
            const response = await fetch(`/api/cart/items/${cartItemId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ quantity })
            });

            if (response.ok) {
                const updatedCart = await response.json();
                setCart(updatedCart);
            }
        } catch (error) {
            console.error('Error updating quantity:', error);
        }
    };

    const removeItem = async (cartItemId) => {
        try {
            const response = await fetch(`/api/cart/items/${cartItemId}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                const updatedCart = await response.json();
                setCart(updatedCart);
            }
        } catch (error) {
            console.error('Error removing item:', error);
        }
    };

    const clearCart = async () => {
        if (!confirm('Are you sure you want to clear your cart?')) return;

        try {
            await fetch('/api/cart', { method: 'DELETE' });
            setCart({ items: [], totalAmount: 0 });
        } catch (error) {
            console.error('Error clearing cart:', error);
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

    const items = cart?.items || [];
    const isEmpty = items.length === 0;

    return (
        <div className="container my-5">
            <h1 className="display-5 fw-bold mb-4">Shopping Cart</h1>

            {isEmpty ? (
                <div className="text-center py-5">
                    <i className="fas fa-shopping-cart fa-5x text-muted mb-4"></i>
                    <h3 className="text-muted">Your cart is empty</h3>
                    <p className="text-muted mb-4">Add some products to get started!</p>
                    <Link to="/products" className="btn btn-primary btn-lg rounded-pill">
                        <i className="fas fa-shopping-bag me-2"></i>
                        Browse Products
                    </Link>
                </div>
            ) : (
                <div className="row">
                    <div className="col-lg-8">
                        <div className="card border-0 shadow-sm mb-4">
                            <div className="card-body">
                                {items.map((item) => (
                                    <div key={item.id} className="cart-item border-bottom py-3">
                                        <div className="row align-items-center">
                                            <div className="col-md-2">
                                                <img
                                                    src={item.product?.imageUrl || 'https://via.placeholder.com/100'}
                                                    alt={item.product?.name}
                                                    className="img-fluid rounded"
                                                />
                                            </div>
                                            <div className="col-md-4">
                                                <h5 className="mb-1">{item.product?.name}</h5>
                                                <p className="text-muted small mb-0">{item.product?.categoryName}</p>
                                            </div>
                                            <div className="col-md-2">
                                                <p className="fw-bold text-primary mb-0">
                                                    Rp {item.product?.price?.toLocaleString('id-ID')}
                                                </p>
                                            </div>
                                            <div className="col-md-2">
                                                <div className="input-group input-group-sm">
                                                    <button
                                                        className="btn btn-outline-secondary"
                                                        onClick={() => updateQuantity(item.id, item.quantity - 1)}
                                                        disabled={item.quantity <= 1}
                                                    >
                                                        <i className="fas fa-minus"></i>
                                                    </button>
                                                    <input
                                                        type="text"
                                                        className="form-control text-center"
                                                        value={item.quantity}
                                                        readOnly
                                                    />
                                                    <button
                                                        className="btn btn-outline-secondary"
                                                        onClick={() => updateQuantity(item.id, item.quantity + 1)}
                                                    >
                                                        <i className="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div className="col-md-2 text-end">
                                                <button
                                                    className="btn btn-sm btn-outline-danger"
                                                    onClick={() => removeItem(item.id)}
                                                >
                                                    <i className="fas fa-trash"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                ))}

                                <div className="text-end mt-3">
                                    <button onClick={clearCart} className="btn btn-outline-danger">
                                        <i className="fas fa-trash me-2"></i>Clear Cart
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-4">
                        <div className="card border-0 shadow-sm sticky-top" style={{ top: '100px' }}>
                            <div className="card-body">
                                <h5 className="card-title mb-4">Order Summary</h5>

                                <div className="d-flex justify-content-between mb-2">
                                    <span>Subtotal ({items.length} items)</span>
                                    <span>Rp {cart?.totalAmount?.toLocaleString('id-ID') || 0}</span>
                                </div>
                                <div className="d-flex justify-content-between mb-2">
                                    <span>Shipping</span>
                                    <span className="text-success">Free</span>
                                </div>
                                <hr />
                                <div className="d-flex justify-content-between mb-4">
                                    <strong>Total</strong>
                                    <strong className="text-primary">
                                        Rp {cart?.totalAmount?.toLocaleString('id-ID') || 0}
                                    </strong>
                                </div>

                                <div className="d-grid gap-2">
                                    <button
                                        className="btn btn-primary btn-lg"
                                        onClick={() => navigate('/checkout')}
                                    >
                                        Proceed to Checkout
                                    </button>
                                    <Link to="/products" className="btn btn-outline-secondary">
                                        Continue Shopping
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default Cart;
