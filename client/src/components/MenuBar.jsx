import React from 'react';
import { Menu } from 'antd';
import { HomeOutlined, MessageOutlined, TeamOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom'; // Import useNavigate hook

function VerticalMenu() {
  const navigate = useNavigate(); // Instantiate navigate function

  // Function to handle menu item clicks
  const handleClick = (e) => {
    if (e.key === 'home') {
      navigate('/home');
    } else if (e.key === 'friends') {
      navigate('/friends');
    }
  };

  return (
    <Menu
      mode="vertical"
      onClick={handleClick}
      className="custom-menu" 
      style={{ width: 245, borderRight: 0, marginTop:'10px', fontWeight: 'bold' }} // Adjust width as needed, remove right border
    >
      <Menu.Item key="home" icon={<HomeOutlined />}>
        Home
      </Menu.Item>
      <Menu.Item key="friends" icon={<TeamOutlined />}>
        Friends
      </Menu.Item>
      <Menu.Item key="newChat" icon={<MessageOutlined />}>
        New Chat
      </Menu.Item>
    </Menu>
  );
}

export default VerticalMenu;
