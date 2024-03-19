import React from 'react';
import { Menu, Divider } from 'antd';
import { HomeOutlined, MessageOutlined, TeamOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom'; 
import { useSelectedTab } from '../context/SelectedTabContext';


function VerticalMenu() {
  const navigate = useNavigate(); 
  const { setSelectedTab } = useSelectedTab();


  const handleClick = (e) => {
    if (e.key === 'home') 
    {
      navigate('/home');
    } 
    else if (e.key === 'friends') 
    {
      setSelectedTab('Friends');
      navigate('/friends');
    }
  };

  return (
    

    <Menu
      mode="vertical"
      onClick={handleClick}
      className="custom-menu" 
      style={{ 
        width: 245, 
        borderRight: 0, 
        fontWeight: 'bold' }} // Adjust width as needed, remove right border
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
