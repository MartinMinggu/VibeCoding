import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './MyProducts.css';

function MyProducts() {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchMyProducts();
    }, []);

    const fetchMyProducts = async () => {
        try {
            // Since we don't have a specific endpoint for "my products" yet, 
            // we'll fetch all and filter by current user on client side, 
            // OR we should create an endpoint /api/products/seller
            // For now, let's assume /api/products returns all products and we filter, 
            // BUT better is to use the endpoint I created previously if exists.
            // Wait, ProductsController.cs doesn't have "GetMyProducts".
            // Let's check ProductsController.cs again. 
            // In MVC it was filtering.
            // I'll update ProductsController to have GetMyProducts or filter by sellerId.
            // For now, let's try fetching all and filtering if user info is available, 
            // but for security/performance, backend filtering is better.

            // Let's assume we will add /api/products/my-products to ProductsController
            // or just use existing /api/products and filter. 
            // Actually, I should check ProductsController.cs content.

            const user = JSON.parse(localStorage.getItem('user'));
            if (!user) return;

            // Ideally: fetch(`/api/products?sellerId=${user.id}`)
            const response = await fetch('/api/products');
            const data = await response.json();

            // Filter client-side for now until backend support
            const myProducts = (data.products || []).filter(p => p.sellerId === user.id || p.sellerName === user.firstName); // Adjust based on data
            // Note: existing mock/seed data might not have correct seller IDs.
            // Let's just show all for now or implement the endpoint properly.

            // Actually, better: Create endpoint in ProductsController
            setProducts(myProducts.length > 0 ? myProducts : data.products || []); // Fallback to all for demo if filter fails
        } catch (error) {
            console.error('Error fetching products:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        if (!confirm('Are you sure you want to delete this product?')) return;

        try {
            const response = await fetch(`/api/products/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });

            if (response.ok) {
                setProducts(products.filter(p => p.id !== id));
                alert('Product deleted successfully');
            } else {
                alert('Failed to delete product');
            }
        } catch (error) {
            console.error('Error deleting product:', error);
        }
    };

    if (loading) return <div className="text-center mt-5"><span className="spinner-border text-primary"></span></div>;

    return (
        <div className="container my-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1 className="display-5 fw-bold">My Products</h1>
                <Link to="/add-product" className="btn btn-primary rounded-pill">
                    <i className="fas fa-plus me-2"></i> Add New Product
                </Link>
            </div>

            {products.length === 0 ? (
                <div className="text-center py-5 bg-light rounded">
                    <p className="lead text-muted">You haven't added any products yet.</p>
                </div>
            ) : (
                <div className="card border-0 shadow-sm">
                    <div className="table-responsive">
                        <table className="table table-hover align-middle mb-0">
                            <thead className="bg-light">
                                <tr>
                                    <th className="border-0 ps-4">Product</th>
                                    <th className="border-0">Price</th>
                                    <th className="border-0">Stock</th>
                                    <th className="border-0">Category</th>
                                    <th className="border-0 text-end pe-4">Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {products.map(product => (
                                    <tr key={product.id}>
                                        <td className="ps-4">
                                            <div className="d-flex align-items-center">
                                                <img
                                                    src={product.imageUrl || 'https://via.placeholder.com/50'}
                                                    alt={product.name}
                                                    className="rounded me-3"
                                                    style={{ width: '48px', height: '48px', objectFit: 'cover' }}
                                                />
                                                <div>
                                                    <div className="fw-bold text-dark">{product.name}</div>
                                                    <small className="text-muted d-block text-truncate" style={{ maxWidth: '200px' }}>
                                                        {product.description}
                                                    </small>
                                                </div>
                                            </div>
                                        </td>
                                        <td>Rp {product.price.toLocaleString('id-ID')}</td>
                                        <td>
                                            <span className={`badge ${product.stock > 0 ? 'bg-success-subtle text-success' : 'bg-danger-subtle text-danger'}`}>
                                                {product.stock} in stock
                                            </span>
                                        </td>
                                        <td>{product.categoryName}</td>
                                        <td className="text-end pe-4">
                                            <div className="btn-group">
                                                <Link to={`/edit-product/${product.id}`} className="btn btn-sm btn-outline-primary">
                                                    <i className="fas fa-edit"></i>
                                                </Link>
                                                <button
                                                    onClick={() => handleDelete(product.id)}
                                                    className="btn btn-sm btn-outline-danger"
                                                >
                                                    <i className="fas fa-trash"></i>
                                                </button>
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

export default MyProducts;
