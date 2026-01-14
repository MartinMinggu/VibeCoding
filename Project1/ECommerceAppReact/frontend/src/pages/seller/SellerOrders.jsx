import React, { useState, useEffect } from 'react';
import './SellerOrders.css';

function SellerOrders() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchOrders();
    }, []);

    const fetchOrders = async () => {
        try {
            const response = await fetch('/api/order/seller');
            if (response.ok) {
                const data = await response.json();
                setOrders(data);
            }
        } catch (error) {
            console.error('Error fetching orders:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleStatusUpdate = async (orderId, newStatus) => {
        try {
            const response = await fetch(`/api/order/${orderId}/status`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ status: newStatus })
            });

            if (response.ok) {
                setOrders(orders.map(o =>
                    o.orderId === orderId ? { ...o, status: newStatus } : o
                ));
                alert('Order status updated');
            } else {
                alert('Failed to update status');
            }
        } catch (error) {
            console.error('Error updating status:', error);
        }
    };

    if (loading) return <div className="text-center mt-5"><span className="spinner-border text-primary"></span></div>;

    return (
        <div className="container my-5">
            <h1 className="display-5 fw-bold mb-4">Manage Orders</h1>

            {orders.length === 0 ? (
                <div className="text-center py-5 bg-light rounded">
                    <p className="lead text-muted">No orders found.</p>
                </div>
            ) : (
                <div className="card border-0 shadow-sm">
                    <div className="table-responsive">
                        <table className="table table-hover align-middle mb-0">
                            <thead className="bg-light">
                                <tr>
                                    <th className="border-0 ps-4">Order #</th>
                                    <th className="border-0">Date</th>
                                    <th className="border-0">Customer</th>
                                    <th className="border-0">Product</th>
                                    <th className="border-0">Qty</th>
                                    <th className="border-0">Total</th>
                                    <th className="border-0">Status</th>
                                    <th className="border-0 text-end pe-4">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {orders.map(order => (
                                    <tr key={`${order.orderId}-${order.productName}`}>
                                        <td className="ps-4 fw-bold">{order.orderNumber}</td>
                                        <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                                        <td>{order.customerName}</td>
                                        <td>{order.productName}</td>
                                        <td>{order.quantity}</td>
                                        <td>Rp {order.subtotal.toLocaleString('id-ID')}</td>
                                        <td>
                                            <span className={`badge ${getStatusBadgeClass(order.status)}`}>
                                                {order.status}
                                            </span>
                                        </td>
                                        <td className="text-end pe-4">
                                            <div className="dropdown">
                                                <button className="btn btn-sm btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                                                    Update Status
                                                </button>
                                                <ul className="dropdown-menu">
                                                    <li><button className="dropdown-item" onClick={() => handleStatusUpdate(order.orderId, 'Processing')}>Processing</button></li>
                                                    <li><button className="dropdown-item" onClick={() => handleStatusUpdate(order.orderId, 'Shipped')}>Shipped</button></li>
                                                    <li><button className="dropdown-item" onClick={() => handleStatusUpdate(order.orderId, 'Delivered')}>Delivered</button></li>
                                                    <li><hr className="dropdown-divider" /></li>
                                                    <li><button className="dropdown-item text-danger" onClick={() => handleStatusUpdate(order.orderId, 'Cancelled')}>Cancelled</button></li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </div>
    );
}

function getStatusBadgeClass(status) {
    switch (status?.toLowerCase()) {
        case 'pending': return 'bg-warning text-dark';
        case 'processing': return 'bg-info text-white';
        case 'shipped': return 'bg-primary';
        case 'delivered': return 'bg-success';
        case 'cancelled': return 'bg-danger';
        default: return 'bg-secondary';
    }
}

export default SellerOrders;
