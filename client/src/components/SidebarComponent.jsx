import React from 'react';
import { Layout, Divider, Input, Avatar, Card } from 'antd';
import MenuBar from './MenuBar';
import { SearchOutlined, UserOutlined } from '@ant-design/icons';
import { useUser } from '../context/UserContext';

const { Sider } = Layout;
const { Search } = Input;


const SidebarComponent = () => {
    const { user } = useUser();

return (
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
    
    
    <div style={{marginTop: '4px'}}>
    <MenuBar />
    </div>
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
  
        <div style={{ margin: '15px auto', width: '90%' }}>
          <Card 
            bordered
            style={{ height: '450px',
            borderColor: '#338EF7', 
            borderWidth: '1px',
            borderStyle: 'solid' }} 
            >
            Chat Boxes
          </Card>
        </div>

        <div
        style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'start', // Aligns items to the start of the container
          padding: '0 15px', // Add some padding inside the sider at the bottom
          marginBottom: '50px', // Add some margin at the bottom
        }}
      >
        <Avatar style={{ marginRight: 9, marginBottom: 3, backgroundColor: '#338EF7', color: 'white' }} icon={<UserOutlined />} /> {/* Adjust the src accordingly */}
        <div style={{ fontWeight: '550', fontSize: '16px' }}>{user?.username}</div>
      </div>

  </Sider>
);

};
export default SidebarComponent;
