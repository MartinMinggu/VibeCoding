import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Admin.css';

function CategoryManagement() {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showForm, setShowForm] = useState(false);
    const [editingCategory, setEditingCategory] = useState(null);
    const [formData, setFormData] = useState({ name: '', description: '' });

    useEffect(() => {
        fetchCategories();
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
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const url = editingCategory
            ? `/api/category/${editingCategory.id}`
            : '/api/category';
        const method = editingCategory ? 'PUT' : 'POST';
        const token = localStorage.getItem('token');

        try {
            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                fetchCategories();
                setShowForm(false);
                setEditingCategory(null);
                setFormData({ name: '', description: '' });
                alert(editingCategory ? 'Category updated!' : 'Category created!');
            } else {
                alert('Failed to save category');
            }
        } catch (error) {
            console.error('Error saving category:', error);
        }
    };

    const handleEdit = (category) => {
        setEditingCategory(category);
        setFormData({ name: category.name, description: category.description || '' });
        setShowForm(true);
    };

    const handleDelete = async (id) => {
        if (!confirm('Are you sure you want to delete this category?')) return;
        const token = localStorage.getItem('token');

        try {
            const response = await fetch(`/api/category/${id}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (response.ok) {
                fetchCategories();
                alert('Category deleted!');
            } else {
                alert('Failed to delete category');
            }
        } catch (error) {
            console.error('Error deleting category:', error);
        }
    };

    if (loading) return <div className="text-center py-5"><span className="spinner-border text-primary"></span></div>;

    return (
        <div className="container my-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h1 className="display-6 fw-bold">Category Management</h1>
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb mb-0">
                            <li className="breadcrumb-item"><Link to="/admin">Admin</Link></li>
                            <li className="breadcrumb-item active">Categories</li>
                        </ol>
                    </nav>
                </div>
                <button
                    className="btn btn-primary"
                    onClick={() => { setShowForm(true); setEditingCategory(null); setFormData({ name: '', description: '' }); }}
                >
                    <i className="fas fa-plus me-2"></i>Add Category
                </button>
            </div>

            {showForm && (
                <div className="card mb-4 shadow-sm">
                    <div className="card-header bg-primary text-white">
                        <h5 className="mb-0">{editingCategory ? 'Edit Category' : 'Add New Category'}</h5>
                    </div>
                    <div className="card-body">
                        <form onSubmit={handleSubmit}>
                            <div className="row">
                                <div className="col-md-6 mb-3">
                                    <label className="form-label">Category Name</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        value={formData.name}
                                        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                        required
                                    />
                                </div>
                                <div className="col-md-6 mb-3">
                                    <label className="form-label">Description</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        value={formData.description}
                                        onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                                    />
                                </div>
                            </div>
                            <div className="d-flex gap-2">
                                <button type="submit" className="btn btn-primary">
                                    <i className="fas fa-save me-2"></i>Save
                                </button>
                                <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>
                                    Cancel
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            <div className="card shadow-sm">
                <div className="table-responsive">
                    <table className="table table-hover align-middle mb-0">
                        <thead className="bg-light">
                            <tr>
                                <th className="ps-4">ID</th>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Products</th>
                                <th className="text-end pe-4">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {categories.length === 0 ? (
                                <tr><td colSpan="5" className="text-center py-4 text-muted">No categories found</td></tr>
                            ) : (
                                categories.map(cat => (
                                    <tr key={cat.id}>
                                        <td className="ps-4">{cat.id}</td>
                                        <td className="fw-bold">{cat.name}</td>
                                        <td className="text-muted">{cat.description || '-'}</td>
                                        <td><span className="badge bg-secondary">{cat.productCount || 0}</span></td>
                                        <td className="text-end pe-4">
                                            <button className="btn btn-sm btn-outline-primary me-2" onClick={() => handleEdit(cat)}>
                                                <i className="fas fa-edit"></i>
                                            </button>
                                            <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(cat.id)}>
                                                <i className="fas fa-trash"></i>
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}

export default CategoryManagement;
