import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Checkout.css';

function Checkout() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [checkoutData, setCheckoutData] = useState(null);
    const [formData, setFormData] = useState({
        shippingAddress: '',
        paymentMethod: 'Credit Card'
    });

    useEffect(() => {
        fetchCheckoutDetails();
    }, []);

    const fetchCheckoutDetails = async () => {
        try {
            const response = await fetch('/api/order/checkout');
            if (response.status === 400) {
                // Cart empty
                alert('Your cart is empty');
                navigate('/cart');
                return;
            }
            if (!response.ok) throw new Error('Failed to fetch checkout details');

            const data = await response.json();
            setCheckoutData(data);
            if (data.shippingAddress) {
                setFormData(prev => ({ ...prev, shippingAddress: data.shippingAddress }));
            }
        } catch (error) {
            console.error('Error fetching checkout details:', error);
            // navigate('/cart');
        } finally {
            setLoading(false);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSubmitting(true);

        try {
            const response = await fetch('/api/order', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.message || 'Failed to place order');
            }

            const data = await response.json();
            alert('Order placed successfully!');
            navigate('/orders');
        } catch (error) {
            console.error('Error placing order:', error);
            alert(error.message || 'Error placing order');
        } finally {
            setSubmitting(false);
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

    if (!checkoutData) return null;

    return (
        <div className="container my-5">
            <h1 className="display-5 fw-bold mb-4">Checkout</h1>

            <div className="row">
                <div className="col-md-8">
                    <div className="card border-0 shadow-sm mb-4">
                        <div className="card-header bg-white py-3">
                            <h5 className="mb-0">Shipping & Payment</h5>
                        </div>
                        <div className="card-body p-4">
                            <form id="checkoutForm" onSubmit={handleSubmit}>
                                <div className="mb-4">
                                    <label className="form-label fw-bold">Shipping Address</label>
                                    <textarea
                                        name="shippingAddress"
                                        className="form-control"
                                        rows="3"
                                        value={formData.shippingAddress}
                                        onChange={handleInputChange}
                                        placeholder="Enter your full delivery address"
                                        required
                                    ></textarea>
                                </div>

                                <div className="mb-4">
                                    <label className="form-label fw-bold">Payment Method</label>
                                    <div className="form-check mb-2">
                                        <input
                                            className="form-check-input"
                                            type="radio"
                                            name="paymentMethod"
                                            id="creditCard"
                                            value="Credit Card"
                                            checked={formData.paymentMethod === 'Credit Card'}
                                            onChange={handleInputChange}
                                        />
                                        <label className="form-check-label" htmlFor="creditCard">
                                            <i className="fas fa-credit-card me-2"></i> Credit Card
                                        </label>
                                    </div>
                                    <div className="form-check mb-2">
                                        <input
                                            className="form-check-input"
                                            type="radio"
                                            name="paymentMethod"
                                            id="bankTransfer"
                                            value="Bank Transfer"
                                            checked={formData.paymentMethod === 'Bank Transfer'}
                                            onChange={handleInputChange}
                                        />
                                        <label className="form-check-label" htmlFor="bankTransfer">
                                            <i className="fas fa-university me-2"></i> Bank Transfer
                                        </label>
                                    </div>
                                    <div className="form-check">
                                        <input
                                            className="form-check-input"
                                            type="radio"
                                            name="paymentMethod"
                                            id="cod"
                                            value="COD"
                                            checked={formData.paymentMethod === 'COD'}
                                            onChange={handleInputChange}
                                        />
                                        <label className="form-check-label" htmlFor="cod">
                                            <i className="fas fa-money-bill-wave me-2"></i> Cash on Delivery
                                        </label>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="card border-0 shadow-sm sticky-top" style={{ top: '100px' }}>
                        <div className="card-header bg-white py-3">
                            <h5 className="mb-0">Order Summary</h5>
                        </div>
                        <div className="card-body">
                            <div className="checkout-items mb-3" style={{ maxHeight: '300px', overflowY: 'auto' }}>
                                {checkoutData.cartItems.map((item) => (
                                    <div key={item.id} className="d-flex justify-content-between mb-2">
                                        <span>
                                            {item.quantity}x {item.productName}
                                        </span>
                                        <span className="fw-semibold">
                                            {((item.unitPrice * item.quantity) / 1000).toFixed(0)}.000
                                        </span>
                                    </div>
                                ))}
                            </div>
                            <hr />
                            <div className="d-flex justify-content-between mb-2">
                                <span>Subtotal</span>
                                <span>Rp {checkoutData.totalAmount?.toLocaleString('id-ID')}</span>
                            </div>
                            <div className="d-flex justify-content-between mb-4">
                                <strong>Total</strong>
                                <strong className="text-primary fs-5">
                                    Rp {checkoutData.totalAmount?.toLocaleString('id-ID')}
                                </strong>
                            </div>

                            <button
                                type="submit"
                                form="checkoutForm"
                                className="btn btn-primary w-100 btn-lg"
                                disabled={submitting}
                            >
                                {submitting ? (
                                    <>
                                        <span className="spinner-border spinner-border-sm me-2"></span>
                                        Processing...
                                    </>
                                ) : (
                                    <>
                                        Place Order <i className="fas fa-check-circle ms-2"></i>
                                    </>
                                )}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Checkout;
