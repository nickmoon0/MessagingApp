import React from 'react';
import { BrowserRouter as Router, Routes, Route, useLocation, useNavigate } from 'react-router-dom';
import { Layout } from 'antd';
import LoginPage from './pages/login/loginPage';
import RegisterPage from './pages/register/registerPage';
import Friends from './pages/friend/Friends';
import Home from './pages/DashBoard/Home';
import HeaderComponent from './components/HeaderComponent';
import SidebarComponent from './components/SidebarComponent';
import './styles/center.css';
import { UserProvider } from './context/UserContext'; 
import { SelectedTabProvider } from './context/SelectedTabContext';


const { Content } = Layout;

const AppContent = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const isDashboardRoute = ['/home', '/friends'].includes(location.pathname);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/'); 
  };

  return (
    <Layout>
      {isDashboardRoute && <HeaderComponent handleLogout={handleLogout} />}
      <Layout style={{ paddingTop: '55px', backgroundColor: '#FFFFFF' }}>
        {isDashboardRoute && <SidebarComponent />}
        <Content style={{ marginTop: 0, backgroundColor: '#FFFFFF' }}>
          <Routes>
            <Route path="/register" element={<div className='body'><RegisterPage /></div>} />
            <Route path="/" element={<div className='body'><LoginPage /></div>} />
            <Route path="/home" element={<Home />} />
            <Route path="/friends" element={<Friends />} />
          </Routes>
        </Content>
      </Layout>
    </Layout>
  );
};

function App() {
  return (
    <Router>
      <UserProvider>
        <SelectedTabProvider>
          <AppContent />
        </SelectedTabProvider>
      </UserProvider>
    </Router>
  );
}

export default App;
