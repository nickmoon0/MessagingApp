import React, { useState } from 'react';
import { Input, Button } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import SearchModal from '../../components/SearchModal';
import UserCard from '../../components/UserCard';
import useUserSearch from '../../hooks/useUserSearch';
import './AddFriend.css';

function UserSearch({ onRequestSent }) {
  const [isModalVisible, setIsModalVisible] = useState(false);
  const {
    searchParam,
    setSearchParam,
    users,
    userFound,
    error,
    handleSendFriendRequest,
  } = useUserSearch(onRequestSent);
  const showModal = () => setIsModalVisible(true);
  const handleOk = () => setIsModalVisible(false);
  const handleCancel = () => setIsModalVisible(false);

  return (
    <div>    
      <div className="top-right-button">
        <Button   
            size="default"
            className="custom-float-button"
            onClick={showModal}
            style={{
              top: 37,
              left: 20,
              borderRadius:'5px',
              fontWeight: 500,
              fontSize: '16px'
            }}
          >
            Add Friend
          </Button>
      </div>
      <SearchModal isModalVisible={isModalVisible} handleOk={handleOk} handleCancel={handleCancel}>
        <Input
          placeholder="Search Talk"
          prefix={<SearchOutlined />}
          value={searchParam}
          onChange={(e) => setSearchParam(e.target.value)}
        />
        {userFound && !error && <UserCard users={users} onSendRequest={handleSendFriendRequest} />}
        {error && !userFound && <div className="search-error">{error}</div>}
      </SearchModal>
    </div>
  );
}

export default UserSearch;
