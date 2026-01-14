import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        // Check if user is logged in
        const checkUser = () => {
            const userData = localStorage.getItem('user');
            if (userData) {
                setUser(JSON.parse(userData));
            } else {
                setUser(null);
            }
        };

        checkUser();

        // Listen for storage changes (from other tabs or custom events)
        window.addEventListener('storage', checkUser);
        window.addEventListener('userLogin', checkUser);

        return () => {
            window.removeEventListener('storage', checkUser);
            window.removeEventListener('userLogin', checkUser);
        };
    }, []);

    const handleLogout = () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        setUser(null);
        navigate('/');
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm fixed-top">
            <div className="container">
                <Link to="/" className="navbar-brand fw-bold">
                    <span className="text-gradient">ECommerce</span>App
                </Link>

                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ms-auto align-items-center">
                        <li className="nav-item">
                            <Link to="/" className="nav-link fw-semibold">
                                <i className="fas fa-home me-1"></i> Home
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/products" className="nav-link fw-semibold">
                                <i className="fas fa-th me-1"></i> Products
                            </Link>
                        </li>

                        {user ? (
                            <>
                                {user.isSeller && (
                                    <li className="nav-item">
                                        <Link to="/add-product" className="nav-link fw-semibold">
                                            <i className="fas fa-plus-circle me-1"></i> Add Product
                                        </Link>
                                    </li>
                                )}
                                <li className="nav-item">
                                    <a href="/cart" className="nav-link fw-semibold position-relative">
                                        <i className="fas fa-shopping-cart me-1"></i> Cart
                                    </a>
                                </li>
                                <li className="nav-item dropdown ms-3">
                                    <a className="nav-link dropdown-toggle fw-semibold" href="#" id="userDropdown" role="button" data-bs-toggle="dropdown">
                                        <i className="fas fa-user-circle me-1"></i> {user.firstName}
                                    </a>
                                    <ul className="dropdown-menu dropdown-menu-end">
                                        <li><span className="dropdown-item-text"><small className="text-muted">{user.email}</small></span></li>
                                        <li><hr className="dropdown-divider" /></li>
                                        {user.isSeller && (
                                            <>
                                                <li><Link className="dropdown-item" to="/seller/products"><i className="fas fa-box me-2"></i>My Products</Link></li>
                                                <li><Link className="dropdown-item" to="/seller/orders"><i className="fas fa-clipboard-list me-2"></i>Manage Orders</Link></li>
                                                <li><hr className="dropdown-divider" /></li>
                                            </>
                                        )}
                                        {user.isAdmin && (
                                            <>
                                                <li><Link className="dropdown-item text-primary fw-bold" to="/admin"><i className="fas fa-shield-alt me-2"></i>Admin Panel</Link></li>
                                                <li><hr className="dropdown-divider" /></li>
                                            </>
                                        )}
                                        <li><Link className="dropdown-item" to="/orders"><i className="fas fa-shopping-bag me-2"></i>My Orders</Link></li>
                                        <li><hr className="dropdown-divider" />
                                        </li><li><button onClick={handleLogout} className="dropdown-item text-danger"><i className="fas fa-sign-out-alt me-2"></i>Logout</button></li>
                                    </ul>
                                </li>
                            </>
                        ) : (
                            <>
                                <li className="nav-item ms-3">
                                    <Link to="/login" className="btn btn-outline-primary rounded-pill px-3">
                                        Login
                                    </Link>
                                </li>
                                <li className="nav-item ms-2">
                                    <Link to="/register" className="btn btn-primary rounded-pill px-3">
                                        Register
                                    </Link>
                                </li>
                            </>
                        )}
                    </ul>
                </div>
            </div>
        </nav>
    );
}

export default Navbar;
