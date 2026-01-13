import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
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
                        <li className="nav-item">
                            <a href="/cart" className="nav-link fw-semibold position-relative">
                                <i className="fas fa-shopping-cart me-1"></i> Cart
                                <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" style={{ fontSize: '0.65rem' }}>
                                    3
                                </span>
                            </a>
                        </li>
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
                    </ul>
                </div>
            </div>
        </nav>
    );
}

export default Navbar;
