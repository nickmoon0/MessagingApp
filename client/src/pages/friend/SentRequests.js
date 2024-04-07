// src/pages/FriendPage/SentRequests.jsx
import React, {useState} from 'react';
import UserSearch from './UserSearch';
import SentRequestsTable from '../../components/friendsTables/SentRequestsTable';
import useSentRequests from '../../hooks/useSentRequest';
import { FiSearch } from 'react-icons/fi';
import { Input, Button} from 'antd';

const SentRequests = ({ searchText, onSearchChange }) => {
  const { sentRequests, handleRequestSent, handleCancelRequest } = useSentRequests();
  const [showSearch, setShowSearch] = useState(false); 
    
  const handleSearchIconClick = () => {
    setShowSearch(!showSearch); 
};

  const filteredRequests = sentRequests.filter(request => 
    request.username.toLowerCase().includes(searchText.toLowerCase())
  );

  const dataWithKey = filteredRequests.map((request) => ({
    key: request.id,
    username: request.username,
    action: '' 
  }));

  return (
    <>
      <UserSearch onRequestSent={handleRequestSent} />
      <div style={{ display: 'flex',
                      justifyContent: 'flex-end',
                      paddingRight: '120px',
                      paddingTop:'3.5px',
                      borderRadius:'10px',
                      }}>
          {showSearch ? (
                <>
                    <Input.Search
                        placeholder="Search Requests..."
                        onChange={onSearchChange}
                        autoFocus 
                        onBlur={() => setShowSearch(false)}
                        allowClear
                        style={{ width: 300 }}
                    />
                </>
            ) : (
              <Button
              onClick={handleSearchIconClick}
              style={{ backgroundColor: '#2898fb', border: 'none' }} // Set the button's background and border color to blue
              icon={<FiSearch style={{ color: 'white', fontSize: '20px', marginTop:'1px' }} />} // Set the icon's color to white and size to 24px
            />
            )}

      </div>
      <SentRequestsTable data={dataWithKey} onCancelRequest={handleCancelRequest} />
    </>
  );
};

export default SentRequests;
