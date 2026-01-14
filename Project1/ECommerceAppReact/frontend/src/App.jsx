import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Products from './pages/Products';
import ProductDetail from './pages/ProductDetail';
import Cart from './pages/Cart';
import Checkout from './pages/Checkout';
import Orders from './pages/Orders';
import MyProducts from './pages/seller/MyProducts';
import SellerOrders from './pages/seller/SellerOrders';
import AdminDashboard from './pages/admin/AdminDashboard';
import CategoryManagement from './pages/admin/CategoryManagement';
import UserManagement from './pages/admin/UserManagement';
import Login from './pages/Login';
import Register from './pages/Register';
import AddProduct from './pages/AddProduct';
import EditProduct from './pages/EditProduct';
import Navbar from './components/Layout/Navbar';
import './App.css';

function App() {
    return (
        <BrowserRouter>
            <div className="app">
                <Navbar />
                <main className="main-content">
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/products" element={<Products />} />
                        <Route path="/product/:id" element={<ProductDetail />} />
                        <Route path="/cart" element={<Cart />} />
                        <Route path="/checkout" element={<Checkout />} />
                        <Route path="/orders" element={<Orders />} />
                        <Route path="/seller/products" element={<MyProducts />} />
                        <Route path="/seller/orders" element={<SellerOrders />} />
                        <Route path="/admin" element={<AdminDashboard />} />
                        <Route path="/admin/categories" element={<CategoryManagement />} />
                        <Route path="/admin/users" element={<UserManagement />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/add-product" element={<AddProduct />} />
                        <Route path="/edit-product/:id" element={<EditProduct />} />
                    </Routes>
                </main>
            </div>
        </BrowserRouter>
    );
}

export default App;
