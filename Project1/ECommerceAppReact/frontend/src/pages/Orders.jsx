import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Orders.css';

function Orders() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchOrders();
    }, []);

    const fetchOrders = async () => {
        try {
            const response = await fetch('/api/order');
            if (!response.ok) throw new Error('Failed to fetch orders');

            const data = await response.json();
            setOrders(data);
        } catch (error) {
            console.error('Error fetching orders:', error);
        } finally {
            setLoading(false);
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

    return (
        <div className="container my-5">
            <h1 className="display-5 fw-bold mb-4">My Orders</h1>

            {orders.length === 0 ? (
                <div className="text-center py-5">
                    <i className="fas fa-box-open fa-5x text-muted mb-4"></i>
                    <h3 className="text-muted">No orders found</h3>
                    <p className="text-muted mb-4">You haven't placed any orders yet.</p>
                    <Link to="/products" className="btn btn-primary btn-lg rounded-pill">
                        Start Shopping
                    </Link>
                </div>
            ) : (
                <div className="row">
                    <div className="col-12">
                        {orders.map((order) => (
                            <div key={order.id} className="card border-0 shadow-sm mb-4">
                                <div className="card-header bg-white py-3 d-flex justify-content-between align-items-center flex-wrap">
                                    <div>
                                        <h6 className="mb-0 fw-bold">{order.orderNumber}</h6>
                                        <small className="text-muted">
                                            Placed on {new Date(order.orderDate).toLocaleDateString()}
                                        </small>
                                    </div>
                                    <div className="text-end">
                                        <span className={`badge rounded-pill status-badge ${getStatusBadgeClass(order.status)} me-3`}>
                                            {order.status}
                                        </span>
                                        <span className="fw-bold text-primary">
                                            Rp {order.totalAmount.toLocaleString('id-ID')}
                                        </span>
                                    </div>
                                </div>
                                <div className="card-body">
                                    <div className="table-responsive">
                                        <table className="table table-borderless align-middle mb-0">
                                            <tbody>
                                                {order.items.slice(0, 3).map((item) => (
                                                    <tr key={item.id}>
                                                        <td style={{ width: '60px' }}>
                                                            <img
                                                                src={item.productImageUrl || 'https://via.placeholder.com/50'}
                                                                alt={item.productName}
                                                                className="rounded"
                                                                style={{ width: '50px', height: '50px', objectFit: 'cover' }}
                                                            />
                                                        </td>
                                                        <td>
                                                            <div className="fw-semibold">{item.productName}</div>
                                                            <small className="text-muted">Rp {item.unitPrice.toLocaleString('id-ID')} x {item.quantity}</small>
                                                        </td>
                                                        <td className="text-end fw-semibold">
                                                            Rp {item.subtotal.toLocaleString('id-ID')}
                                                        </td>
                                                    </tr>
                                                ))}
                                                {order.items.length > 3 && (
                                                    <tr>
                                                        <td colSpan="3" className="text-center text-muted small">
                                                            + {order.items.length - 3} more items...
                                                        </td>
                                                    </tr>
                                                )}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div className="card-footer bg-white py-3">
                                    <div className="d-flex justify-content-between align-items-center">
                                        <span className="text-muted small">
                                            {order.itemCount} Items
                                        </span>
                                        <button className="btn btn-outline-primary btn-sm rounded-pill">
                                            View Order Details
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
}

function getStatusBadgeClass(status) {
    switch (status.toLowerCase()) {
        case 'pending': return 'bg-warning text-dark';
        case 'processing': return 'bg-info text-white';
        case 'shipped': return 'bg-primary';
        case 'delivered': return 'bg-success';
        case 'cancelled': return 'bg-danger';
        default: return 'bg-secondary';
    }
}

export default Orders;
