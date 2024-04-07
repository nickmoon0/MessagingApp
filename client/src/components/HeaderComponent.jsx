import React from 'react';
import { Layout } from 'antd';
import { useUser } from '../context/UserContext';

const { Header } = Layout;

const HeaderComponent = () => {
    const { user } = useUser();
    return (
    <Header style={{
        position: 'fixed',
        zIndex: 1,
        width: '100%',
        height: '40px',
        backgroundImage: 'linear-gradient(to right,  #33ccff, #0099ff)',
        color: "white",
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '0 20px',
    }}>
        
         <div style={{  textAlign: 'left', fontWeight: 'bold', fontSize: '33px' }}>Talk.</div>
         <div style={{ display: 'flex', alignItems: 'center' }}>
            <div style={{ fontWeight: '550', fontSize: '18px' }}>{user?.username}</div>
        </div>

          

    </Header>
);
};

export default HeaderComponent;
