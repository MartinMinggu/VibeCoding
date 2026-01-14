import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Admin.css';

function UserManagement() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const user = JSON.parse(localStorage.getItem('user'));

    useEffect(() => {
        fetchUsers();
    }, []);

    const fetchUsers = async () => {
        try {
            const response = await fetch('/api/admin/users');
            if (response.ok) {
                const data = await response.json();
                setUsers(data);
            }
        } catch (error) {
            console.error('Error fetching users:', error);
        } finally {
            setLoading(false);
        }
    };

    const handlePromote = async (userId) => {
        if (!confirm('Promote this user to Admin?')) return;

        try {
            const response = await fetch(`/api/admin/promote/${userId}`, { method: 'POST' });
            const data = await response.json();
            if (response.ok) {
                fetchUsers();
                alert(data.message);
            } else {
                alert(data.message || 'Failed to promote user');
            }
        } catch (error) {
            console.error('Error promoting user:', error);
        }
    };

    const handleDemote = async (userId) => {
        if (!confirm('Remove Admin role from this user?')) return;

        try {
            const response = await fetch(`/api/admin/demote/${userId}`, { method: 'POST' });
            const data = await response.json();
            if (response.ok) {
                fetchUsers();
                alert(data.message);
            } else {
                alert(data.message || 'Failed to demote user');
            }
        } catch (error) {
            console.error('Error demoting user:', error);
        }
    };

    if (loading) return <div className="text-center py-5"><span className="spinner-border text-primary"></span></div>;

    return (
        <div className="container my-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h1 className="display-6 fw-bold">User Management</h1>
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb mb-0">
                            <li className="breadcrumb-item"><Link to="/admin">Admin</Link></li>
                            <li className="breadcrumb-item active">Users</li>
                        </ol>
                    </nav>
                </div>
            </div>

            {!user?.isSuperAdmin && (
                <div className="alert alert-warning">
                    <i className="fas fa-info-circle me-2"></i>
                    Only SuperAdmin can promote or demote users.
                </div>
            )}

            <div className="card shadow-sm">
                <div className="table-responsive">
                    <table className="table table-hover align-middle mb-0">
                        <thead className="bg-light">
                            <tr>
                                <th className="ps-4">User</th>
                                <th>Email</th>
                                <th>Roles</th>
                                <th>Type</th>
                                {user?.isSuperAdmin && <th className="text-end pe-4">Actions</th>}
                            </tr>
                        </thead>
                        <tbody>
                            {users.length === 0 ? (
                                <tr><td colSpan="5" className="text-center py-4 text-muted">No users found</td></tr>
                            ) : (
                                users.map(u => (
                                    <tr key={u.id}>
                                        <td className="ps-4">
                                            <div className="d-flex align-items-center">
                                                <div className="avatar-circle bg-primary text-white me-3">
                                                    {u.firstName?.charAt(0)}{u.lastName?.charAt(0)}
                                                </div>
                                                <div>
                                                    <div className="fw-bold">{u.firstName} {u.lastName}</div>
                                                    <small className="text-muted">{u.id.substring(0, 8)}...</small>
                                                </div>
                                            </div>
                                        </td>
                                        <td>{u.email}</td>
                                        <td>
                                            {u.roles?.map(r => (
                                                <span key={r} className={`badge me-1 ${r === 'SuperAdmin' ? 'bg-danger' :
                                                        r === 'Admin' ? 'bg-primary' :
                                                            r === 'Seller' ? 'bg-success' : 'bg-secondary'
                                                    }`}>{r}</span>
                                            ))}
                                        </td>
                                        <td>
                                            {u.isSeller && <span className="badge bg-success-subtle text-success">Seller</span>}
                                            {!u.isSeller && <span className="badge bg-info-subtle text-info">Customer</span>}
                                        </td>
                                        {user?.isSuperAdmin && (
                                            <td className="text-end pe-4">
                                                {!u.roles?.includes('SuperAdmin') && (
                                                    <>
                                                        {u.roles?.includes('Admin') ? (
                                                            <button
                                                                className="btn btn-sm btn-outline-warning"
                                                                onClick={() => handleDemote(u.id)}
                                                            >
                                                                <i className="fas fa-arrow-down me-1"></i>Demote
                                                            </button>
                                                        ) : (
                                                            <button
                                                                className="btn btn-sm btn-outline-primary"
                                                                onClick={() => handlePromote(u.id)}
                                                            >
                                                                <i className="fas fa-arrow-up me-1"></i>Promote
                                                            </button>
                                                        )}
                                                    </>
                                                )}
                                                {u.roles?.includes('SuperAdmin') && (
                                                    <span className="badge bg-danger">Protected</span>
                                                )}
                                            </td>
                                        )}
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

export default UserManagement;
