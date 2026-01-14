import React from 'react';
import { Link } from 'react-router-dom';
import './Admin.css';

function AdminDashboard() {
    const user = JSON.parse(localStorage.getItem('user'));

    return (
        <div className="container my-5">
            <div className="mb-4">
                <h1 className="display-5 fw-bold">Admin Dashboard</h1>
                <p className="text-muted">Welcome, {user?.firstName}! Manage your e-commerce platform here.</p>
            </div>

            <div className="row g-4">
                <div className="col-md-6 col-lg-4">
                    <div className="card admin-card h-100 border-0 shadow-sm">
                        <div className="card-body text-center py-5">
                            <div className="rounded-circle bg-primary bg-opacity-10 p-4 d-inline-block mb-3">
                                <i className="fas fa-tags fa-2x text-primary"></i>
                            </div>
                            <h5 className="fw-bold">Categories</h5>
                            <p className="text-muted small">Manage product categories</p>
                            <Link to="/admin/categories" className="btn btn-primary">
                                <i className="fas fa-arrow-right me-2"></i>Manage
                            </Link>
                        </div>
                    </div>
                </div>

                {user?.isSuperAdmin && (
                    <div className="col-md-6 col-lg-4">
                        <div className="card admin-card h-100 border-0 shadow-sm">
                            <div className="card-body text-center py-5">
                                <div className="rounded-circle bg-success bg-opacity-10 p-4 d-inline-block mb-3">
                                    <i className="fas fa-users fa-2x text-success"></i>
                                </div>
                                <h5 className="fw-bold">Users</h5>
                                <p className="text-muted small">Manage users and admins</p>
                                <Link to="/admin/users" className="btn btn-success">
                                    <i className="fas fa-arrow-right me-2"></i>Manage
                                </Link>
                            </div>
                        </div>
                    </div>
                )}

                <div className="col-md-6 col-lg-4">
                    <div className="card admin-card h-100 border-0 shadow-sm">
                        <div className="card-body text-center py-5">
                            <div className="rounded-circle bg-info bg-opacity-10 p-4 d-inline-block mb-3">
                                <i className="fas fa-database fa-2x text-info"></i>
                            </div>
                            <h5 className="fw-bold">Database</h5>
                            <p className="text-muted small">Clear all data for testing</p>
                            <button
                                className="btn btn-outline-danger"
                                onClick={async () => {
                                    if (confirm('⚠️ This will DELETE ALL DATA! Are you sure?')) {
                                        try {
                                            const res = await fetch('/api/database/clear', { method: 'POST' });
                                            const data = await res.json();
                                            alert(data.message);
                                        } catch (e) {
                                            alert('Failed to clear database');
                                        }
                                    }
                                }}
                            >
                                <i className="fas fa-trash me-2"></i>Clear DB
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div className="mt-5">
                <div className="card border-0 shadow-sm">
                    <div className="card-header bg-white py-3">
                        <h5 className="mb-0 fw-bold">Your Permissions</h5>
                    </div>
                    <div className="card-body">
                        <div className="row">
                            <div className="col-md-6">
                                <ul className="list-unstyled mb-0">
                                    <li className="mb-2">
                                        <i className="fas fa-check text-success me-2"></i>
                                        View all users
                                    </li>
                                    <li className="mb-2">
                                        <i className="fas fa-check text-success me-2"></i>
                                        Manage categories (CRUD)
                                    </li>
                                </ul>
                            </div>
                            <div className="col-md-6">
                                <ul className="list-unstyled mb-0">
                                    <li className="mb-2">
                                        {user?.isSuperAdmin ? (
                                            <><i className="fas fa-check text-success me-2"></i>Promote/Demote admins</>
                                        ) : (
                                            <><i className="fas fa-times text-danger me-2"></i>Promote/Demote admins (SuperAdmin only)</>
                                        )}
                                    </li>
                                    <li className="mb-2">
                                        <i className="fas fa-check text-success me-2"></i>
                                        Clear database
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default AdminDashboard;
