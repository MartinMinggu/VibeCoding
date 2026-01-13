import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Auth.css';

function Login() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        rememberMe: false
    });
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        // TODO: Implement login API call
        console.log('Login:', formData);

        // Simulate success for demo
        alert('Login successful! (Demo mode)');
        // navigate('/');
    };

    return (
        <div className="auth-page">
            <div className="container">
                <div className="auth-card">
                    <div className="text-center mb-4">
                        <h1 className="display-6 fw-bold mb-2">Welcome Back</h1>
                        <p className="text-muted">Login to access your account</p>
                    </div>

                    {error && (
                        <div className="alert alert-danger" role="alert">
                            <i className="fas fa-exclamation-circle me-2"></i>
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
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
                                placeholder="Enter your password"
                                required
                            />
                        </div>

                        <div className="mb-4">
                            <div className="form-check">
                                <input
                                    className="form-check-input"
                                    type="checkbox"
                                    id="rememberMe"
                                    checked={formData.rememberMe}
                                    onChange={(e) => setFormData({ ...formData, rememberMe: e.target.checked })}
                                />
                                <label className="form-check-label" htmlFor="rememberMe">
                                    Remember me
                                </label>
                            </div>
                        </div>

                        <div className="d-grid mb-3">
                            <button type="submit" className="btn btn-primary btn-lg">
                                <i className="fas fa-sign-in-alt me-2"></i>Login
                            </button>
                        </div>

                        <div className="text-center">
                            <p className="text-muted mb-2">
                                <a href="#" className="text-primary">Forgot password?</a>
                            </p>
                            <p className="text-muted mb-0">
                                Don't have an account?
                                <Link to="/register" className="text-primary fw-semibold ms-2">
                                    Register now
                                </Link>
                            </p>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Login;
