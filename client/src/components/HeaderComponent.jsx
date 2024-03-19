import React from 'react';
import { Layout, Button } from 'antd';
import { LogoutOutlined, InboxOutlined, QuestionOutlined } from '@ant-design/icons';

const { Header } = Layout;

const HeaderComponent = ({ handleLogout }) => (
    <Header style={{
        position: 'fixed',
        zIndex: 1,
        width: '100%',
        height: '50px',
        backgroundImage: 'linear-gradient(to left,  #33ccff, #0099ff)',
        color: "white",
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '0 20px',
    }}>
    <div style={{ textAlign: 'left', fontWeight: 'bold', fontSize: '35px' }}>Talk.</div>
        <div style={{ display: 'flex', alignItems: 'center' }}>
        <Button 
            icon={<InboxOutlined />}
            shape="circle" 
            style={{ 
                marginRight: '10px', 
                background: 'white', 
                color: '#27272A', 
                fontSize: '15px', 
                border: 'none' }} />
        <Button 
            icon={<QuestionOutlined />} 
            shape="circle" 
            style={{ 
                marginRight: '10px', 
                background: 'white', 
                color: '#27272A', 
                fontSize: '15px', 
                border: 'none' }} />
        <Button 
            onClick={handleLogout} 
            icon={<LogoutOutlined />} 
            shape='circle' 
            style={{ 
                background: 'white', 
                color: '#27272A', 
                fontSize: '15px', 
                border: 'none' }} />
    </div>
    </Header>
);

export default HeaderComponent;
