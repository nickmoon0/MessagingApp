// src/components/SidebarComponent.js
import React from 'react';
import { Layout, Divider, Input } from 'antd';
import MenuBar from './MenuBar';
import { SearchOutlined } from '@ant-design/icons';

const { Sider } = Layout;
const { Search } = Input;

const SidebarComponent = () => (
  <Sider 
    width={250} 
    style={{ 
        overflow: 'auto', 
        height: 'calc(100vh - 50px)', 
        position: 'fixed', 
        left: 0, 
        top: 50, 
        background: '#FFFFFF', 
        borderRight: '2px solid #E4E4E7' }}>

    <MenuBar />
    <div style={{ width: '90%', margin: '-10px auto' }}>
      <Divider style={{ border: '1px solid #D4D4D8' }} />
    </div>

    <Search 
        placeholder="Search..." 
        onSearch={value => console.log(value)} 
        style={{ 
            marginLeft: '13px', 
            width: '90%', 
            borderRadius: '10px' }} 
            allowClear 
            prefix={<SearchOutlined 
            style={{ marginTop: '1px' }} />} />
  </Sider>
);

export default SidebarComponent;
