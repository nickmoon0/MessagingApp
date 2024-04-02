import React, { useState, useEffect } from 'react';
import UserSearch from './UserSearch';
import { Segmented, Card,  notification, Input, Button} from 'antd';
import SentRequests from './SentRequests';
import { acceptFriendRequest } from '../../api/userService';
import useReceivedRequests from '../../hooks/useReceivedRequests';
import ReceivedRequestsTable from '../../components/friendsTables/ReceivedRequestsTable';
import '../../styles/center.css'; 
import './AddFriend.css';
import { useSelectedTab } from '../../context/SelectedTabContext';
import { fetchFriends } from '../../api/userService';
import FriendsListTable from '../../components/friendsTables/FriendsListTable';
import { SearchOutlined } from '@ant-design/icons';


function Friends() {
    const { receivedRequests, handleRequestSent, setReceivedRequests } = useReceivedRequests();
    const [friends, setFriends] = useState([]);
    const { selectedTab, setSelectedTab } = useSelectedTab();
    const [searchText, setSearchText] = useState('');
    const [showSearch, setShowSearch] = useState(false); 
    
    const handleSearchIconClick = () => {
      setShowSearch(!showSearch); 
  };
    useEffect(() => {
      if (selectedTab === 'Friends') {
          refreshFriendsList();
      }
  }, [selectedTab]);

  const onSearchChange = (e) => {
    setSearchText(e.target.value.toLowerCase());
};

// Filter friends based on search text
const filteredFriends = friends.filter(friend => 
    friend.username.toLowerCase().includes(searchText)
);

    useEffect(() => {
        if (selectedTab === 'Friends') {
          refreshFriendsList();
        }
      }, [selectedTab]); 

    const onSegmentChange = (key) => 
    {
        setSelectedTab(key);
    };

    const handleAcceptFriendRequest = async (friendRequestId) => {
        try 
        {
          console.log("Accepting friend request with ID:", friendRequestId);
          await acceptFriendRequest(friendRequestId); 
          notification.success({
            message: 'Friend Request Accepted',
            description: 'The friend request has been successfully accepted.',
            duration: 2.5,
          });
          const updatedRequests = receivedRequests.filter(request => request.friendRequestId !== friendRequestId);
          setReceivedRequests(updatedRequests);
      
          await refreshFriendsList();
        } 
        catch (error) 
        {
          notification.error({
            message: 'Error Accepting Friend Request',
            description: error.message,
            duration: 2.5,
          });
        }
      };
      
      const refreshFriendsList = async () => {
        try 
        {
          const response = await fetchFriends();
          console.log('Refreshed friends list:', response);
          setFriends(response.friends); 
        } 
        catch (error) 
        {
          console.error('Failed to refresh friends list:', error);
        }
      };

      return (
        <div>
            <UserSearch onRequestSent={handleRequestSent} />
            
              <div 
                style={{ 
                  marginLeft: "150px", 
                  position: 'relative'
                  }}>
  
              <div className="w-full flex-col">
              <Segmented 
                      className="custom-segmented"
                      size="default"
                      options={[{
                        label: 'Friends', value: 'Friends'}, 
                        {label: 'Received', value: 'Received'},
                        {label:'Sent',value: 'Sent'}]}
                      value={selectedTab}
                      onChange={onSegmentChange}
                      style={{ 
                        position: 'absolute', 
                        backgroundColor: '#ffffff',
                        left: 165, 
                        top: 70, 
                        zIndex: 1000, 
                        fontWeight: 500,
                        fontSize: '15px',
                        borderRadius: '10px' }}
                  />

            
                  {selectedTab === 'Friends' && (
                    <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                    <div style={{ display: 'flex', justifyContent: 'flex-end', paddingRight: '800px', paddingTop:'6px' }}>
                    {showSearch ? (
                            <>
                                <Input.Search
                                    placeholder="Search friends"
                                    onChange={onSearchChange}
                                    onBlur={() => setShowSearch(false)} // Optionally hide search on blur
                                    autoFocus // Focus on the input when it appears
                                    style={{ width: 300 }}
                                />
                            </>
                        ) : (
                            <Button icon={<SearchOutlined />} onClick={handleSearchIconClick} />
                        )}
                      
                    </div>
                    <FriendsListTable friends={filteredFriends} />
                  </Card>
                )}
                {selectedTab === 'Received' && (
                    <Card  className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                        <ReceivedRequestsTable requests={receivedRequests} onAccept={handleAcceptFriendRequest} />
                    </Card>
                )}
                {selectedTab === 'Sent' && (
                    <Card className="fixed-size-card" style={{ border: 'none', boxShadow: 'none' }}>
                        <SentRequests />
                    </Card>
                )}
            </div>
            </div>
        </div>
    );
}

export default Friends;
