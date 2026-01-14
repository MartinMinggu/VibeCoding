import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import './ProductForm.css';

function EditProduct() {
    const navigate = useNavigate();
    const { id } = useParams();
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        price: '',
        stock: '',
        imageUrl: '',
        categoryId: 1
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [loadingProduct, setLoadingProduct] = useState(true);

    const [categories, setCategories] = useState([]);

    useEffect(() => {
        fetchCategories();
        loadProduct();
    }, [id]);

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



    const loadProduct = async () => {
        try {
            const response = await fetch(`/api/products/${id}`);
            if (response.ok) {
                const product = await response.json();
                setFormData({
                    name: product.name,
                    description: product.description,
                    price: product.price,
                    stock: product.stock,
                    imageUrl: product.imageUrl,
                    categoryId: product.categoryId || 1
                });
            } else {
                setError('Product not found');
            }
        } catch (err) {
            setError('Error loading product: ' + err.message);
        } finally {
            setLoadingProduct(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            const response = await fetch(`/api/products/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify({
                    name: formData.name,
                    description: formData.description,
                    price: parseFloat(formData.price),
                    stock: parseInt(formData.stock),
                    imageUrl: formData.imageUrl,
                    categoryId: formData.categoryId
                })
            });

            if (response.ok) {
                alert('Product updated successfully!');
                navigate('/seller/products');
            } else {
                const data = await response.json();
                setError(data.message || 'Failed to update product');
            }
        } catch (err) {
            setError('Error updating product: ' + err.message);
        } finally {
            setLoading(false);
        }
    };

    if (loadingProduct) {
        return (
            <div className="container py-5 text-center">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
                <p className="mt-3">Loading product...</p>
            </div>
        );
    }

    return (
        <div className="product-form-page">
            <div className="container py-5">
                <div className="row justify-content-center">
                    <div className="col-lg-8">
                        <div className="card shadow-sm border-0">
                            <div className="card-header bg-warning text-dark py-3">
                                <h3 className="mb-0">
                                    <i className="fas fa-edit me-2"></i>
                                    Edit Product
                                </h3>
                            </div>

                            <div className="card-body p-4">
                                {error && (
                                    <div className="alert alert-danger" role="alert">
                                        <i className="fas fa-exclamation-circle me-2"></i>
                                        {error}
                                    </div>
                                )}

                                <form onSubmit={handleSubmit}>
                                    <div className="mb-3">
                                        <label className="form-label fw-semibold">
                                            <i className="fas fa-tag me-2"></i>Product Name
                                        </label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={formData.name}
                                            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                            placeholder="Enter product name"
                                            required
                                        />
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label fw-semibold">
                                            <i className="fas fa-align-left me-2"></i>Description
                                        </label>
                                        <textarea
                                            className="form-control"
                                            rows="4"
                                            value={formData.description}
                                            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                                            placeholder="Enter product description"
                                            required
                                        />
                                    </div>

                                    <div className="row">
                                        <div className="col-md-6 mb-3">
                                            <label className="form-label fw-semibold">
                                                <i className="fas fa-dollar-sign me-2"></i>Price (Rp)
                                            </label>
                                            <input
                                                type="number"
                                                className="form-control"
                                                value={formData.price}
                                                onChange={(e) => setFormData({ ...formData, price: e.target.value })}
                                                placeholder="0"
                                                min="0"
                                                step="1000"
                                                required
                                            />
                                        </div>

                                        <div className="col-md-6 mb-3">
                                            <label className="form-label fw-semibold">
                                                <i className="fas fa-boxes me-2"></i>Stock
                                            </label>
                                            <input
                                                type="number"
                                                className="form-control"
                                                value={formData.stock}
                                                onChange={(e) => setFormData({ ...formData, stock: e.target.value })}
                                                placeholder="0"
                                                min="0"
                                                required
                                            />
                                        </div>
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label fw-semibold">
                                            <i className="fas fa-image me-2"></i>Image URL
                                        </label>
                                        <input
                                            type="url"
                                            className="form-control"
                                            value={formData.imageUrl}
                                            onChange={(e) => setFormData({ ...formData, imageUrl: e.target.value })}
                                            placeholder="https://example.com/image.jpg"
                                            required
                                        />
                                        {formData.imageUrl && (
                                            <div className="mt-2">
                                                <img
                                                    src={formData.imageUrl}
                                                    alt="Preview"
                                                    className="img-thumbnail"
                                                    style={{ maxWidth: '200px', maxHeight: '200px' }}
                                                    onError={(e) => e.target.style.display = 'none'}
                                                />
                                            </div>
                                        )}
                                    </div>

                                    <div className="mb-4">
                                        <label className="form-label fw-semibold">
                                            <i className="fas fa-list me-2"></i>Category
                                        </label>
                                        <select
                                            className="form-select"
                                            value={formData.categoryId}
                                            onChange={(e) => setFormData({ ...formData, categoryId: parseInt(e.target.value) })}
                                            required
                                        >
                                            {categories.map(cat => (
                                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                                            ))}
                                        </select>
                                    </div>

                                    <div className="d-flex gap-2">
                                        <button
                                            type="submit"
                                            className="btn btn-warning px-4"
                                            disabled={loading}
                                        >
                                            {loading ? (
                                                <>
                                                    <span className="spinner-border spinner-border-sm me-2"></span>
                                                    Updating...
                                                </>
                                            ) : (
                                                <>
                                                    <i className="fas fa-save me-2"></i>
                                                    Update Product
                                                </>
                                            )}
                                        </button>
                                        <button
                                            type="button"
                                            className="btn btn-outline-secondary px-4"
                                            onClick={() => navigate('/seller/products')}
                                        >
                                            <i className="fas fa-times me-2"></i>
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default EditProduct;
