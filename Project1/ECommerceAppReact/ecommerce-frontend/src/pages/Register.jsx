import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Auth.css';

function Register() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: '',
        firstName: '',
        lastName: '',
        registerAsSeller: false
    });
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match!');
            return;
        }

        if (formData.password.length < 6) {
            setError('Password must be at least 6 characters');
            return;
        }

        // TODO: Implement register API call
        console.log('Register:', formData);

        // Simulate success
        alert('Registration successful! Please login.');
        navigate('/login');
    };

    return (
        <div className="auth-page">
            <div className="container">
                <div className="auth-card">
                    <div className="text-center mb-4">
                        <h1 className="display-6 fw-bold mb-2">Create Account</h1>
                        <p className="text-muted">Join our community and start shopping!</p>
                    </div>

                    {error && (
                        <div className="alert alert-danger" role="alert">
                            <i className="fas fa-exclamation-circle me-2"></i>
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <div className="row g-3">
                            <div className="col-md-6">
                                <label className="form-label fw-semibold">
                                    <i className="fas fa-user me-2"></i>First Name
                                </label>
                                <input
                                    type="text"
                                    className="form-control form-control-lg"
                                    value={formData.firstName}
                                    onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                                    placeholder="Enter your first name"
                                    required
                                />
                            </div>

                            <div className="col-md-6">
                                <label className="form-label fw-semibold">
                                    <i className="fas fa-user me-2"></i>Last Name
                                </label>
                                <input
                                    type="text"
                                    className="form-control form-control-lg"
                                    value={formData.lastName}
                                    onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                                    placeholder="Enter your last name"
                                    required
                                />
                            </div>
                        </div>

                        <div className="mb-3 mt-3">
                            <label className="form-label fw-semibold">
                                <i className="fas fa-envelope me-2"></i>Email Address
                            </label>
                            <input
                                type="email"
                                className="form-control form-control-lg"
                                value={formData.email}
                                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                                placeholder="your.email@example.com"
                                required
                            />
                        </div>

                        <div className="mb-3">
                            <label className="form-label fw-semibold">
                                <i className="fas fa-lock me-2"></i>Password
                            </label>
                            <input
                                type="password"
                                className="form-control form-control-lg"
                                value={formData.password}
                                onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                                placeholder="At least 6 characters"
                                required
                            />
                            <small className="text-muted">Must be at least 6 characters</small>
                        </div>

                        <div className="mb-3">
                            <label className="form-label fw-semibold">
                                <i className="fas fa-lock me-2"></i>Confirm Password
                            </label>
                            <input
                                type="password"
                                className="form-control form-control-lg"
                                value={formData.confirmPassword}
                                onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
                                placeholder="Re-enter your password"
                                required
                            />
                        </div>

                        <div className="mb-4">
                            <div className="form-check">
                                <input
                                    className="form-check-input"
                                    type="checkbox"
                                    id="sellerCheck"
                                    checked={formData.registerAsSeller}
                                    onChange={(e) => setFormData({ ...formData, registerAsSeller: e.target.checked })}
                                />
                                <label className="form-check-label" htmlFor="sellerCheck">
                                    <i className="fas fa-store me-2"></i>
                                    Register as a <strong>Seller</strong>
                                    <small className="d-block text-muted">You'll be able to list and sell your own products</small>
                                </label>
                            </div>
                        </div>

                        <div className="d-grid mb-3">
                            <button type="submit" className="btn btn-primary btn-lg">
                                <i className="fas fa-user-plus me-2"></i>Create Account
                            </button>
                        </div>

                        <div className="text-center">
                            <p className="text-muted mb-0">
                                Already have an account?
                                <Link to="/login" className="text-primary fw-semibold ms-2">
                                    Login here
                                </Link>
                            </p>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Register;
